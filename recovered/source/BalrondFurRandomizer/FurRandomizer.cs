using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BalrondFurRandomizer;

[Serializable]
public class FurRandomizer : MonoBehaviour
{
	private static readonly string[] ModelsToChange = new string[9] { "Wolf", "Lox", "Serpent", "Neck", "Hatchling", "Hare", "Boar", "Deer", "Asksvin" };

	private ZNetView m_zview;

	private MonsterAI m_monsterAI;

	private AnimalAI m_animalAI;

	private Humanoid m_humanoid;

	public List<MaterialPack> m_materials = new List<MaterialPack>();

	private string skinName;

	private MaterialPack m_materialPack;

	public SkinnedMeshRenderer face;

	public SkinnedMeshRenderer body;

	public SkinnedMeshRenderer fur;

	public static ModResourceLoader modResourceLoader = Launch.modResourceLoader;

	private readonly List<MaterialPack> materialPacks = ItemSetBuilder.materialPacks;

	private const string ZdoSetKey = "furRandomizerSet";

	private const string ZdoActiveKey = "furRandomizerSetActive";

	private void Awake()
	{
		if (!ValidatePrefab())
		{
			Debug.LogWarning((object)("I should not be randomized: " + ((Object)((Component)this).gameObject).name));
			return;
		}
		if (modResourceLoader == null)
		{
			Debug.LogWarning((object)"ModResourceLoader is null");
			return;
		}
		if ((Object)(object)modResourceLoader.drakeBody_DarkBlue == (Object)null)
		{
			Debug.LogWarning((object)"Nothing loaded");
		}
		m_zview = ((Component)this).GetComponent<ZNetView>();
		m_monsterAI = ((Component)this).GetComponent<MonsterAI>();
		m_animalAI = ((Component)this).GetComponent<AnimalAI>();
		m_humanoid = ((Component)this).GetComponent<Humanoid>();
		if ((Object)(object)m_zview == (Object)null || m_zview.m_zdo == null)
		{
			Debug.LogWarning((object)("Missing ZNetView or ZDO on " + ((Object)((Component)this).gameObject).name));
			return;
		}
		CreateMaterialPacksOnce();
		if (SetupFurRandomizerForMonster())
		{
			if (m_materials == null || m_materials.Count == 0)
			{
				Debug.LogWarning((object)("Material list is empty for " + ((Object)((Component)this).gameObject).name));
				return;
			}
			bool chosen = m_zview.m_zdo.GetBool("furRandomizerSetActive", false);
			PickRandomSet(chosen);
		}
	}

	private bool ValidatePrefab()
	{
		string value = EditZnetName(((Object)((Component)this).gameObject).name);
		return Enumerable.Contains(ModelsToChange, value);
	}

	private string EditZnetName(string name)
	{
		name = name.Replace("(Clone)", "");
		int num = name.IndexOf("(");
		if (num >= 0)
		{
			name = name.Substring(0, num);
		}
		return name.Trim();
	}

	private void PickRandomSet(bool chosen = false)
	{
		if (m_materials == null || m_materials.Count == 0)
		{
			Debug.LogWarning((object)("No material packs available for " + ((Object)((Component)this).gameObject).name));
			return;
		}
		int num = -1;
		if (chosen)
		{
			num = m_zview.m_zdo.GetInt("furRandomizerSet", 0);
		}
		if (!chosen || num < 0 || num >= m_materials.Count)
		{
			num = Random.Range(0, m_materials.Count);
		}
		m_zview.m_zdo.Set("furRandomizerSet", num);
		m_zview.m_zdo.Set("furRandomizerSetActive", true);
		MaterialPack materialPack = m_materials[num];
		skinName = materialPack.skin;
		m_materialPack = materialPack;
		SetSkin(materialPack);
	}

	private void SetSkin(MaterialPack pack)
	{
		switch (EditZnetName(((Object)((Component)this).gameObject).name))
		{
		case "Lox":
			ApplyLoxMaterials(pack);
			break;
		case "Hare":
		case "Serpent":
		case "Neck":
		case "Wolf":
		case "Boar":
		case "Deer":
		case "Asksvin":
			ApplySingleRendererMaterial(body, pack.face);
			break;
		case "Hatchling":
			ApplyHatchlingMaterials(pack);
			break;
		}
	}

