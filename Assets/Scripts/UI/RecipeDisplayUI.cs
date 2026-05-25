using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RecipeDisplayUI : MonoBehaviour
{
    [Serializable]
    public class IngredientSprite
    {
        public string ingredientName;
        public Sprite sprite;
    }

    [Header("UI")]
    [SerializeField] private TMP_Text recipeTitleText;
    [SerializeField] private Transform ingredientsContainer;
    [SerializeField] private RecipeIngredientIconUI ingredientIconPrefab;

    [Header("Sprites")]
    [SerializeField] private List<IngredientSprite> ingredientSprites = new List<IngredientSprite>();

    private readonly List<RecipeIngredientIconUI> spawnedIcons = new List<RecipeIngredientIconUI>();

    private void Start()
    {
        if (GameManager.Instance == null)
        {
            Debug.LogError("RecipeDisplayUI: GameManager.Instance не найден.");
            return;
        }

        GameManager.Instance.OnBurgerStarted += RefreshRecipe;

        RefreshRecipe();
    }

    private void OnDestroy()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.OnBurgerStarted -= RefreshRecipe;
        }
    }

    private void RefreshRecipe()
    {
        if (GameManager.Instance == null)
        {
            return;
        }

        if (ingredientsContainer == null)
        {
            Debug.LogError("RecipeDisplayUI: не назначен Ingredients Container.");
            return;
        }

        if (ingredientIconPrefab == null)
        {
            Debug.LogError("RecipeDisplayUI: не назначен Ingredient Icon Prefab.");
            return;
        }

        ClearRecipeIcons();

        List<string> ingredients = GameManager.Instance.GetCurrentResolvedRecipeIngredients();

        if (recipeTitleText != null)
        {
            recipeTitleText.text = "Burger " + GameManager.Instance.CurrentRecipeNumber;
        }

        foreach (string ingredientName in ingredients)
        {
            Sprite sprite = GetSpriteForIngredient(ingredientName);

            RecipeIngredientIconUI icon = Instantiate(
                ingredientIconPrefab,
                ingredientsContainer
            );

            icon.Setup(sprite);
            spawnedIcons.Add(icon);
        }
    }
    
    private void ClearRecipeIcons()
    {
        foreach (RecipeIngredientIconUI icon in spawnedIcons)
        {
            if (icon != null)
            {
                Destroy(icon.gameObject);
            }
        }

        spawnedIcons.Clear();
    }

    private Sprite GetSpriteForIngredient(string ingredientName)
    {
        string normalizedName = Normalize(ingredientName);

        foreach (IngredientSprite item in ingredientSprites)
        {
            if (Normalize(item.ingredientName) == normalizedName)
            {
                return item.sprite;
            }
        }

        Debug.LogWarning("RecipeDisplayUI: нет спрайта для ингредиента: " + ingredientName);
        return null;
    }

    private string Normalize(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return "";
        }

        return value.Trim().ToLower();
    }
}