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
        if (Keyboard.current == null)
        {
            return;
        }

        if (Keyboard.current.eKey.wasPressedThisFrame)
        {
            if (heldItem == null)
            {
                TryPickUpItem();
            }
            else
            {
                DropHeldItem();
            }
        }
    }

    private void TryPickUpItem()
    {
        Ray ray = new Ray(playerCamera.transform.position, playerCamera.transform.forward);

        if (Physics.Raycast(ray, out RaycastHit hit, interactionDistance))
        {
            IngredientItem item = hit.collider.GetComponent<IngredientItem>();

            if (item != null)
            {
                heldItem = item;
                heldItem.PickUp(holdPoint);

                Debug.Log("Взяли ингредиент: " + heldItem._ingredientName);
            }
        }
    }

    private void DropHeldItem()
    {
        Vector3 dropPosition = playerCamera.transform.position + playerCamera.transform.forward * 1.5f;

        heldItem.Drop(dropPosition);

        Debug.Log("Выбросили ингредиент: " + heldItem._ingredientName);

        heldItem = null;
    }
}