	private void ApplyLoxMaterials(MaterialPack pack)
	{
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Expected Obj, but got Unknown
		//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ad: Expected Obj, but got Unknown
		//IL_00db: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e1: Expected Obj, but got Unknown
		//IL_0110: Unknown result type (might be due to invalid IL or missing references)
		//IL_0116: Expected Obj, but got Unknown
		//IL_0147: Unknown result type (might be due to invalid IL or missing references)
		//IL_014d: Expected Obj, but got Unknown
		//IL_017f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0185: Expected Obj, but got Unknown
		if ((Object)(object)face == (Object)null || (Object)(object)body == (Object)null || (Object)(object)fur == (Object)null)
		{
			Debug.LogWarning((object)("Lox renderers missing on " + ((Object)((Component)this).gameObject).name));
			return;
		}
		Material[] materials = ((Renderer)face).materials;
		if (materials.Length != 0)
		{
			materials[0] = new Material(pack.body);
			((Renderer)face).materials = materials;
		}
		Material[] materials2 = ((Renderer)body).materials;
		if (materials2.Length != 0)
		{
			materials2[0] = new Material(pack.body);
			((Renderer)body).materials = materials2;
		}
		Material[] materials3 = ((Renderer)fur).materials;
		if (materials3.Length != 0)
		{
			materials3[0] = new Material(pack.body2);
			((Renderer)fur).materials = materials3;
		}
		Material[] sharedMaterials = ((Renderer)face).sharedMaterials;
		if (sharedMaterials.Length > 1)
		{
			sharedMaterials[1] = new Material(pack.face);
			((Renderer)face).sharedMaterials = sharedMaterials;
		}
		Material[] sharedMaterials2 = ((Renderer)body).sharedMaterials;
		if (sharedMaterials2.Length != 0)
		{
			sharedMaterials2[0] = new Material(pack.body);
			((Renderer)body).sharedMaterials = sharedMaterials2;
		}
		Material[] sharedMaterials3 = ((Renderer)fur).sharedMaterials;
		if (sharedMaterials3.Length != 0)
		{
			sharedMaterials3[0] = new Material(pack.body2);
			((Renderer)fur).sharedMaterials = sharedMaterials3;
		}
	}

	private void ApplyHatchlingMaterials(MaterialPack pack)
	{
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Expected Obj, but got Unknown
		if ((Object)(object)face == (Object)null || (Object)(object)body == (Object)null || (Object)(object)fur == (Object)null)
		{
			Debug.LogWarning((object)("Hatchling renderers missing on " + ((Object)((Component)this).gameObject).name));
			return;
		}
		Material[] materials = ((Renderer)face).materials;
		if (materials.Length != 0)
		{
			materials[0] = new Material(pack.body);
		}
		((Renderer)face).materials = materials;
		((Renderer)body).materials = materials;
		((Renderer)fur).materials = materials;
		((Renderer)face).sharedMaterials = materials;
		((Renderer)body).sharedMaterials = materials;
		((Renderer)fur).sharedMaterials = materials;
	}

	private void ApplySingleRendererMaterial(SkinnedMeshRenderer renderer, Material source)
	{
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Expected Obj, but got Unknown
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Expected Obj, but got Unknown
		if ((Object)(object)renderer == (Object)null || (Object)(object)source == (Object)null)
		{
			Debug.LogWarning((object)("Renderer or source material missing on " + ((Object)((Component)this).gameObject).name));
			return;
		}
		((Renderer)renderer).sharedMaterial = new Material(source);
		((Renderer)renderer).material = new Material(source);
	}

