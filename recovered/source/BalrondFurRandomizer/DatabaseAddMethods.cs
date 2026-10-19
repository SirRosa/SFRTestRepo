using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

namespace BalrondFurRandomizer;

public class DatabaseAddMethods
{
	public void AddItems(List<GameObject> items)
	{
		foreach (GameObject item in items)
		{
			AddItem(item);
		}
	}

	public void AddRecipes(List<Recipe> recipes)
	{
		foreach (Recipe recipe in recipes)
		{
			AddRecipe(recipe);
		}
	}

	public void AddStatuseffects(List<StatusEffect> statusEffects)
	{
		foreach (StatusEffect statusEffect in statusEffects)
		{
			AddStatus(statusEffect);
		}
	}

	private bool IsObjectDBValid()
	{
		return (Object)(object)ObjectDB.instance != (Object)null && ObjectDB.instance.m_items.Count != 0 && ObjectDB.instance.m_recipes.Count != 0 && (Object)(object)ObjectDB.instance.GetItemPrefab("Amber") != (Object)null;
	}

	private void AddStatus(StatusEffect status)
	{
		if (!IsObjectDBValid())
		{
			return;
		}
		if ((Object)(object)status != (Object)null)
		{
			if ((Object)(object)ObjectDB.instance.GetStatusEffect(((object)status).GetHashCode()) == (Object)null)
			{
				ObjectDB.instance.m_StatusEffects.Add(status);
			}
		}
		else
		{
			Debug.LogError((object)("BalrondFurRandomizer:  " + ((Object)status).name + " - Status not found"));
		}
	}

	private void AddRecipe(Recipe recipe)
	{
		if (!IsObjectDBValid())
		{
			return;
		}
		if ((Object)(object)recipe != (Object)null)
		{
			if ((Object)(object)ObjectDB.instance.GetRecipe(recipe.m_item.m_itemData) == (Object)null)
			{
				ObjectDB.instance.m_recipes.Add(recipe);
			}
		}
		else
		{
			Debug.LogError((object)("BalrondFurRandomizer:  " + ((Object)recipe).name + " - Recipe not found"));
		}
	}

	private void AddItem(GameObject newPrefab)
	{
		if (!IsObjectDBValid())
		{
			return;
		}
		ItemDrop component = newPrefab.GetComponent<ItemDrop>();
		if ((Object)(object)component != (Object)null)
		{
			if ((Object)(object)ObjectDB.instance.GetItemPrefab(((Object)newPrefab).name) == (Object)null)
			{
				ObjectDB.instance.m_items.Add(newPrefab);
				Dictionary<int, GameObject> dictionary = (Dictionary<int, GameObject>)typeof(ObjectDB).GetField("m_itemByHash", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(ObjectDB.instance);
				dictionary[((Object)newPrefab).name.GetHashCode()] = newPrefab;
			}
		}
		else
		{
			Debug.LogError((object)("BalrondFurRandomizer:  " + ((Object)newPrefab).name + " - ItemDrop not found on prefab"));
		}
	}
}
