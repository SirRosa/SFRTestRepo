using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BalrondFurRandomizer;

public class ShaderReplacment
{
	public static List<GameObject> prefabsToReplaceShader = new List<GameObject>();

	public static List<Material> materialsInPrefabs = new List<Material>();

	public string[] shaderlist = new string[49]
	{
		"Custom/AlphaParticle", "Custom/Blob", "Custom/Bonemass", "Custom/Clouds", "Custom/Creature", "Custom/Decal", "Custom/Distortion", "Custom/Flow", "Custom/FlowOpaque", "Custom/Grass",
		"Custom/GuiScroll", "Custom/Heightmap", "Custom/icon", "Custom/InteriorSide", "Custom/LitGui", "Custom/LitParticles", "Custom/mapshader", "Custom/ParticleDecal", "Custom/Piece", "Custom/Player",
		"Custom/Rug", "Custom/ShadowBlob", "Custom/SkyboxProcedural", "Custom/SkyObject", "Custom/StaticRock", "Custom/Tar", "Custom/Trilinearmap", "Custom/UI/BGBlur", "Custom/Vegetation", "Custom/Water",
		"Custom/WaterBottom", "Custom/WaterMask", "Custom/Yggdrasil", "Custom/Yggdrasil/root", "Hidden/BlitCopyHDRTonemap", "Hidden/Dof/DepthOfFieldHdr", "Hidden/Dof/DX11Dof", "Hidden/Internal-Loading", "Hidden/Internal-UIRDefaultWorld", "Hidden/SimpleClear",
		"Hidden/SunShaftsComposite", "Lux Lit Particles/ Bumped", "Lux Lit Particles/ Tess Bumped", "Particles/Standard Surface2", "Particles/Standard Unlit2", "Standard TwoSided", "ToonDeferredShading2017", "Unlit/DepthWrite", "Unlit/Lighting"
	};

	public static List<Shader> shaders = new List<Shader>();

	private static readonly HashSet<Shader> CachedShaders = new HashSet<Shader>();

	public static bool debug = true;

	public static Shader findShader(string name)
	{
		Shader[] array = Resources.FindObjectsOfTypeAll<Shader>();
		if (array.Length == 0)
		{
			Debug.LogWarning((object)"SHADER LIST IS EMPTY!");
			return null;
		}
		if (debug)
		{
		}
		return shaders.Find((Shader x) => ((Object)x).name == name);
	}

	public static void ReplaceMaterial(Material material)
	{
		if ((Object)(object)material == (Object)null)
		{
			Debug.LogWarning((object)"Material is empty !");
		}
		else
		{
			materialsInPrefabs.Add(material);
		}
	}

	public static Shader GetShaderByName(string name)
	{
		return shaders.Find((Shader x) => ((Object)x).name == name.Trim());
	}

	public static void debugShaderList(List<Shader> shadersRes)
	{
		foreach (Shader shadersRe in shadersRes)
		{
			Debug.LogWarning((object)("SHADER NAME IS: " + ((Object)shadersRe).name));
		}
		debug = false;
	}

	public static void Replace(GameObject gameObject)
	{
		prefabsToReplaceShader.Add(gameObject);
		GetMaterialsInPrefab(gameObject);
	}

	public static void GetMaterialsInPrefab(GameObject gameObject)
	{
		Renderer[] componentsInChildren = gameObject.GetComponentsInChildren<Renderer>(true);
		Renderer[] array = componentsInChildren;
		foreach (Renderer val in array)
		{
			Material[] sharedMaterials = val.sharedMaterials;
			if (sharedMaterials == null || sharedMaterials.Length == 0)
			{
				continue;
			}
			Material[] array2 = sharedMaterials;
			foreach (Material val2 in array2)
			{
				if ((Object)(object)val2 != (Object)null)
				{
					materialsInPrefabs.Add(val2);
				}
			}
		}
	}

	public static void getMeShaders()
	{
		AssetBundle[] array = Resources.FindObjectsOfTypeAll<AssetBundle>();
		AssetBundle[] array2 = array;
		foreach (AssetBundle val in array2)
		{
			IEnumerable<Shader> enumerable3;
			try
			{
				IEnumerable<Shader> enumerable2;
				if (!val.isStreamedSceneAssetBundle || !Object.op_Implicit((Object)(object)val))
				{
					IEnumerable<Shader> enumerable = val.LoadAllAssets<Shader>();
					enumerable2 = enumerable;
				}
				else
				{
					enumerable2 = from shader in ((IEnumerable<string>)val.GetAllAssetNames()).Select((Func<string, Shader>)val.LoadAsset<Shader>)
						where (Object)(object)shader != (Object)null
						select shader;
				}
				enumerable3 = enumerable2;
			}
			catch (Exception)
			{
				continue;
			}
			if (enumerable3 == null)
			{
				continue;
			}
			foreach (Shader item in enumerable3)
			{
				CachedShaders.Add(item);
			}
		}
	}

	public static void runMaterialFix()
	{
		getMeShaders();
		shaders.AddRange(CachedShaders);
		foreach (Material materialsInPrefab in materialsInPrefabs)
		{
			if ((Object)(object)materialsInPrefab == (Object)null)
			{
				Debug.Log((object)"I had a null in list");
				continue;
			}
			Shader shader = materialsInPrefab.shader;
			if (!((Object)(object)shader == (Object)null))
			{
				string name = ((Object)shader).name;
				if (!(name == "Standard") && name.Contains("Balrond"))
				{
					setProperValue(materialsInPrefab, name);
				}
			}
		}
	}

	private static void setProperValue(Material material, string shaderName)
	{
		string name = shaderName.Replace("Balrond", "Custom");
		name = checkNaming(name);
		Shader shaderByName = GetShaderByName(name);
		if ((Object)(object)shaderByName == (Object)null)
		{
			Debug.LogWarning((object)("Shader not found " + name));
		}
		else
		{
			material.shader = shaderByName;
		}
	}

	private static string checkNaming(string name)
	{
		string result = name;
		if (name.Contains("Bumped"))
		{
			result = name.Replace("Custom", "Lux Lit Particles");
		}
		if (name.Contains("Tess Bumped"))
		{
			result = name.Replace("Custom", "Lux Lit Particles");
		}
		if (name.Contains("Standard Surface"))
		{
			result = name.Replace("Custom", "Particles");
			result = result.Replace("Standard Surface2", "Standard Surface");
		}
		if (name.Contains("Standard Unlit"))
		{
			result = name.Replace("Custom", "Particles");
			result = result.Replace("Standard Unlit", "Standard Unlit2");
			result = result.Replace("Standard Unlit22", "Standard Unlit2");
		}
		return result;
	}
}
