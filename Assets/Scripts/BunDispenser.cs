using UnityEngine;

public class BunDispenser : MonoBehaviour
{
    [Header("Bottom Bun")]
    [SerializeField] private IngredientItem bottomBunPrefab;
    [SerializeField] private string bottomBunName = "bottom loaf";

    [Header("Top Bun")]
    [SerializeField] private IngredientItem topBunPrefab;
    [SerializeField] private string topBunName = "top loaf";

    [Header("State")]
    [SerializeField] private bool bottomBunAlreadyTaken = false;
    
    public IngredientItem SpawnIngredient()
    {
        IngredientItem prefabToSpawn;
        string ingredientName;

        if (!bottomBunAlreadyTaken)
        {
            prefabToSpawn = bottomBunPrefab;
            ingredientName = bottomBunName;
            bottomBunAlreadyTaken = true;
        }
        else
        {
            prefabToSpawn = topBunPrefab;
            ingredientName = topBunName;
        }

        if (prefabToSpawn == null)
        {
            Debug.LogError("BunDispenser: не назначен prefab булки на объекте " + gameObject.name);
            return null;
        }

        IngredientItem newBun = Instantiate(prefabToSpawn);
        newBun._ingredientName = ingredientName;

        Debug.Log("BunDispenser создал: " + ingredientName);

        return newBun;
    }

    public void ResetBunOrder()
    {
        bottomBunAlreadyTaken = false;
        Debug.Log("BunDispenser сброшен: следующая булка снова будет нижней.");
    }
}