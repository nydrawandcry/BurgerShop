
using System.Collections.Generic;
using UnityEngine;

public class BurgerAssemblyPlace : MonoBehaviour
{
    [Header("Stack")]
    [SerializeField] private Transform stackRoot;

    [Tooltip("Маленький зазор между слоями. 0 = вплотную. Можно поставить -0.002, если остаются щели.")]
    [SerializeField] private float layerGap = 0f;
    
    [Header("Burger State")]
    [SerializeField] private string topBunIngredientName = "top loaf";
    
    private readonly List<IngredientItem> placedItems = new List<IngredientItem>();
    private readonly List<string> placedIngredientNames = new List<string>();
    
    private float currentTopWorldY;

    public bool IsBurgerClosed
    {
        get; private set;
    }
    
    private void Awake()
    {
        if (stackRoot == null)
        {
            Debug.LogError("BurgerAssemblyPlace: не назначен StackRoot. Создай пустой объект StackRoot и перетащи его в поле.");
            enabled = false;
            return;
        }
        
        currentTopWorldY = stackRoot.position.y;
        IsBurgerClosed = false;
    }

    public bool PlaceIngredient(IngredientItem ingredient)
    {
        if (ingredient == null)
        {
            return false;
        }
        
        if (IsBurgerClosed)
        {
            Debug.LogWarning("Бургер уже закрыт верхней булкой. Больше ингредиенты класть нельзя.");
            return false;
        }

        ingredient.PlaceOnBurger(stackRoot, Vector3.zero);

        if (ingredient.TryGetRendererBounds(out Bounds bounds))
        {
            float offsetToPutBottomOnStack = currentTopWorldY - bounds.min.y;

            ingredient.transform.position += Vector3.up * offsetToPutBottomOnStack;

            if (ingredient.TryGetRendererBounds(out Bounds updatedBounds))
            {
                currentTopWorldY = updatedBounds.max.y + layerGap;
            }
        }
        else
        {
            Debug.LogWarning("У ингредиента нет Renderer: " + ingredient.gameObject.name);
            currentTopWorldY += 0.05f;
        }

        placedItems.Add(ingredient);
        placedIngredientNames.Add(ingredient._ingredientName);
        
        string placedName = ingredient._ingredientName.Trim().ToLower();
        string topBunName = topBunIngredientName.Trim().ToLower();

        Debug.Log("Проверка закрытия бургера. Положили: [" + placedName + "], верхняя булка должна быть: [" + topBunName + "]");

        if (placedName == topBunName)
        {
            IsBurgerClosed = true;
            Debug.Log("Бургер закрыт верхней булкой. Больше ингредиенты брать нельзя.");
        }

        Debug.Log("Положили ингредиент: " + ingredient._ingredientName);
        Debug.Log("Текущий бургер: " + string.Join(" -> ", placedIngredientNames));
        
        return true;
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

        currentTopWorldY = stackRoot.position.y;
        IsBurgerClosed = false;
    }
}