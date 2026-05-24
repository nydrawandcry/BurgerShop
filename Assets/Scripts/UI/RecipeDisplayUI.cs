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
    [SerializeField] private List<Image> ingredientImages = new List<Image>();

    [Header("Sprites")]
    [SerializeField] private List<IngredientSprite> ingredientSprites = new List<IngredientSprite>();

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

        List<string> ingredients = GameManager.Instance.GetCurrentResolvedRecipeIngredients();

        if (recipeTitleText != null)
        {
            recipeTitleText.text = "Burger " + GameManager.Instance.CurrentRecipeNumber;
        }

        for (int i = 0; i < ingredientImages.Count; i++)
        {
            Image image = ingredientImages[i];

            if (image == null)
            {
                continue;
            }

            if (i >= ingredients.Count)
            {
                image.gameObject.SetActive(false);
                continue;
            }

            string ingredientName = ingredients[i];
            Sprite sprite = GetSpriteForIngredient(ingredientName);

            image.sprite = sprite;
            image.enabled = sprite != null;
            image.gameObject.SetActive(true);
        }
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