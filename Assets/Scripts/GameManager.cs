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
        public string name; public List<string> ingredients; 
        
    } 
    
    //рецепты для каждого уровня (процедурной генерации пока не будет)
    public List<Recipe> level1Recipes = new List<Recipe>(); 
    public List<Recipe> level2Recipes = new List<Recipe>(); 
    public List<Recipe> level3Recipes = new List<Recipe>(); 
    public int currentLevel = 1; 
    private readonly string[] optionalIngredients =
    {
        "салат", 
        "помидор", 
        "сыр", 
        "огурец", 
        "кетчуп"
    };

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this; 
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject); 
            
        }
    }

    void Start()
    {
        GenerateAllRecipes(); 
    } 
    /**
     * * Генерация рецептов на основе настроек
     */ 
    void GenerateAllRecipes() 
    { 
        level1Recipes = GenerateRecipesForLevel(2); //2 рецепта на 1 уровне
        level2Recipes = GenerateRecipesForLevel(3); //3 рецепта на 2 уровне
        level3Recipes = GenerateRecipesForLevel(5); //5 рецептов на 3 уровне
        Debug.Log( $"Рецепты сгенерированы! " + $"Мясо: {GetMeatIngredientName()}, " + $"ингредиентов в рецепте: {GameSettings.IngredientCount}, " + $"время: {GameSettings.TimePerBurger} сек" ); }

    List<Recipe> GenerateRecipesForLevel(int recipeCount)
    {
        List<Recipe> recipes = new List<Recipe>();
        for (int i = 0; i < recipeCount; i++)
        {
            Recipe newRecipe = new Recipe(); 
            newRecipe.name = $"Бургер #{i + 1}"; 
            newRecipe.ingredients = new List<string>(); 
            GenerateIngredientsForRecipe(newRecipe); 
            recipes.Add(newRecipe);
        } 
        return recipes;
    } 
    private void GenerateIngredientsForRecipe(Recipe recipe) 
    { 
        recipe.ingredients.Add("нижняя булка"); 
        recipe.ingredients.Add(GetMeatIngredientName()); 
        int extraIngredientCount = Mathf.Max(0, GameSettings.IngredientCount - 3); 
        List<string> availableIngredients = new List<string>(optionalIngredients); 
        for (int i = 0; i < extraIngredientCount; i++) 
        {
            if (availableIngredients.Count == 0)
            {
                break;
            } 
            int randomIndex = Random.Range(0, availableIngredients.Count); string randomIngredient = availableIngredients[randomIndex]; 
            recipe.ingredients.Add(randomIngredient); // Убираем ингредиент, чтобы он не повторялся в одном рецепте
            availableIngredients.RemoveAt(randomIndex); 
        } 
        recipe.ingredients.Add("верхняя булка"); 
    }

    private string GetMeatIngredientName()
    {
        switch (GameSettings.Meat)
        {
            case MeatType.Chicken: 
                return "курица"; 
            case MeatType.Beef: 
                return "говядина"; 
            default: 
                return "курица";
        }
    }

    /**
     * * Получить рецепты для текущего уровня
     */
    public List<Recipe> GetCurrentLevelRecipes()
    {
        switch (currentLevel)
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
}