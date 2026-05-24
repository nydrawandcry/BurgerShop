using System.Collections.Generic;
using UnityEngine;

public class GameManager :  MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    /**
     * * Структура одного рецепта
     */
    [System.Serializable]
    public class Recipe
    {
        public string name; 
        public List<string> ingredients = new List<string>(); 
    } 
    
    [Header("Level Recipes")]
    [SerializeField] private List<Recipe> level1Recipes = new List<Recipe>();
    [SerializeField] private List<Recipe> level2Recipes = new List<Recipe>();
    [SerializeField] private List<Recipe> level3Recipes = new List<Recipe>();

    public int CurrentLevel { get; private set; } = 1;
    public bool LevelEnded { get; private set; }
    
    private List<Recipe> currentLevelRecipes;
    private int currentRecipeIndex;
    private int currentIngredientIndex;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
    
    public void StartLevel(int level)
    {
        CurrentLevel = level;
        LevelEnded = false;

        currentLevelRecipes = GetRecipesForLevel(CurrentLevel);
        currentRecipeIndex = 0;
        currentIngredientIndex = 0;

        Debug.Log("GameManager: старт уровня " + CurrentLevel);
        PrintCurrentRecipe();
    }
    
    public bool RegisterPlacedIngredient(string ingredientName)
    {
        if (LevelEnded)
        {
            return false;
        }

        if (currentLevelRecipes == null || currentLevelRecipes.Count == 0)
        {
            Debug.LogError("GameManager: нет рецептов для уровня.");
            LoseLevel();
            return false;
        }

        Recipe currentRecipe = currentLevelRecipes[currentRecipeIndex];

        if (currentIngredientIndex >= currentRecipe.ingredients.Count)
        {
            Debug.LogWarning("Игрок положил лишний ингредиент.");
            LoseLevel();
            return false;
        }

        string expectedIngredient = ResolveIngredientName(currentRecipe.ingredients[currentIngredientIndex]);
        string actualIngredient = NormalizeIngredientName(ingredientName);

        Debug.Log("Ожидалось: [" + expectedIngredient + "], положили: [" + actualIngredient + "]");

        if (actualIngredient != expectedIngredient)
        {
            LoseLevel();
            return false;
        }

        currentIngredientIndex++;

        if (currentIngredientIndex >= currentRecipe.ingredients.Count)
        {
            CompleteCurrentBurger();
        }

        return true;
    }
    
    private void CompleteCurrentBurger()
    {
        Debug.Log("Бургер собран правильно: " + currentLevelRecipes[currentRecipeIndex].name);

        currentRecipeIndex++;
        currentIngredientIndex = 0;

        if (currentRecipeIndex >= currentLevelRecipes.Count)
        {
            WinLevel();
        }
        else
        {
            Debug.Log("Переход к следующему бургеру.");
        }
    }
    
    public bool HasMoreRecipes()
    {
        return !LevelEnded && currentLevelRecipes != null && currentRecipeIndex < currentLevelRecipes.Count;
    }
    
    public void WinLevel()
    {
        LevelEnded = true;
        Debug.Log("Победа на уровне.");
    }

    public void LoseLevel()
    {
        LevelEnded = true;
        Debug.Log("Поражение на уровне.");
    }

    private List<Recipe> GetRecipesForLevel(int level)
    {
        switch (level)
        {
            case 1:
                return level1Recipes;

            case 2:
                return level2Recipes;

            case 3:
                return level3Recipes;

            default:
                return level1Recipes;
        }
    }
    
    private string ResolveIngredientName(string ingredientName)
    {
        string normalizedName = NormalizeIngredientName(ingredientName);

        if (normalizedName == "meat")
        {
            return GetMeatIngredientName();
        }

        return normalizedName;
    }

    private string GetMeatIngredientName()
    {
        switch (GameSettings.Meat)
        {
            case MeatType.Chicken: 
                return "chicken"; 
            case MeatType.Beef: 
                return "beef"; 
            default: 
                return "chicken";
        }
    }
    
    private string NormalizeIngredientName(string ingredientName)
    {
        if (string.IsNullOrWhiteSpace(ingredientName))
        {
            return "";
        }

        return ingredientName.Trim().ToLower();
    }
    
    private void PrintCurrentRecipe()
    {
        if (currentLevelRecipes == null || currentLevelRecipes.Count == 0)
        {
            Debug.LogWarning("Нет рецептов для текущего уровня.");
            return;
        }

        Recipe recipe = currentLevelRecipes[currentRecipeIndex];

        List<string> resolvedIngredients = new List<string>();

        foreach (string ingredient in recipe.ingredients)
        {
            resolvedIngredients.Add(ResolveIngredientName(ingredient));
        }

        Debug.Log(
            "Текущий рецепт: " + recipe.name + " | " +
            string.Join(" -> ", resolvedIngredients)
        );
    }
}