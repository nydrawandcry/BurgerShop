
using System.Collections.Generic;
using UnityEngine;

public class BurgerAssemblyPlace : MonoBehaviour
{
    [Header("Stack")]
    [SerializeField] private Transform stackRoot;
    [SerializeField] private float layerHeight = 0.08f;

    private readonly List<IngredientItem> placedItems = new List<IngredientItem>();
    private readonly List<string> placedIngredientNames = new List<string>();
    
    private void Awake()
    {
        if (stackRoot == null)
        {
            stackRoot = transform;
        }
    }

    public void PlaceIngredient(IngredientItem ingredient)
    {
        if (ingredient == null)
        {
            return;
        }

        Vector3 localPosition = new Vector3(
            0f,
            placedItems.Count * layerHeight,
            0f
        );

        ingredient.PlaceOnBurger(
            stackRoot,
            localPosition,
            Quaternion.identity
        );

        placedItems.Add(ingredient);
        placedIngredientNames.Add(ingredient._ingredientName);

        Debug.Log("Положили ингредиент: " + ingredient._ingredientName);
        Debug.Log("Текущий бургер: " + string.Join(" -> ", placedIngredientNames));
    }

    public List<string> GetPlacedIngredientNames()
    {
        return new List<string>(placedIngredientNames);
    }

    public void ClearBurger()
    {
        foreach (IngredientItem item in placedItems)
        {
            if (item != null)
            {
                Destroy(item.gameObject);
            }
        }

        placedItems.Clear();
        placedIngredientNames.Clear();
    }
}