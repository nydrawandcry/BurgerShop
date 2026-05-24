
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

		IngredientItem newIngredient = Instantiate(ingredientPrefab, holdPoint);

		//newIngredient.transform.localPosition = Vector3.zero;
		//newIngredient.transform.localRotation = Quaternion.identity;
		//newIngredient.transform.localScale = Vector3.one;
		
		if (!string.IsNullOrEmpty(ingredientName))
		{
			newIngredient._ingredientName = ingredientName;
		}

		newIngredient.PickUp(holdPoint);
		
		Debug.Log(
			"Создан ингредиент: " + newIngredient.name +
			" | Position: " + newIngredient.transform.position +
			" | Parent: " + newIngredient.transform.parent.name
		);

		return newIngredient;
	}
}