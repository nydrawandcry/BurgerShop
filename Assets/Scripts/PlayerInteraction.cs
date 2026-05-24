using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerInteraction :  MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Camera playerCamera;
    [SerializeField] private Transform holdPoint;

    [Header("Interaction")]
    [SerializeField] private float interactionDistance = 3f;

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
        Debug.DrawRay(
            playerCamera.transform.position,
            playerCamera.transform.forward * interactionDistance,
            Color.red
        );
        
        if (Keyboard.current == null)
        {
            return;
        }

        if (Keyboard.current.eKey.wasPressedThisFrame)
        {
            Interact();
        }
    }

    private void Interact()
    {
        if (playerCamera == null || holdPoint == null)
        {
            Debug.LogError("PlayerInteraction: не назначены playerCamera или holdPoint.");
            return;
        }
        
        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);

        if (!Physics.Raycast(ray, out RaycastHit hit, interactionDistance))
        {
            return; 
        }

        if (heldItem == null)
        {
            TryTakeFromDispenser(hit);
        }
        else
        {
            TryPlaceOnBurger(hit);
        }
    }
    
    private void TryTakeFromDispenser(RaycastHit hit)
    {
        IngredientDispenser dispenser = hit.collider.GetComponentInParent<IngredientDispenser>();

        if (dispenser == null)
        {
            return;
        }

        heldItem = dispenser.SpawnIngredient(holdPoint);

        if (heldItem != null)
        {
            Debug.Log("Взяли ингредиент: " + heldItem._ingredientName);
        }
    }

    private void TryPlaceOnBurger(RaycastHit hit)
    {
        BurgerAssemblyPlace assemblyPlace = hit.collider.GetComponentInParent<BurgerAssemblyPlace>();

        if (assemblyPlace == null)
        {
            return;
        }

        assemblyPlace.PlaceIngredient(heldItem);

        Debug.Log("Положили ингредиент в бургер: " + heldItem._ingredientName);

        heldItem = null;
    }
}