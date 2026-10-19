using System.Collections.Generic;
using BepInEx;
using HarmonyLib;
using UnityEngine;

namespace BalrondFurRandomizer;

[BepInPlugin("balrond.astafaraios.BalrondFurRandomizer", "BalrondFurRandomizer", "1.0.7")]
public class Launch : BaseUnityPlugin
{
	[HarmonyPatch(typeof(Game), "Awake")]
	public static class Game_Awake_Patch
	{
		public static void Prefix()
		{
			ZNetScene_Awake_Patch.hasSpawned = false;
		}
	}

	[HarmonyPatch(typeof(ObjectDB), "CopyOtherDB")]
	public static class ObjectDB_CopyOtherDB_Patch
	{
		public static void Postfix()
		{
			if (IsObjectDBValid())
			{
			}
		}
	}

	[HarmonyPatch(typeof(ObjectDB), "Awake")]
	public static class ObjectDB_Awake_Patch
	{
		public static void Postfix()
		{
			if (IsObjectDBValid())
			{
			}
		}
	}

	[HarmonyPatch(typeof(Character), "OnDeath")]
	public static class Character_OnDeath_Patch
	{
		public static void Prefix(Character __instance)
		{
			if ((Object)(object)__instance == (Object)null)
			{
				Debug.LogWarning((object)"Character instance does not exist yet");
			}
			else
			{
				if (__instance.m_deathEffects == null || __instance.m_deathEffects.m_effectPrefabs == null)
				{
					return;
				}
				LevelEffects componentInChildren = ((Component)__instance).GetComponentInChildren<LevelEffects>();
				if ((Object)(object)componentInChildren == (Object)null || (Object)(object)componentInChildren.m_mainRender == (Object)null)
				{
					return;
				}
				Material[] sharedMaterials = componentInChildren.m_mainRender.sharedMaterials;
				if (sharedMaterials == null || sharedMaterials.Length == 0)
				{
					return;
				}
				EffectData[] effectPrefabs = __instance.m_deathEffects.m_effectPrefabs;
				foreach (EffectData val in effectPrefabs)
				{
					if (val != null && !((Object)(object)val.m_prefab == (Object)null))
					{
						Ragdoll component = val.m_prefab.GetComponent<Ragdoll>();
						if ((Object)(object)component != (Object)null && (Object)(object)component.m_mainModel != (Object)null)
						{
							component.m_mainModel.sharedMaterials = sharedMaterials;
						}
					}
				}
			}
		}
	}

	[HarmonyPatch(typeof(LevelEffects), "SetupLevelVisualization")]
	public static class LevelEffects_SetupLevelVisualization_Patch
	{
		public static bool Prefix(LevelEffects __instance, int level)
		{
			//IL_005e: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b9: Expected Obj, but got Unknown
			//IL_010f: Unknown result type (might be due to invalid IL or missing references)
			if ((Object)(object)__instance == (Object)null)
			{
				return false;
			}
			if (level <= 1 || __instance.m_levelSetups.Count < level - 1)
			{
				return false;
			}
			LevelSetup val = __instance.m_levelSetups[level - 2];
			((Component)__instance).transform.localScale = new Vector3(val.m_scale, val.m_scale, val.m_scale);
			if ((Object)(object)__instance.m_mainRender != (Object)null)
			{
				Material[] sharedMaterials = __instance.m_mainRender.sharedMaterials;
				if (sharedMaterials != null && sharedMaterials.Length != 0 && (Object)(object)sharedMaterials[0] != (Object)null)
				{
					sharedMaterials[0] = new Material(sharedMaterials[0]);
					sharedMaterials[0].SetFloat("_Hue", val.m_hue);
					sharedMaterials[0].SetFloat("_Saturation", val.m_saturation);
					sharedMaterials[0].SetFloat("_Value", val.m_value);
					if (val.m_setEmissiveColor)
					{
						sharedMaterials[0].SetColor("_EmissionColor", val.m_emissiveColor);
					}
					__instance.m_mainRender.sharedMaterials = sharedMaterials;
					if ((Object)(object)__instance.m_character != (Object)null)
					{
						string key = Utils.GetPrefabName(((Component)__instance.m_character).gameObject) + level;
						LevelEffects.m_materials[key] = sharedMaterials[0];
					}
				}
			}
			if ((Object)(object)__instance.m_baseEnableObject != (Object)null)
			{
				__instance.m_baseEnableObject.SetActive(false);
			}
			if ((Object)(object)val.m_enableObject != (Object)null)
			{
				val.m_enableObject.SetActive(true);
			}
			return false;
		}
	}