	private void CreateMaterialPacksOnce()
	{
		if (materialPacks == null)
		{
			Debug.LogWarning((object)"materialPacks list is null");
		}
		else if (materialPacks.Count <= 0)
		{
			BuildPack(modResourceLoader.loxFace_Default, modResourceLoader.loxtBody_Default, modResourceLoader.loxFur_Default, "Lox", "Default");
			BuildPack(modResourceLoader.loxFace_Albino, modResourceLoader.loxtBody_Albino, modResourceLoader.loxFur_Albino, "Lox", "Albino");
			BuildPack(modResourceLoader.loxFace_Black, modResourceLoader.loxtBody_Black, modResourceLoader.loxFur_Black, "Lox", "Black");
			BuildPack(modResourceLoader.loxFace_Blond, modResourceLoader.loxtBody_Blond, modResourceLoader.loxFur_Blond, "Lox", "Blond");
			BuildPack(modResourceLoader.loxFace_DarkBrown, modResourceLoader.loxtBody_DarkBrown, modResourceLoader.loxFur_DarkBrown, "Lox", "Brown");
			BuildPack(modResourceLoader.loxFace_Ginger, modResourceLoader.loxtBody_Ginger, modResourceLoader.loxFur_Ginger, "Lox", "Ginger");
			BuildPack(modResourceLoader.wolfBody_Default, modResourceLoader.wolfBody_Default, modResourceLoader.wolfBody_Default, "Wolf", "Default");
			BuildPack(modResourceLoader.wolfBody_Albino, modResourceLoader.wolfBody_Albino, modResourceLoader.wolfBody_Albino, "Wolf", "Albino");
			BuildPack(modResourceLoader.wolfBody_Black, modResourceLoader.wolfBody_Black, modResourceLoader.wolfBody_Black, "Wolf", "Black");
			BuildPack(modResourceLoader.wolfBody_DarkBrown, modResourceLoader.wolfBody_DarkBrown, modResourceLoader.wolfBody_DarkBrown, "Wolf", "Brown");
			BuildPack(modResourceLoader.wolfBody_Grey, modResourceLoader.wolfBody_Grey, modResourceLoader.wolfBody_Grey, "Wolf", "Grey");
			BuildPack(modResourceLoader.neckBody_Default, modResourceLoader.neckBody_Default, modResourceLoader.neckBody_Default, "Neck", "Default");
			BuildPack(modResourceLoader.neckBody_Albino, modResourceLoader.neckBody_Albino, modResourceLoader.neckBody_Albino, "Neck", "Albino");
			BuildPack(modResourceLoader.neckBody_DarkBlue, modResourceLoader.neckBody_DarkBlue, modResourceLoader.neckBody_DarkBlue, "Neck", "Dark");
			BuildPack(modResourceLoader.neckBody_DarkGreen, modResourceLoader.neckBody_DarkGreen, modResourceLoader.neckBody_DarkGreen, "Neck", "Green");
			BuildPack(modResourceLoader.neckBody_Blue, modResourceLoader.neckBody_Blue, modResourceLoader.neckBody_Blue, "Neck", "Blue");
			BuildPack(modResourceLoader.hareBody_Default, modResourceLoader.hareBody_Default, modResourceLoader.hareBody_Default, "Hare", "Default");
			BuildPack(modResourceLoader.hareBody_Albino, modResourceLoader.hareBody_Albino, modResourceLoader.hareBody_Albino, "Hare", "Albino");
			BuildPack(modResourceLoader.hareBody_Black, modResourceLoader.hareBody_Black, modResourceLoader.hareBody_Black, "Hare", "Black");
			BuildPack(modResourceLoader.hareBody_DarkBlue, modResourceLoader.hareBody_DarkBlue, modResourceLoader.hareBody_DarkBlue, "Hare", "Blue");
			BuildPack(modResourceLoader.hareBody_Ginger, modResourceLoader.hareBody_Ginger, modResourceLoader.hareBody_Ginger, "Hare", "Ginger");
			BuildPack(modResourceLoader.drakeBody_Default, modResourceLoader.drakeBody_Default, modResourceLoader.drakeBody_Default, "Hatchling", "Default");
			BuildPack(modResourceLoader.drakeBody_Red, modResourceLoader.drakeBody_Red, modResourceLoader.drakeBody_Red, "Hatchling", "Albino");
			BuildPack(modResourceLoader.drakeBody_DarkBlue, modResourceLoader.drakeBody_DarkBlue, modResourceLoader.drakeBody_DarkBlue, "Hatchling", "Blue");
			BuildPack(modResourceLoader.drakeBody_IceBlue, modResourceLoader.drakeBody_IceBlue, modResourceLoader.drakeBody_IceBlue, "Hatchling", "Ice");
			BuildPack(modResourceLoader.drakeBody_Purple, modResourceLoader.drakeBody_Purple, modResourceLoader.drakeBody_Purple, "Hatchling", "Purple");
			BuildPack(modResourceLoader.serpentBody_Default, modResourceLoader.serpentBody_Default, modResourceLoader.serpentBody_Default, "Serpent", "Default");
			BuildPack(modResourceLoader.serpentBody_Albino, modResourceLoader.serpentBody_Albino, modResourceLoader.serpentBody_Albino, "Serpent", "Albino");
			BuildPack(modResourceLoader.serpentBody_Black, modResourceLoader.serpentBody_Black, modResourceLoader.serpentBody_Black, "Serpent", "Black");
			BuildPack(modResourceLoader.serpentBody_Blue, modResourceLoader.serpentBody_Blue, modResourceLoader.serpentBody_Blue, "Serpent", "Blue");
			BuildPack(modResourceLoader.serpentBody_Green, modResourceLoader.serpentBody_Green, modResourceLoader.serpentBody_Green, "Serpent", "Green");
			BuildPack(modResourceLoader.serpentBody_Red, modResourceLoader.serpentBody_Red, modResourceLoader.serpentBody_Red, "Serpent", "Red");
			BuildPack(modResourceLoader.askvinBody_Default, modResourceLoader.askvinBody_Default, modResourceLoader.askvinBody_Default, "Asksvin", "Default");
			BuildPack(modResourceLoader.askvinBody_Albino, modResourceLoader.askvinBody_Albino, modResourceLoader.askvinBody_Albino, "Asksvin", "Albino");
			BuildPack(modResourceLoader.askvinBody_Black, modResourceLoader.askvinBody_Black, modResourceLoader.askvinBody_Black, "Asksvin", "Dark");
			BuildPack(modResourceLoader.askvinBody_Purple, modResourceLoader.askvinBody_Purple, modResourceLoader.askvinBody_Purple, "Asksvin", "Purple");
			BuildPack(modResourceLoader.askvinBody_Grey, modResourceLoader.askvinBody_Grey, modResourceLoader.askvinBody_Grey, "Asksvin", "Grey");
			BuildPack(modResourceLoader.deerBody_Default, modResourceLoader.deerBody_Default, modResourceLoader.deerBody_Default, "Deer", "Default");
			BuildPack(modResourceLoader.deerBody_Albino, modResourceLoader.deerBody_Albino, modResourceLoader.deerBody_Albino, "Deer", "Albino");
			BuildPack(modResourceLoader.deerBody_Red, modResourceLoader.deerBody_Red, modResourceLoader.deerBody_Red, "Deer", "Red");
			BuildPack(modResourceLoader.deerBody_DarkBrown, modResourceLoader.deerBody_DarkBrown, modResourceLoader.deerBody_DarkBrown, "Deer", "Brown");
			BuildPack(modResourceLoader.deerBody_Grey, modResourceLoader.deerBody_Grey, modResourceLoader.deerBody_Grey, "Deer", "Grey");
			BuildPack(modResourceLoader.boarBody_Default, modResourceLoader.boarBody_Default, modResourceLoader.boarBody_Default, "Boar", "Default");
			BuildPack(modResourceLoader.boarBody_Grey, modResourceLoader.boarBody_Grey, modResourceLoader.boarBody_Grey, "Boar", "Grey");
			BuildPack(modResourceLoader.boarBody_Black, modResourceLoader.boarBody_Black, modResourceLoader.boarBody_Black, "Boar", "Black");
			BuildPack(modResourceLoader.boarBody_DarkBrown, modResourceLoader.boarBody_DarkBrown, modResourceLoader.boarBody_DarkBrown, "Boar", "Brown");
			BuildPack(modResourceLoader.boarBody_LightBrown, modResourceLoader.boarBody_LightBrown, modResourceLoader.boarBody_LightBrown, "Boar", "Brown2");
		}
	}

