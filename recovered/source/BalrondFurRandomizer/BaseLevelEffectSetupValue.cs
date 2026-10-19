using System.Linq;
using UnityEngine;

namespace BalrondFurRandomizer;

public class BaseLevelEffectSetupValue
{
	public float scale = 1.1f;

	public float saturation = 0.5f;

	public float hue = -0.1f;

	public float value = -0.1f;

	public LevelSetup setup = null;

	public GameObject enabledObject = null;

	public GameObject enabledObjectLevel2 = null;

	public GameObject enabledObjectLevel3 = null;

	public bool setEmmisive = false;

	public Color emissive = Color.white;

	public float sumValue = 0.05f;

	public void getSetup(LevelEffects levelEffects)
	{
		//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
		if ((Object)(object)levelEffects == (Object)null)
		{
			Debug.LogWarning((object)"I should not be here i am null");
			return;
		}
		int num = 0;
		if (levelEffects.m_levelSetups != null && levelEffects.m_levelSetups.Count > 0)
		{
			num = levelEffects.m_levelSetups.Count;
			setup = levelEffects.m_levelSetups.Last();
		}
		if (num != 0 && setup != null)
		{
			scale = setup.m_scale;
			saturation = setup.m_saturation;
			hue = setup.m_hue;
			value = setup.m_value;
			enabledObject = setup.m_enableObject;
			emissive = setup.m_emissiveColor;
			setEmmisive = setup.m_setEmissiveColor;
			enabledObjectLevel2 = enabledObject;
			enabledObjectLevel3 = enabledObject;
		}
		else
		{
			setValues(num, levelEffects);
		}
		if ((Object)(object)levelEffects.m_character != (Object)null)
		{
			if (((Object)((Component)levelEffects.m_character).gameObject).name == "Deer")
			{
				EditDeer(levelEffects);
			}
			if (((Object)((Component)levelEffects.m_character).gameObject).name == "Boar")
			{
				EditBoar(levelEffects);
			}
		}
	}

	private void EditDeer(LevelEffects levelEffects)
	{
		GameObject enableObject = levelEffects.m_levelSetups[0].m_enableObject;
		GameObject enableObject2 = levelEffects.m_levelSetups[1].m_enableObject;
		enabledObjectLevel2 = enableObject;
		enabledObjectLevel3 = enableObject2;
		Transform parent = enableObject.transform.parent;
		GameObject enableObject3 = (enabledObject = ((Component)parent.Find("Antler1")).gameObject);
		levelEffects.m_levelSetups[0].m_enableObject = enableObject3;
		levelEffects.m_levelSetups[1].m_enableObject = enableObject3;
	}

	private void EditBoar(LevelEffects gameObject)
	{
	}

	private void setValues(int levels, LevelEffects levelEffects)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		float y = ((Component)levelEffects).transform.localScale.y;
		if (levels == 0)
		{
			scale = y;
			saturation = 0f;
			hue = 0f;
			value = 0f;
		}
		if (levels == 1)
		{
			scale = y + 0.05f;
			saturation = 0.5f;
			hue = -0.05f;
			value = -0.05f;
		}
		if (levels == 2)
		{
			scale = y + 0.1f;
			saturation = 0.5f;
			hue = -0.1f;
			value = -0.1f;
		}
	}

	public BaseLevelEffectSetupValue()
	{
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
	}
}