	[HarmonyPatch(typeof(ZNetScene), "Awake")]
	public static class ZNetScene_Awake_Patch
	{
		public static bool hasSpawned;

		public static void Prefix(ZNetScene __instance)
		{
			if ((Object)(object)__instance == (Object)null)
			{
				Debug.LogWarning((object)"BalrondFurRandomizer: No ZNetScene found");
			}
			else if (!hasSpawned)
			{
				itemSetBuilder.CreateMaterialPacks(modResourceLoader);
				SetupFurRandomizers(__instance.m_prefabs);
				if ((Object)(object)ZNet.instance != (Object)null && ZNet.instance.IsDedicated())
				{
					hasSpawned = true;
					return;
				}
				ShaderReplacment.runMaterialFix();
				hasSpawned = true;
			}
		}

		private static void SetupFurRandomizers(List<GameObject> prefabs)
		{
			if (prefabs == null || prefabs.Count == 0)
			{
				Debug.LogWarning((object)"BalrondFurRandomizer: Prefab list is empty");
				return;
			}
			string[] array = new string[9] { "Wolf", "Neck", "Hare", "Serpent", "Hatchling", "Lox", "Deer", "Boar", "Asksvin" };
			string[] array2 = array;
			foreach (string targetName in array2)
			{
				GameObject monster = prefabs.Find((GameObject x) => (Object)(object)x != (Object)null && ((Object)x).name == targetName);
				itemSetBuilder.SetupFurRandomizerForMonster(monster);
			}
		}
	}

	private readonly Harmony harmony = new Harmony("balrond.astafaraios.BalrondFurRandomizer");

	public const string PluginGUID = "balrond.astafaraios.BalrondFurRandomizer";

	public const string PluginName = "BalrondFurRandomizer";

	public const string PluginVersion = "1.0.7";

	public static ModResourceLoader modResourceLoader = new ModResourceLoader();

	public static DatabaseAddMethods databaseAddMethods = new DatabaseAddMethods();

	public static MonsterManager monsterManager = new MonsterManager();

	public static ItemSetBuilder itemSetBuilder = new ItemSetBuilder();

	public static GameObject gui;

	public static GameObject gui2;

	public static GameObject RootObject;

	public static GameObject PrefabContainer;

	private void Awake()
	{
		CreatePrefabContainer();
		modResourceLoader.loadAssets();
		harmony.PatchAll();
	}

	private void OnDestroy()
	{
		harmony.UnpatchSelf();
	}

	public void CreatePrefabContainer()
	{
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Expected Obj, but got Unknown
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Expected Obj, but got Unknown
		if (!((Object)(object)RootObject != (Object)null) || !((Object)(object)PrefabContainer != (Object)null))
		{
			RootObject = new GameObject("_ValheimReforgedRoot");
			Object.DontDestroyOnLoad((Object)(object)RootObject);
			PrefabContainer = new GameObject("Prefabs");
			PrefabContainer.transform.SetParent(RootObject.transform, false);
			PrefabContainer.SetActive(false);
		}
	}

	public static GameObject CloneMe(GameObject source, string name)
	{
		if ((Object)(object)source == (Object)null)
		{
			Debug.LogWarning((object)"BalrondFurRandomizer: CloneMe source is null");
			return null;
		}
		if ((Object)(object)PrefabContainer == (Object)null)
		{
			Debug.LogWarning((object)"BalrondFurRandomizer: PrefabContainer is null");
			return null;
		}
		GameObject val = Object.Instantiate<GameObject>(source, PrefabContainer.transform);
		((Object)val).name = name;
		return val;
	}

	private static bool IsObjectDBValid()
	{
		return (Object)(object)ObjectDB.instance != (Object)null && ObjectDB.instance.m_items != null && ObjectDB.instance.m_recipes != null && ObjectDB.instance.m_items.Count != 0 && ObjectDB.instance.m_recipes.Count != 0 && (Object)(object)ObjectDB.instance.GetItemPrefab("Amber") != (Object)null;
	}

	public static void AddStatusToAttackField(GameObject gameObject, StatusEffect statusEffect)
	{
		if (!((Object)(object)gameObject == (Object)null) && !((Object)(object)statusEffect == (Object)null))
		{
			ItemDrop component = gameObject.GetComponent<ItemDrop>();
			if ((Object)(object)component == (Object)null)
			{
				Debug.LogWarning((object)("BalrondFurRandomizer: ItemDrop not found on " + ((Object)gameObject).name));
			}
			else
			{
				component.m_itemData.m_shared.m_attackStatusEffect = statusEffect;
			}
		}
	}

	public Launch()
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Expected Obj, but got Unknown
	}
}
