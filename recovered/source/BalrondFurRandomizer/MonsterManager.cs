using System.Collections.Generic;
using UnityEngine;

namespace BalrondFurRandomizer;

public class MonsterManager
{
	private readonly LevelEffectGenerator levelEffectGenerator = new LevelEffectGenerator();

	private List<GameObject> monsters = new List<GameObject>();

	private static readonly HashSet<string> VisualNames = new HashSet<string>
	{
		"Lox", "Wraith", "TentaRoot", "TheHive", "Hatchling", "GoblinKing", "Eikthyr", "Bonemass", "Frog", "Haugbui",
		"Greydwarf_Mystic", "Varsvin", "Varsvin_Brute", "Varsvin_Hunter", "SeekerQueen"
	};

	private static readonly HashSet<string> SkipNames = new HashSet<string> { "TheHive", "Haugbui", "Greydwarf_Mystic" };

	public void SetupMonsterList(List<GameObject> list)
	{
		if (list == null)
		{
			Debug.LogWarning((object)"I DID NOT FIND PREFAB LIST!");
			monsters = new List<GameObject>();
		}
		else
		{
			monsters = list.FindAll((GameObject x) => (Object)(object)x != (Object)null && (Object)(object)x.GetComponent<MonsterAI>() != (Object)null);
		}
	}

	public void ChangeMonsterResistance()
	{
		if (monsters == null || monsters.Count == 0)
		{
			Debug.LogWarning((object)"I DID NOT FIND ANY MONSTERS!");
			return;
		}
		foreach (GameObject monster in monsters)
		{
			if (!((Object)(object)monster == (Object)null))
			{
				SetupLevelVisual(monster);
			}
		}
	}

	private void SetupLevelVisual(GameObject gameObject)
	{
		if ((Object)(object)gameObject == (Object)null || SkipNames.Contains(((Object)gameObject).name))
		{
			return;
		}
		Transform val = gameObject.transform.Find("Visual");
		if ((Object)(object)val == (Object)null)
		{
			return;
		}
		LevelEffects val2 = gameObject.GetComponentInChildren<LevelEffects>();
		if (VisualNames.Contains(((Object)gameObject).name))
		{
			Transform val3 = FindSpecialBodyTransform(gameObject);
			if ((Object)(object)val3 == (Object)null)
			{
				return;
			}
			SkinnedMeshRenderer component = ((Component)val3).GetComponent<SkinnedMeshRenderer>();
			if ((Object)(object)component == (Object)null)
			{
				return;
			}
			val2 = levelEffectGenerator.CreateLevelEffectAt(val, component);
		}
		else if ((Object)(object)val2 == (Object)null)
		{
			val2 = levelEffectGenerator.CreateLevelEffectComponent(val);
		}
		if ((Object)(object)val2 != (Object)null)
		{
			levelEffectGenerator.CreateVisuals(val2);
		}
	}

	private Transform FindSpecialBodyTransform(GameObject gameObject)
	{
		switch (((Object)gameObject).name)
		{
		case "Lox":
			return gameObject.transform.Find("Visual/offset/Body");
		case "Wraith":
			return gameObject.transform.Find("Visual/wraith/wraith");
		case "TentaRoot":
			return gameObject.transform.Find("Visual/Tentaroots/Cube.001");
		case "Hatchling":
			return gameObject.transform.Find("Visual/Hatchling_mountain/Hatchling");
		case "GoblinKing":
			return gameObject.transform.Find("Visual/Yagluth");
		case "Eikthyr":
			return gameObject.transform.Find("Visual/Deer");
		case "Bonemass":
			return gameObject.transform.Find("Visual/model/bonemass");
		case "Varsvin":
		case "Varsvin_Brute":
		case "Varsvin_Hunter":
			return gameObject.transform.Find("Visual/Body");
		case "Frog":
			return gameObject.transform.Find("Visual/Frog2_mesh");
		case "SeekerQueen":
			return gameObject.transform.Find("Visual/Cube");
		default:
			return null;
		}
	}
}
