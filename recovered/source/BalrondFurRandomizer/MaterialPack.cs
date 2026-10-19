using System;
using UnityEngine;

namespace BalrondFurRandomizer;

[Serializable]
public class MaterialPack
{
	public Material face;

	public Material body;

	public Material body2;

	public string animal;

	public string skin;

	public MaterialPack(Material body, Material body2, Material face, string monsterName, string skinName)
	{
		animal = monsterName;
		this.face = face;
		this.body = body;
		this.body2 = body2;
		skin = skinName;
	}
}
