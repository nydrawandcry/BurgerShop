
using UnityEngine;

public class IngredientDispenser : MonoBehaviour
{
	[Header("Prefab")]
	[SerializeField] private IngredientItem ingredientPrefab;

	[Header("Data")]
	[SerializeField] private string ingredientName;

	public IngredientItem SpawnIngredient()
	{
		if (ingredientPrefab == null)
		{
			Debug.LogError("IngredientDispenser: не назначен ingredientPrefab на объекте " + gameObject.name);
			return null;
		}

		IngredientItem newIngredient = Instantiate(ingredientPrefab);
		
		if (!string.IsNullOrEmpty(ingredientName))
		{
			newIngredient._ingredientName = ingredientName;
		}
		
		Debug.Log("Создан ингредиент: " + newIngredient.name);

		return newIngredient;
	}
}