
using UnityEngine;

public class IngredientDispenser : MonoBehaviour
{
	[Header("Prefab")]
	[SerializeField] private IngredientItem ingredientPrefab;

	[Header("Data")]
	[SerializeField] private string ingredientName;

	public IngredientItem SpawnIngredient(Transform holdPoint)
	{
		if (ingredientPrefab == null)
		{
			Debug.LogError("IngredientDispenser: не назначен ingredientPrefab на объекте " + gameObject.name);
			return null;
		}

		IngredientItem newIngredient = Instantiate(
			ingredientPrefab,
			holdPoint.position,
			holdPoint.rotation
		);

		if (!string.IsNullOrEmpty(ingredientName))
		{
			newIngredient._ingredientName = ingredientName;
		}

		newIngredient.PickUp(holdPoint);

		return newIngredient;
	}
}