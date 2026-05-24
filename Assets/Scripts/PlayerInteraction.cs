using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerInteraction :  MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Camera playerCamera;

    [Header("Interaction")]
    [SerializeField] private float interactionDistance = 10f;

    private IngredientItem heldItem;

    private void Awake()
    {
        if (playerCamera == null)
        {
            playerCamera = Camera.main;
        }
    }

    private void Update()
    {
        if (playerCamera == null)
        {
            Debug.LogError("PlayerInteraction: playerCamera не назначена.");
            return;
        }
        
        Ray ray = playerCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));

        Debug.DrawRay(
            ray.origin,
            ray.direction * interactionDistance,
            Color.red
        );
        
        if (Keyboard.current == null)
        {
            return;
        }

        if (Keyboard.current.eKey.wasPressedThisFrame)
        {
            Debug.Log("E нажата");
            Interact(ray);
        }
    }
    
    private void LateUpdate()
    {
        if (heldItem != null)
        {
            heldItem.RefreshHoldTransform();
        }
    }

    private void Interact(Ray ray)
    {
        RaycastHit[] allHits = Physics.RaycastAll(
            ray,
            interactionDistance,
            ~0,
            QueryTriggerInteraction.Collide
        );

        if (allHits.Length == 0)
        {
            Debug.Log("RaycastAll: вообще ни во что не попал.");
            return;
        }
        
        System.Array.Sort(allHits, (a, b) => a.distance.CompareTo(b.distance));
        Debug.Log("RaycastAll попал в объектов: " + allHits.Length);

        
        foreach (RaycastHit itemHit in allHits)
        {
            Debug.Log(
                "Hit: " + itemHit.collider.gameObject.name +
                " | Layer: " + LayerMask.LayerToName(itemHit.collider.gameObject.layer) +
                " | Distance: " + itemHit.distance
            );
        }
        
        if (heldItem == null)
        {
            TryTakeFromDispenser(allHits);
        }
        else
        {
            TryPlaceOnBurger(allHits);
        }
    }
    
    private void TryTakeFromDispenser(RaycastHit[] hits)
    {
        foreach (RaycastHit hit in hits)
        {
            //для булок!
            BunDispenser bunDispenser = hit.collider.GetComponentInParent<BunDispenser>();

            if (bunDispenser != null)
            {
                Debug.Log("BunDispenser найден на объекте: " + bunDispenser.gameObject.name);

                heldItem = bunDispenser.SpawnIngredient();

                if (heldItem != null)
                {
                    heldItem.PickUp(playerCamera.transform);
                    Debug.Log("Взяли булку: " + heldItem._ingredientName);
                }
                else
                {
                    Debug.LogError("BunDispenser вернул null.");
                }

                return;
            }
            //для булок кончилось
            
            IngredientDispenser dispenser = hit.collider.GetComponentInParent<IngredientDispenser>();

            if (dispenser == null)
            {
                continue;
            }

            Debug.Log("IngredientDispenser найден на объекте: " + dispenser.gameObject.name);

            heldItem = dispenser.SpawnIngredient();

            if (heldItem != null)
            {
                heldItem.PickUp(playerCamera.transform);
                Debug.Log("Взяли ингредиент: " + heldItem._ingredientName);
            }
            else
            {
                Debug.LogError("SpawnIngredient вернул null.");
            }

            return;
        }

        Debug.LogWarning("Среди объектов, в которые попал луч, нет IngredientDispenser.");

    }

    private void TryPlaceOnBurger(RaycastHit[] hits)
    {
        foreach (RaycastHit hit in hits)
        {
            BurgerAssemblyPlace assemblyPlace = hit.collider.GetComponentInParent<BurgerAssemblyPlace>();

            if (assemblyPlace == null)
            {
                continue;
            }

            assemblyPlace.PlaceIngredient(heldItem);

            Debug.Log("Положили ингредиент в бургер: " + heldItem._ingredientName);

            heldItem = null;

            return;
        }

        Debug.LogWarning("Среди объектов, в которые попал луч, нет BurgerAssemblyPlace.");
    }
}