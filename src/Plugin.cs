using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using UnityEngine;
using UnityEngine.Rendering;

namespace Sirroh.FurRandomizer
{
    [BepInPlugin("sirroh.furrandomizer", "Sirroh Fur Randomizer", "0.1.0")]
    public sealed class Plugin : BaseUnityPlugin
    {
        internal static ManualLogSource Log;
        private Harmony _harmony;

        private void Awake()
        {
            Log = Logger;
            try
            {
                // Dedicated servers still set the exact same ZDO indices as clients.
                if (SystemInfo.graphicsDeviceType != GraphicsDeviceType.Null)
                {
                    MaterialCatalog.Load();
                }
                _harmony = new Harmony("sirroh.furrandomizer");
                _harmony.PatchAll(typeof(Plugin).Assembly);
                Logger.LogInfo("Skin randomization initialized (no global shader/level patches).");
            }
            catch (Exception exception)
            {
                Logger.LogError("Initialization failed: " + exception);
                _harmony?.UnpatchSelf();
            }
        }

        private void OnDestroy()
        {
            _harmony?.UnpatchSelf();
            MaterialCatalog.Release();
        }

        // One callback for newly initialized characters; no Update polling for every mob.
        [HarmonyPatch(typeof(Character), "Start")]
        private static class CharacterStartPatch
        {
            private static void Postfix(Character __instance)
            {
                if (!__instance || !__instance.gameObject)
                    return;

                string prefabName = __instance.gameObject.name;
                int cloneMarker = prefabName.IndexOf('(');
                if (cloneMarker >= 0)
                    prefabName = prefabName.Substring(0, cloneMarker);

                if (!MaterialCatalog.IsSupported(prefabName))
                    return;

                // Avoid duplicate components if a companion mod also calls the patch.
                if (!__instance.GetComponent<SkinAssignment>())
                    __instance.gameObject.AddComponent<SkinAssignment>();
            }
        }
    }

    /// <summary>
    /// One short-lived coroutine per spawned animal. No per-frame Update or global scan.
    /// Clients wait for the authoritative owner to publish a variant in ZDO.
    /// </summary>
    public sealed class SkinAssignment : MonoBehaviour
    {
        // Keep the old ZDO keys to preserve existing worlds and variants.
        private const string SkinKey = "furRandomizerSet";
        private const string ActiveKey = "furRandomizerSetActive";
        private static readonly WaitForSeconds RetryDelay = new WaitForSeconds(0.2f);
        private const int MaxAttempts = 300;

        private IEnumerator Start()
        {
            string animal = gameObject.name;
            int cloneMarker = animal.IndexOf('(');
            if (cloneMarker >= 0)
                animal = animal.Substring(0, cloneMarker);

            if (!MaterialCatalog.IsSupported(animal))
            {
                Destroy(this);
                yield break;
            }

            ZNetView view = GetComponent<ZNetView>();
            if (!view)
            {
                Destroy(this);
                yield break;
            }

            // On dedicated servers no graphics material loading is necessary.
            bool headless = SystemInfo.graphicsDeviceType == GraphicsDeviceType.Null;
            for (int attempt = 0; attempt < MaxAttempts; attempt++)
            {
                if (!view || !gameObject)
                    yield break;

                if (!view.IsValid())
                {
                    yield return RetryDelay;
                    continue;
                }

                ZDO zdo = view.GetZDO();
                if (zdo == null)
                {
                    yield return RetryDelay;
                    continue;
                }

                int selected;
                if (zdo.GetBool(ActiveKey, false))
                {
                    selected = zdo.GetInt(SkinKey, 0);
                    // Do not write an already assigned skin again: minimizes ZDO traffic.
                    if (!headless)
                        MaterialCatalog.TryApply(gameObject, animal, selected);
                    Destroy(this);
                    yield break;
                }

                if (!view.IsOwner())
                {
                    yield return RetryDelay;
                    continue;
                }

                selected = MaterialCatalog.RandomIndex(animal);
                if (selected >= 0)
                {
                    // Index first, then active flag; remote clients read after both.
                    zdo.Set(SkinKey, selected);
                    zdo.Set(ActiveKey, true);
                    if (!headless)
                        MaterialCatalog.TryApply(gameObject, animal, selected);
                }

                Destroy(this);
                yield break;
            }

            // No fabricated local random skin on a non-owning client.
            if (Plugin.Log != null)
                Plugin.Log.LogDebug("Skin data not received for " + animal + "; keeping vanilla.");
            Destroy(this);
        }
    }

    internal sealed class Variant
    {
        internal Material Main;
        internal Material Body;
        internal Material Face;
        internal Material Fur;
    }

