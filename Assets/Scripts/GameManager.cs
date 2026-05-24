using System;
using System.Collections.Generic;
using UnityEngine;

public enum LevelResult
{
    None,
    Win,
    Lose
}

public class GameManager :  MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public LevelResult CurrentResult { get; private set; } = LevelResult.None;
    
    public event Action OnBurgerStarted;

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
    
    public int CurrentRecipeNumber => currentRecipeIndex + 1;

    public int TotalRecipesInLevel
    {
        get
        {
            if (currentLevelRecipes == null)
            {
                return 0;
            }

            return currentLevelRecipes.Count;
        }
    }

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
        CurrentResult = LevelResult.None;
        CurrentLevel = level;
        LevelEnded = false;

        currentLevelRecipes = GetRecipesForLevel(CurrentLevel);
        currentRecipeIndex = 0;
        currentIngredientIndex = 0;

        Debug.Log("GameManager: старт уровня " + CurrentLevel);
        PrintCurrentRecipe();
        
        OnBurgerStarted?.Invoke();
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
            return;
        }
        
        Debug.Log("Переход к следующему бургеру.");
        PrintCurrentRecipe();
        
        OnBurgerStarted?.Invoke();
    }
    
    public bool HasMoreRecipes()
    {
        return !LevelEnded && currentLevelRecipes != null && currentRecipeIndex < currentLevelRecipes.Count;
    }
    
    public void WinLevel()
    {
        if (LevelEnded)
        {
            return;
        }
        
        LevelEnded = true;
        CurrentResult = LevelResult.Win;
        Debug.Log("Победа на уровне.");
    }

    public void LoseLevel()
    {
        if (LevelEnded)
        {
            return;
        }

        LevelEnded = true;
        CurrentResult = LevelResult.Lose;
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
    
    public List<string> GetCurrentResolvedRecipeIngredients()
    {
        List<string> result = new List<string>();

        if (currentLevelRecipes == null || currentLevelRecipes.Count == 0)
        {
            return result;
        }

        if (currentRecipeIndex < 0 || currentRecipeIndex >= currentLevelRecipes.Count)
        {
            return result;
        }

        Recipe recipe = currentLevelRecipes[currentRecipeIndex];

        foreach (string ingredient in recipe.ingredients)
        {
            result.Add(ResolveIngredientName(ingredient));
        }

        return result;
    }
}