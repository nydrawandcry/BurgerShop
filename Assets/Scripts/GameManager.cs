using System.Collections.Generic;
using UnityEngine;

public class GameManager :  MonoBehaviour
{
    public static GameManager Instance;

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

    [Header("Current Level")]
    [SerializeField] private int currentLevel = 1;

    [Header("Scene References")]
    [SerializeField] private BurgerAssemblyPlace burgerAssemblyPlace;
    [SerializeField] private BunDispenser bunDispenser;

    [Header("Result Panels")]
    [SerializeField] private GameObject winPanel;
    [SerializeField] private GameObject losePanel;
    
    private List<Recipe> currentLevelRecipes;
    private int currentRecipeIndex;
    private int currentIngredientIndex;
    private bool levelEnded;

    public bool LevelEnded => levelEnded;

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

    void Start()
    {
        StartLevel(currentLevel); 
    } 
    
    public void StartLevel(int level)
    {
        currentLevel = level;
        currentLevelRecipes = GetRecipesForLevel(currentLevel);

        currentRecipeIndex = 0;
        currentIngredientIndex = 0;
        levelEnded = false;

        if (winPanel != null)
        {
            winPanel.SetActive(false);
        }

        if (losePanel != null)
        {
            losePanel.SetActive(false);
        }

        if (burgerAssemblyPlace != null)
        {
            burgerAssemblyPlace.ClearBurger();
        }

        if (bunDispenser != null)
        {
            bunDispenser.ResetBunOrder();
        }

        Debug.Log("Старт уровня: " + currentLevel);
        Debug.Log("Рецептов на уровне: " + currentLevelRecipes.Count);

        PrintCurrentRecipe();
    }
    
    public void RegisterPlacedIngredient(string ingredientName)
    {
        if (levelEnded)
        {
            return;
        }

        if (currentLevelRecipes == null || currentLevelRecipes.Count == 0)
        {
            Debug.LogError("На уровне нет рецептов.");
            LoseLevel();
            return;
        }

        Recipe currentRecipe = currentLevelRecipes[currentRecipeIndex];

        if (currentIngredientIndex >= currentRecipe.ingredients.Count)
        {
            Debug.LogError("Игрок положил лишний ингредиент после окончания рецепта.");
            LoseLevel();
            return;
        }

        string expectedIngredient = ResolveIngredientName(currentRecipe.ingredients[currentIngredientIndex]);
        string actualIngredient = NormalizeIngredientName(ingredientName);

        Debug.Log(
            "Проверка ингредиента. Ожидалось: [" + expectedIngredient + "], положили: [" + actualIngredient + "]"
        );

        if (actualIngredient != expectedIngredient)
        {
            Debug.LogWarning("Неверный ингредиент. Поражение.");
            LoseLevel();
            return;
        }

        currentIngredientIndex++;

        if (currentIngredientIndex >= currentRecipe.ingredients.Count)
        {
            CompleteCurrentBurger();
        }
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

        Invoke(nameof(PrepareNextBurger), 0.5f);
    }
    
    private void PrepareNextBurger()
    {
        if (levelEnded)
        {
            return;
        }

        if (burgerAssemblyPlace != null)
        {
            burgerAssemblyPlace.ClearBurger();
        }

        if (bunDispenser != null)
        {
            bunDispenser.ResetBunOrder();
        }

        PrintCurrentRecipe();
    }

    private void WinLevel()
    {
        levelEnded = true;

        Debug.Log("Победа! Все бургеры уровня собраны правильно.");

        if (winPanel != null)
        {
            winPanel.SetActive(true);
        }
    }

    private void LoseLevel()
    {
        levelEnded = true;

        Debug.Log("Поражение.");

        if (losePanel != null)
        {
            losePanel.SetActive(true);
        }
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