    /// <summary>
    /// Loads each Unity Material exactly once and shares it across renderers.
    /// No 'new Material' per spawn, no shader inventory scan, no global renderer edits.
    /// </summary>
    internal static class MaterialCatalog
    {
        private static AssetBundle _bundle;
        private static readonly Dictionary<string, Variant[]> Variants =
            new Dictionary<string, Variant[]>(StringComparer.OrdinalIgnoreCase);
        private static readonly HashSet<Material> ShaderChecked = new HashSet<Material>();
        private static readonly HashSet<string> Warnings = new HashSet<string>();

        // This order matches the existing saved ZDO numeric indices. Do not reorder.
        private static readonly Dictionary<string, string[]> VariantNames =
            new Dictionary<string, string[]>(StringComparer.OrdinalIgnoreCase)
            {
                { "Lox", new[] { "Default", "Albino", "Black", "Blonde", "DarkBrown", "Ginger" } },
                { "Wolf", new[] { "Default", "Albino", "Black", "DarkBrown", "Grey" } },
                { "Boar", new[] { "Default", "Grey", "Black", "DarkBrown", "Young" } },
                { "Deer", new[] { "Default", "Albino", "Red", "LightBrown", "Grey" } },
                { "Asksvin", new[] { "Default", "Albino", "Black", "Purple", "Young" } },
                { "Neck", new[] { "Default", "Albino", "DarkBlue", "DarkGreen", "Blue" } },
                { "Hare", new[] { "Default", "Albino", "Black", "DarkBlue", "Ginger" } },
                { "Hatchling", new[] { "Default", "Red", "DarkBlue", "IceBlue", "Purple" } },
                { "Serpent", new[] { "Default", "Albino", "Black", "Blue", "Green", "Red" } }
            };

        internal static bool IsSupported(string name) => VariantNames.ContainsKey(name);

        internal static int RandomIndex(string animal)
        {
            if (!VariantNames.TryGetValue(animal, out string[] names))
                return -1;
            return UnityEngine.Random.Range(0, names.Length);
        }

        internal static void Load()
        {
            if (_bundle != null)
                return;

            var assembly = Assembly.GetExecutingAssembly();
            // Use renamed embedded resource; the original Unity asset internal paths
            // can remain unchanged without referencing their historical root names.
            using (Stream stream = assembly.GetManifestResourceStream("SirrohFurRandomizer.furrandomizer"))
            {
                if (stream == null)
                    throw new FileNotFoundException("Embedded skin AssetBundle not found.");
                _bundle = AssetBundle.LoadFromStream(stream);
            }

            if (_bundle == null)
                throw new InvalidOperationException("Could not load embedded skin AssetBundle.");

            // Only read the AssetBundle directory once at plugin initialization.
            var byAnimal = new Dictionary<string, Dictionary<string, Variant>>(StringComparer.OrdinalIgnoreCase);
            string[] paths = _bundle.GetAllAssetNames();
            for (int i = 0; i < paths.Length; i++)
            {
                string path = paths[i];
                if (!path.EndsWith(".mat", StringComparison.OrdinalIgnoreCase))
                    continue;

                string[] segments = path.Replace('\\', '/').Split('/');
                if (segments.Length < 3)
                    continue;

                string folder = segments[segments.Length - 3];
                string variantName = segments[segments.Length - 2];
                string filename = segments[segments.Length - 1];
                string animal = folder.Equals("Drake", StringComparison.OrdinalIgnoreCase)
                    ? "Hatchling" : folder;

                if (!VariantNames.ContainsKey(animal))
                    continue;

                Material material = _bundle.LoadAsset<Material>(path);
                if (!material)
                    continue;

                if (!byAnimal.TryGetValue(animal, out Dictionary<string, Variant> skins))
                {
                    skins = new Dictionary<string, Variant>(StringComparer.OrdinalIgnoreCase);
                    byAnimal.Add(animal, skins);
                }

                if (!skins.TryGetValue(variantName, out Variant set))
                {
                    set = new Variant();
                    skins.Add(variantName, set);
                }

                // Only Lox uses 3 separate material slots.
                if (animal.Equals("Lox", StringComparison.OrdinalIgnoreCase))
                {
                    if (filename.Equals("lox_face.mat", StringComparison.OrdinalIgnoreCase))
                        set.Face = material;
                    else if (filename.Equals("lox_body1.mat", StringComparison.OrdinalIgnoreCase))
                        set.Body = material;
                    else if (filename.Equals("lox_fur1.mat", StringComparison.OrdinalIgnoreCase))
                        set.Fur = material;
                }
                else
                {
                    set.Main = material;
                }
            }

            int available = 0;
            foreach (var entry in VariantNames)
            {
                Variant[] sorted = new Variant[entry.Value.Length];
                byAnimal.TryGetValue(entry.Key, out Dictionary<string, Variant> skinMap);
                for (int index = 0; index < sorted.Length; index++)
                {
                    Variant value;
                    if (skinMap != null && skinMap.TryGetValue(entry.Value[index], out value))
                    {
                        if (entry.Key == "Lox" ? (value.Body && value.Face && value.Fur) : value.Main)
                        {
                            sorted[index] = value;
                            available++;
                        }
                    }
                    if (sorted[index] == null)
                        WarnOnce(entry.Key + "/" + entry.Value[index], "Missing skin material: " + entry.Key + "/" + entry.Value[index]);
                }
                Variants[entry.Key] = sorted;
            }
            Plugin.Log?.LogInfo("Loaded " + available + " skin variants using shared material instances.");
        }

