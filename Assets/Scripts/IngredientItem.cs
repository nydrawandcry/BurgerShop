using UnityEngine;

public class IngredientItem : MonoBehaviour
{
    [Header("Ingredient Data")]
    public string _ingredientName;

    private Rigidbody _rb;
    private Collider[] _itemColliders;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _itemColliders = GetComponentsInChildren<Collider>();
    }

    public void PickUp(Transform holdPoint)
    {
        transform.SetParent(holdPoint, false);
        
        //transform.localPosition = Vector3.zero;
        //transform.localRotation = Quaternion.identity;
        //transform.localScale = Vector3.one;

        if (_rb != null)
        {
            _rb.isKinematic = true;
            _rb.useGravity = false;
        }

        SetCollidersEnabled(false);
        
        Debug.Log(
            "Предмет прикреплен к HoldPoint: " + gameObject.name +
            " | Parent: " + transform.parent.name +
            " | Local Position: " + transform.localPosition +
            " | World Position: " + transform.position
        );
    }
    
    public void PlaceOnBurger(Transform stackRoot, Vector3 localPosition, Quaternion localRotation)
    {
        transform.SetParent(stackRoot);
        transform.localPosition = localPosition;
        transform.localRotation = localRotation;

        if (_rb != null)
        {
            _rb.isKinematic = true;
            _rb.useGravity = false;
        }

        SetCollidersEnabled(false);
    }

    public void Drop(Vector3 dropPosition)
    {
        transform.SetParent(null);
        transform.position = dropPosition;

        if (_itemColliders != null)
        {
            SetCollidersEnabled(true);
        }

        if (_rb != null)
        {
            _rb.isKinematic = false;
            _rb.useGravity = true;
        }
    }
    
    private void SetCollidersEnabled(bool value)
    {
        foreach (Collider itemCollider in _itemColliders)
        {
            itemCollider.enabled = value;
        }
    }
}