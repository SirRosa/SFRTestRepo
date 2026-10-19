using System.Collections.Generic;
using UnityEngine;

namespace BalrondFurRandomizer;

public class FxReplacment
{
	private List<GameObject> allPrefabs;

	private string projectName = "[BalrondFurRandomizer]";

	public void setInstance(List<GameObject> gameObjects)
	{
		allPrefabs = gameObjects;
	}

	public void ReplaceOnObject(GameObject gameObject)
	{
		if ((Object)(object)gameObject == (Object)null)
		{
			return;
		}
		SpawnArea component = gameObject.GetComponent<SpawnArea>();
		if ((Object)(object)component != (Object)null)
		{
			EffectList spawnEffects = component.m_spawnEffects;
			if (spawnEffects != null)
			{
				findEffectsAndChange(spawnEffects.m_effectPrefabs);
			}
		}
		Destructible component2 = gameObject.GetComponent<Destructible>();
		if ((Object)(object)component2 != (Object)null)
		{
			EffectList hitEffect = component2.m_hitEffect;
			if (hitEffect != null)
			{
				findEffectsAndChange(hitEffect.m_effectPrefabs);
			}
			EffectList destroyedEffect = component2.m_destroyedEffect;
			if (destroyedEffect != null)
			{
				findEffectsAndChange(destroyedEffect.m_effectPrefabs);
			}
		}
		Projectile component3 = gameObject.GetComponent<Projectile>();
		if ((Object)(object)component3 != (Object)null)
		{
			EffectList hitEffects = component3.m_hitEffects;
			if (hitEffects != null)
			{
				findEffectsAndChange(hitEffects.m_effectPrefabs);
			}
			EffectList hitWaterEffects = component3.m_hitWaterEffects;
			if (hitWaterEffects != null)
			{
				findEffectsAndChange(hitWaterEffects.m_effectPrefabs);
			}
			EffectList spawnOnHitEffects = component3.m_spawnOnHitEffects;
			if (spawnOnHitEffects != null)
			{
				findEffectsAndChange(spawnOnHitEffects.m_effectPrefabs);
			}
		}
	}

	public void ReplaceOnVegetation(GameObject gameObject)
	{
		Pickable component = gameObject.GetComponent<Pickable>();
		if ((Object)(object)component != (Object)null)
		{
			fixPlant(component);
		}
		Destructible component2 = gameObject.GetComponent<Destructible>();
		if ((Object)(object)component2 != (Object)null)
		{
			fixPDestructable(component2);
		}
		MineRock5 component3 = gameObject.GetComponent<MineRock5>();
		if ((Object)(object)component3 != (Object)null)
		{
			fixMineRock5(component3);
		}
		MineRock component4 = gameObject.GetComponent<MineRock>();
		if ((Object)(object)component4 != (Object)null)
		{
			fixMineRock(component4);
		}
	}

	private void fixPlant(Pickable pickable)
	{
		EffectList pickEffector = pickable.m_pickEffector;
		if (pickEffector != null)
		{
			findEffectsAndChange(pickEffector.m_effectPrefabs);
		}
	}

	private void fixPDestructable(Destructible minerock5)
	{
		EffectList hitEffect = minerock5.m_hitEffect;
		if (hitEffect != null)
		{
			findEffectsAndChange(hitEffect.m_effectPrefabs);
		}
		EffectList destroyedEffect = minerock5.m_destroyedEffect;
		if (destroyedEffect != null)
		{
			findEffectsAndChange(destroyedEffect.m_effectPrefabs);
		}
	}

	private void fixMineRock5(MineRock5 minerock5)
	{
		EffectList hitEffect = minerock5.m_hitEffect;
		if (hitEffect != null)
		{
			findEffectsAndChange(hitEffect.m_effectPrefabs);
		}
		EffectList destroyedEffect = minerock5.m_destroyedEffect;
		if (destroyedEffect != null)
		{
			findEffectsAndChange(destroyedEffect.m_effectPrefabs);
		}
	}

	private void fixMineRock(MineRock minerock5)
	{
		EffectList hitEffect = minerock5.m_hitEffect;
		if (hitEffect != null)
		{
			findEffectsAndChange(hitEffect.m_effectPrefabs);
		}
		EffectList destroyedEffect = minerock5.m_destroyedEffect;
		if (destroyedEffect != null)
		{
			findEffectsAndChange(destroyedEffect.m_effectPrefabs);
		}
	}

