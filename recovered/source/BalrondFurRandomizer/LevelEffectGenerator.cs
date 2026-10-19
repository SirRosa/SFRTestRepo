using UnityEngine;

namespace BalrondFurRandomizer;

public class LevelEffectGenerator
{
	private BaseLevelEffectSetupValue baseLevelEffectSetupValue = new BaseLevelEffectSetupValue();

	public void CreateVisuals(LevelEffects levelEffects)
	{
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		baseLevelEffectSetupValue.getSetup(levelEffects);
		int num = 0;
		if (levelEffects.m_levelSetups != null)
		{
			num = levelEffects.m_levelSetups.Count;
		}
		int num2 = 6 - num;
		for (int i = 0; i < num2; i++)
		{
			LevelSetup item = createSetup(baseLevelEffectSetupValue.scale, baseLevelEffectSetupValue.saturation, baseLevelEffectSetupValue.hue, baseLevelEffectSetupValue.value, baseLevelEffectSetupValue.setEmmisive, baseLevelEffectSetupValue.emissive, baseLevelEffectSetupValue.sumValue * (float)(i + 1), num2, i + 1);
			levelEffects.m_levelSetups.Add(item);
		}
	}

	public LevelEffects CreateLevelEffectAt(Transform visual, SkinnedMeshRenderer meshRenderer)
	{
		if ((Object)(object)meshRenderer == (Object)null)
		{
			return null;
		}
		if ((Object)(object)meshRenderer != (Object)null)
		{
			LevelEffects val = ((Component)visual).gameObject.AddComponent<LevelEffects>();
			val.m_mainRender = (Renderer)(object)meshRenderer;
			val.m_character = (Character)(object)((Component)visual.parent).gameObject.GetComponent<Humanoid>();
			return val;
		}
		return null;
	}

	public LevelEffects CreateLevelEffectComponent(Transform visual)
	{
		SkinnedMeshRenderer[] componentsInChildren = ((Component)visual).GetComponentsInChildren<SkinnedMeshRenderer>();
		if (componentsInChildren.Length > 1)
		{
			Debug.LogWarning((object)("To many renderers to create Visual Effects:" + ((Object)visual.parent).name));
			return null;
		}
		if (componentsInChildren.Length == 1)
		{
			LevelEffects val = ((Component)visual).gameObject.AddComponent<LevelEffects>();
			val.m_mainRender = (Renderer)(object)componentsInChildren[0];
			val.m_character = (Character)(object)((Component)visual.parent).gameObject.GetComponent<Humanoid>();
			return val;
		}
		return null;
	}

	private LevelSetup createSetup(float scale, float saturation, float hue, float value, bool isEmmisive, Color emission, float changeValue, int maxAmount, int lvl)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Expected Obj, but got Unknown
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		LevelSetup val = new LevelSetup();
		val.m_scale = scale + changeValue;
		val.m_saturation = saturation + changeValue;
		val.m_hue = hue + changeValue;
		val.m_value = value + changeValue;
		val.m_enableObject = pickEnabledObject(maxAmount, lvl);
		val.m_setEmissiveColor = isEmmisive;
		val.m_emissiveColor = new Color(emission.r + changeValue, emission.g + changeValue, emission.b + changeValue, emission.a);
		return val;
	}

	private GameObject pickEnabledObject(int maxAmount, int lvl)
	{
		if (maxAmount == 6)
		{
			if (lvl > 4)
			{
				return baseLevelEffectSetupValue.enabledObjectLevel3;
			}
			if (lvl > 2)
			{
				return baseLevelEffectSetupValue.enabledObjectLevel2;
			}
		}
		if (maxAmount == 3)
		{
			if (lvl > 3)
			{
				return baseLevelEffectSetupValue.enabledObjectLevel3;
			}
			if (lvl > 2)
			{
				return baseLevelEffectSetupValue.enabledObjectLevel2;
			}
		}
		if (maxAmount < 3)
		{
			switch (lvl)
			{
			case 2:
				return baseLevelEffectSetupValue.enabledObjectLevel3;
			case 1:
				return baseLevelEffectSetupValue.enabledObjectLevel2;
			}
		}
		return baseLevelEffectSetupValue.enabledObject;
	}
}
