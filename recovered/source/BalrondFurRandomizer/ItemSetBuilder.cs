using System.Collections.Generic;
using UnityEngine;

namespace BalrondFurRandomizer;

public class ItemSetBuilder
{
	public List<ItemSet> itemSets = new List<ItemSet>();

	public List<GameObject> prefabs = new List<GameObject>();

	public ModResourceLoader modResourceLoader;

	public static readonly List<MaterialPack> materialPacks = new List<MaterialPack>();

	public void Setup(List<GameObject> prefabs)
	{
		this.prefabs = prefabs ?? new List<GameObject>();
	}

	public void CreateMaterialPacks(ModResourceLoader loader)
	{
		if (loader == null)
		{
			Debug.LogWarning((object)"ModResourceLoader is null");
			return;
		}
		modResourceLoader = loader;
		if (materialPacks.Count <= 0)
		{
			BuildPack(loader.loxFace_Default, loader.loxtBody_Default, loader.loxFur_Default, "Lox", "Default");
			BuildPack(loader.loxFace_Albino, loader.loxtBody_Albino, loader.loxFur_Albino, "Lox", "Albino");
			BuildPack(loader.loxFace_Black, loader.loxtBody_Black, loader.loxFur_Black, "Lox", "Black");
			BuildPack(loader.loxFace_Blond, loader.loxtBody_Blond, loader.loxFur_Blond, "Lox", "Blond");
			BuildPack(loader.loxFace_DarkBrown, loader.loxtBody_DarkBrown, loader.loxFur_DarkBrown, "Lox", "Brown");
			BuildPack(loader.loxFace_Ginger, loader.loxtBody_Ginger, loader.loxFur_Ginger, "Lox", "Ginger");
			BuildPack(loader.wolfBody_Default, loader.wolfBody_Default, loader.wolfBody_Default, "Wolf", "Default");
			BuildPack(loader.wolfBody_Albino, loader.wolfBody_Albino, loader.wolfBody_Albino, "Wolf", "Albino");
			BuildPack(loader.wolfBody_Black, loader.wolfBody_Black, loader.wolfBody_Black, "Wolf", "Black");
			BuildPack(loader.wolfBody_DarkBrown, loader.wolfBody_DarkBrown, loader.wolfBody_DarkBrown, "Wolf", "Brown");
			BuildPack(loader.wolfBody_Grey, loader.wolfBody_Grey, loader.wolfBody_Grey, "Wolf", "Grey");
			BuildPack(loader.deerBody_Default, loader.deerBody_Default, loader.deerBody_Default, "Deer", "Default");
			BuildPack(loader.deerBody_Albino, loader.deerBody_Albino, loader.deerBody_Albino, "Deer", "Albino");
			BuildPack(loader.deerBody_Red, loader.deerBody_Red, loader.deerBody_Red, "Deer", "Red");
			BuildPack(loader.deerBody_DarkBrown, loader.deerBody_DarkBrown, loader.deerBody_DarkBrown, "Deer", "Brown");
			BuildPack(loader.deerBody_Grey, loader.deerBody_Grey, loader.deerBody_Grey, "Deer", "Grey");
			BuildPack(loader.boarBody_Default, loader.boarBody_Default, loader.boarBody_Default, "Boar", "Default");
			BuildPack(loader.boarBody_Grey, loader.boarBody_Grey, loader.boarBody_Grey, "Boar", "Grey");
			BuildPack(loader.boarBody_Black, loader.boarBody_Black, loader.boarBody_Black, "Boar", "Black");
			BuildPack(loader.boarBody_DarkBrown, loader.boarBody_DarkBrown, loader.boarBody_DarkBrown, "Boar", "Brown");
			BuildPack(loader.boarBody_LightBrown, loader.boarBody_LightBrown, loader.boarBody_LightBrown, "Boar", "Brown2");
			BuildPack(loader.neckBody_Default, loader.neckBody_Default, loader.neckBody_Default, "Neck", "Default");
			BuildPack(loader.neckBody_Albino, loader.neckBody_Albino, loader.neckBody_Albino, "Neck", "Albino");
			BuildPack(loader.neckBody_DarkBlue, loader.neckBody_DarkBlue, loader.neckBody_DarkBlue, "Neck", "Dark");
			BuildPack(loader.neckBody_DarkGreen, loader.neckBody_DarkGreen, loader.neckBody_DarkGreen, "Neck", "Green");
			BuildPack(loader.neckBody_Blue, loader.neckBody_Blue, loader.neckBody_Blue, "Neck", "Blue");
			BuildPack(loader.askvinBody_Default, loader.askvinBody_Default, loader.askvinBody_Default, "Asksvin", "Default");
			BuildPack(loader.askvinBody_Albino, loader.askvinBody_Albino, loader.askvinBody_Albino, "Asksvin", "Albino");
			BuildPack(loader.askvinBody_Black, loader.askvinBody_Black, loader.askvinBody_Black, "Asksvin", "Dark");
			BuildPack(loader.askvinBody_Purple, loader.askvinBody_Purple, loader.askvinBody_Purple, "Asksvin", "Purple");
			BuildPack(loader.askvinBody_Grey, loader.askvinBody_Grey, loader.askvinBody_Grey, "Asksvin", "Grey");
			BuildPack(loader.hareBody_Default, loader.hareBody_Default, loader.hareBody_Default, "Hare", "Default");
			BuildPack(loader.hareBody_Albino, loader.hareBody_Albino, loader.hareBody_Albino, "Hare", "Albino");
			BuildPack(loader.hareBody_Black, loader.hareBody_Black, loader.hareBody_Black, "Hare", "Black");
			BuildPack(loader.hareBody_DarkBlue, loader.hareBody_DarkBlue, loader.hareBody_DarkBlue, "Hare", "Blue");
			BuildPack(loader.hareBody_Ginger, loader.hareBody_Ginger, loader.hareBody_Ginger, "Hare", "Ginger");
			BuildPack(loader.drakeBody_Default, loader.drakeBody_Default, loader.drakeBody_Default, "Hatchling", "Default");
			BuildPack(loader.drakeBody_Red, loader.drakeBody_Red, loader.drakeBody_Red, "Hatchling", "Albino");
			BuildPack(loader.drakeBody_DarkBlue, loader.drakeBody_DarkBlue, loader.drakeBody_DarkBlue, "Hatchling", "Blue");
			BuildPack(loader.drakeBody_IceBlue, loader.drakeBody_IceBlue, loader.drakeBody_IceBlue, "Hatchling", "Ice");
			BuildPack(loader.drakeBody_Purple, loader.drakeBody_Purple, loader.drakeBody_Purple, "Hatchling", "Purple");
			BuildPack(loader.serpentBody_Default, loader.serpentBody_Default, loader.serpentBody_Default, "Serpent", "Default");
			BuildPack(loader.serpentBody_Albino, loader.serpentBody_Albino, loader.serpentBody_Albino, "Serpent", "Albino");
			BuildPack(loader.serpentBody_Black, loader.serpentBody_Black, loader.serpentBody_Black, "Serpent", "Black");
			BuildPack(loader.serpentBody_Blue, loader.serpentBody_Blue, loader.serpentBody_Blue, "Serpent", "Blue");
			BuildPack(loader.serpentBody_Green, loader.serpentBody_Green, loader.serpentBody_Green, "Serpent", "Green");
			BuildPack(loader.serpentBody_Red, loader.serpentBody_Red, loader.serpentBody_Red, "Serpent", "Red");
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

	public void SetupFurRandomizerForMonster(GameObject monster)
	{
		if ((Object)(object)monster == (Object)null)
		{
			Debug.LogWarning((object)"Monster not found");
		}
		else if ((Object)(object)monster.GetComponent<FurRandomizer>() == (Object)null)
		{
			monster.AddComponent<FurRandomizer>();
		}
	}
}