        internal static bool TryApply(GameObject animalObject, string animal, int index)
        {
            if (!animalObject || !Variants.TryGetValue(animal, out Variant[] skins))
                return false;

            if (index < 0 || index >= skins.Length)
                index = 0;
            Variant skin = skins[index] ?? skins[0];
            if (skin == null)
                return false;

            try
            {
                SkinnedMeshRenderer[] renderers = animalObject.GetComponentsInChildren<SkinnedMeshRenderer>(true);
                if (renderers == null || renderers.Length == 0)
                {
                    WarnOnce(animal + "/render", "No skinned renderer found on " + animal);
                    return false;
                }

                if (animal.Equals("Lox", StringComparison.OrdinalIgnoreCase))
                    return ApplyLox(renderers, skin);

                if (animal.Equals("Hatchling", StringComparison.OrdinalIgnoreCase))
                {
                    bool assigned = false;
                    for (int i = 0; i < renderers.Length; i++)
                    {
                        string rendererName = renderers[i].name;
                        if (rendererName == "Hatchling" || rendererName == "Hatchling.001")
                        {
                            Assign(renderers[i], 0, skin.Main);
                            assigned = true;
                        }
                    }
                    return assigned;
                }

                // Names from the original vanilla models avoid changing unrelated meshes.
                SkinnedMeshRenderer target = null;
                if (animal.Equals("Boar", StringComparison.OrdinalIgnoreCase) ||
                    animal.Equals("Neck", StringComparison.OrdinalIgnoreCase))
                {
                    string expected = animal.Equals("Boar", StringComparison.OrdinalIgnoreCase)
                        ? "Poly Art Boar" : "Body";
                    for (int i = 0; i < renderers.Length; i++)
                        if (renderers[i].name == expected)
                        {
                            target = renderers[i];
                            break;
                        }
                }

                if (!target)
                    target = renderers[0];

                Assign(target, 0, skin.Main);
                return true;
            }
            catch (Exception exception)
            {
                WarnOnce(animal + "/apply", "Material apply failed for " + animal + ": " + exception);
                return false;
            }
        }

        private static bool ApplyLox(SkinnedMeshRenderer[] renderers, Variant skin)
        {
            SkinnedMeshRenderer body = null;
            SkinnedMeshRenderer fur = null;
            for (int i = 0; i < renderers.Length; i++)
            {
                if (renderers[i].name == "Body")
                    body = renderers[i];
                else if (renderers[i].name == "Furr1")
                    fur = renderers[i];
            }
            if (!body || !fur)
            {
                WarnOnce("Lox/render", "Lox Body/Furr1 renderer not found; keeping vanilla.");
                return false;
            }

            Assign(body, 0, skin.Body);
            Assign(body, 1, skin.Face);
            Assign(fur, 0, skin.Fur);
            return true;
        }

        private static void Assign(SkinnedMeshRenderer renderer, int slot, Material selected)
        {
            if (!renderer || !selected)
                return;

            // Resolve shader against a known-good live vanilla material, once per variant.
            // Avoid Resources.FindObjectsOfTypeAll, LoadAllAssets<Shader>, and scene scans.
            Material[] slots = renderer.sharedMaterials;
            if (slots == null || slot >= slots.Length)
                return;

            Material original = slots[slot];
            if (ShaderChecked.Add(selected) && original && original.shader)
                selected.shader = original.shader;

            if (slots[slot] == selected)
                return;
            if (slots.Length == 1)
                renderer.sharedMaterial = selected;
            else
            {
                slots[slot] = selected;
                renderer.sharedMaterials = slots;
            }
        }

        private static void WarnOnce(string key, string message)
        {
            if (Warnings.Add(key))
                Plugin.Log?.LogWarning(message);
        }

        internal static void Release()
        {
            // Assets continue to exist while referenced by active renderers.
            if (_bundle)
                _bundle.Unload(false);
            _bundle = null;
            Variants.Clear();
            ShaderChecked.Clear();
            Warnings.Clear();
        }
    }
}
