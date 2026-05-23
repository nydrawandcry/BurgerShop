using UnityEngine;

public class IngredientItem : MonoBehaviour
{
    [Header("Ingredient Data")]
    public string _ingredientName;

    private Rigidbody _rb;
    private Collider _itemCollider;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _itemCollider = GetComponent<Collider>();
    }

    public void PickUp(Transform holdPoint)
    {
        transform.SetParent(holdPoint);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity;

        if (_rb != null)
        {
            _rb.isKinematic = true;
            _rb.useGravity = false;
        }

        if (_itemCollider != null)
        {
            _itemCollider.enabled = false;
        }
    }

    public void Drop(Vector3 dropPosition)
    {
        transform.SetParent(null);
        transform.position = dropPosition;

        if (_itemCollider != null)
        {
            _itemCollider.enabled = true;
        }

        if (_rb != null)
        {
            _rb.isKinematic = false;
            _rb.useGravity = true;
        }
    }
}