	public void ReplaceOnMonster(GameObject gameObject)
	{
		if ((Object)(object)gameObject == (Object)null)
		{
			Debug.LogWarning((object)(projectName + ":: GameObject not found"));
			return;
		}
		Humanoid component = gameObject.GetComponent<Humanoid>();
		if ((Object)(object)component == (Object)null)
		{
			Debug.LogWarning((object)(projectName + ":: GameObject not found"));
			return;
		}
		EffectList dropEffects = component.m_dropEffects;
		if (dropEffects != null)
		{
			findEffectsAndChange(dropEffects.m_effectPrefabs);
		}
		EffectList backstabHitEffects = ((Character)component).m_backstabHitEffects;
		if (backstabHitEffects != null)
		{
			findEffectsAndChange(backstabHitEffects.m_effectPrefabs);
		}
		EffectList consumeItemEffects = component.m_consumeItemEffects;
		if (consumeItemEffects != null)
		{
			findEffectsAndChange(consumeItemEffects.m_effectPrefabs);
		}
		EffectList critHitEffects = ((Character)component).m_critHitEffects;
		if (critHitEffects != null)
		{
			findEffectsAndChange(critHitEffects.m_effectPrefabs);
		}
		EffectList deathEffects = ((Character)component).m_deathEffects;
		if (deathEffects != null)
		{
			findEffectsAndChange(deathEffects.m_effectPrefabs);
		}
		EffectList hitEffects = ((Character)component).m_hitEffects;
		if (hitEffects != null)
		{
			findEffectsAndChange(hitEffects.m_effectPrefabs);
		}
		EffectList jumpEffects = ((Character)component).m_jumpEffects;
		if (jumpEffects != null)
		{
			findEffectsAndChange(jumpEffects.m_effectPrefabs);
		}
		EffectList perfectBlockEffect = component.m_perfectBlockEffect;
		if (perfectBlockEffect != null)
		{
			findEffectsAndChange(perfectBlockEffect.m_effectPrefabs);
		}
		EffectList pickupEffects = component.m_pickupEffects;
		if (pickupEffects != null)
		{
			findEffectsAndChange(pickupEffects.m_effectPrefabs);
		}
		EffectList slideEffects = ((Character)component).m_slideEffects;
		if (slideEffects != null)
		{
			findEffectsAndChange(slideEffects.m_effectPrefabs);
		}
		EffectList tarEffects = ((Character)component).m_tarEffects;
		if (tarEffects != null)
		{
			findEffectsAndChange(tarEffects.m_effectPrefabs);
		}
		EffectList waterEffects = ((Character)component).m_waterEffects;
		if (waterEffects != null)
		{
			findEffectsAndChange(waterEffects.m_effectPrefabs);
		}
		FootStep component2 = gameObject.GetComponent<FootStep>();
		if (!((Object)(object)component2 != (Object)null))
		{
			return;
		}
		List<StepEffect> effects = component2.m_effects;
		foreach (StepEffect item in effects)
		{
			GameObject[] effectPrefabs = item.m_effectPrefabs;
			List<GameObject> list = new List<GameObject>();
			list.AddRange(effectPrefabs);
			for (int i = 0; i < list.Count; i++)
			{
				if ((Object)(object)list[i] != (Object)null)
				{
					string name = ((Object)list[i]).name;
					GameObject val = allPrefabs.Find((GameObject x) => ((Object)x).name == name);
					if (!((Object)(object)val == (Object)null))
					{
						list[i] = val;
					}
				}
			}
		}
	}

	public void ReplaceOnItem(GameObject gameObject)
	{
		if ((Object)(object)gameObject == (Object)null)
		{
			return;
		}
		ItemDrop component = gameObject.GetComponent<ItemDrop>();
		if (!((Object)(object)component == (Object)null))
		{
			EffectList hitEffect = component.m_itemData.m_shared.m_hitEffect;
			if (hitEffect != null)
			{
				findEffectsAndChange(hitEffect.m_effectPrefabs);
			}
			EffectList hitTerrainEffect = component.m_itemData.m_shared.m_hitTerrainEffect;
			if (hitTerrainEffect != null)
			{
				findEffectsAndChange(hitTerrainEffect.m_effectPrefabs);
			}
			EffectList holdStartEffect = component.m_itemData.m_shared.m_holdStartEffect;
			if (holdStartEffect != null)
			{
				findEffectsAndChange(holdStartEffect.m_effectPrefabs);
			}
			EffectList trailStartEffect = component.m_itemData.m_shared.m_trailStartEffect;
			if (trailStartEffect != null)
			{
				findEffectsAndChange(trailStartEffect.m_effectPrefabs);
			}
			EffectList blockEffect = component.m_itemData.m_shared.m_blockEffect;
			if (blockEffect != null)
			{
				findEffectsAndChange(blockEffect.m_effectPrefabs);
			}
		}
	}

	public void ReplaceFxOnPiece(GameObject gameObject)
	{
		if ((Object)(object)gameObject == (Object)null)
		{
			return;
		}
		Piece component = gameObject.GetComponent<Piece>();
		if ((Object)(object)component != (Object)null)
		{
			EffectList placeEffect = component.m_placeEffect;
			if (placeEffect != null)
			{
				findEffectsAndChange(placeEffect.m_effectPrefabs);
			}
		}
		WearNTear component2 = gameObject.GetComponent<WearNTear>();
		if ((Object)(object)component2 != (Object)null)
		{
			EffectList hitEffect = component2.m_hitEffect;
			if (hitEffect != null)
			{
				findEffectsAndChange(hitEffect.m_effectPrefabs);
			}
		}
	}

	private void findEffectsAndChange(EffectData[] effects)
	{
		if (effects == null || effects.Length == 0)
		{
			return;
		}
		foreach (EffectData val in effects)
		{
			if ((Object)(object)val.m_prefab != (Object)null)
			{
				string name = ((Object)val.m_prefab).name;
				GameObject val2 = allPrefabs.Find((GameObject x) => ((Object)x).name == name);
				if (!((Object)(object)val2 == (Object)null))
				{
					val.m_prefab = val2;
				}
			}
		}
	}
}