	private void BuildPack(Material face, Material body, Material body2, string name, string skin)
	{
		if ((Object)(object)face == (Object)null || (Object)(object)body == (Object)null || (Object)(object)body2 == (Object)null)
		{
			Debug.LogWarning((object)("Skipping null material pack: " + name + " / " + skin));
		}
		else
		{
			materialPacks.Add(new MaterialPack(face, body, body2, name, skin));
		}
	}

	private bool SetupFurRandomizerForMonster()
	{
		string name = EditZnetName(((Object)((Component)this).gameObject).name);
		m_materials = materialPacks.FindAll((MaterialPack x) => x.animal == name);
		switch (name)
		{
		case "Lox":
		{
			Transform[] componentsInChildren4 = ((Component)this).gameObject.GetComponentsInChildren<Transform>(true);
			foreach (Transform val4 in componentsInChildren4)
			{
				if (((Object)((Component)val4).gameObject).name == "Body")
				{
					SkinnedMeshRenderer component4 = ((Component)val4).GetComponent<SkinnedMeshRenderer>();
					if ((Object)(object)component4 != (Object)null)
					{
						face = component4;
						body = component4;
					}
				}
				else if (((Object)((Component)val4).gameObject).name == "Furr1")
				{
					SkinnedMeshRenderer component5 = ((Component)val4).GetComponent<SkinnedMeshRenderer>();
					if ((Object)(object)component5 != (Object)null)
					{
						fur = component5;
					}
				}
			}
			break;
		}
		case "Hatchling":
		{
			Transform[] componentsInChildren2 = ((Component)this).gameObject.GetComponentsInChildren<Transform>(true);
			foreach (Transform val2 in componentsInChildren2)
			{
				if (((Object)((Component)val2).gameObject).name == "Hatchling")
				{
					SkinnedMeshRenderer component = ((Component)val2).GetComponent<SkinnedMeshRenderer>();
					if ((Object)(object)component != (Object)null)
					{
						body = component;
						face = ((Component)this).gameObject.GetComponentInChildren<SkinnedMeshRenderer>(true);
					}
				}
				else if (((Object)((Component)val2).gameObject).name == "Hatchling.001")
				{
					SkinnedMeshRenderer component2 = ((Component)val2).GetComponent<SkinnedMeshRenderer>();
					if ((Object)(object)component2 != (Object)null)
					{
						fur = component2;
					}
				}
			}
			break;
		}
		case "Neck":
		{
			Transform[] componentsInChildren3 = ((Component)this).gameObject.GetComponentsInChildren<Transform>(true);
			foreach (Transform val3 in componentsInChildren3)
			{
				if (((Object)((Component)val3).gameObject).name == "Body")
				{
					SkinnedMeshRenderer component3 = ((Component)val3).GetComponent<SkinnedMeshRenderer>();
					if ((Object)(object)component3 != (Object)null)
					{
						face = component3;
						body = component3;
						fur = component3;
					}
				}
			}
			break;
		}
		case "Boar":
		{
			SkinnedMeshRenderer[] componentsInChildren = ((Component)this).gameObject.GetComponentsInChildren<SkinnedMeshRenderer>(true);
			foreach (SkinnedMeshRenderer val in componentsInChildren)
			{
				if (((Object)val).name == "Poly Art Boar")
				{
					face = val;
					body = val;
					fur = val;
					break;
				}
			}
			break;
		}
		case "Asksvin":
		case "Deer":
		case "Serpent":
		case "Wolf":
		case "Hare":
			fur = (body = (face = ((Component)this).gameObject.GetComponentInChildren<SkinnedMeshRenderer>(true)));
			break;
		}
		if ((Object)(object)face == (Object)null || (Object)(object)body == (Object)null || (Object)(object)fur == (Object)null)
		{
			Debug.LogWarning((object)("Couldn't find renderers for " + ((Object)((Component)this).gameObject).name));
			return false;
		}
		return true;
	}
}
