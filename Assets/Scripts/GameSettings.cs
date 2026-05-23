using UnityEngine;

public enum MeatType
{
    Chicken = 0, 
    Beef = 1
} 
public static class GameSettings 
{ 
    /**
     * * Тип котлеты (курица или говядина - по дефолту говядина)
     */ 
    public static MeatType Meat = MeatType.Chicken; 
    
    /**
     * * Количество ингредиентов в каждом бургере (3 по дефолту)
     */ 
    public static int IngredientCount = 3; 
    
    /**
     * * Время на приготовление 1 бургера (15 секунд по дефолту)
     */ 
    public static int TimePerBurger = 15;

    public static void Set(MeatType meat, int ingredientCount, int timePerBurger)
    {
        Meat = meat; IngredientCount = Mathf.Clamp(ingredientCount, 3, 5); 
        TimePerBurger = Mathf.Clamp(timePerBurger, 5, 60);
    } 
    public static void ResetToDefaults() 
    { Set(MeatType.Chicken, 3, 30); } 
}