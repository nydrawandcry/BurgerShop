using UnityEngine;

public class IngredientItem : MonoBehaviour
{
    [Header("Ingredient Data")]
    public string _ingredientName;

    [Header("Hold Settings")]
    [SerializeField] private Vector3 holdLocalPosition = new Vector3(0.35f, -0.25f, 1.1f);
    [SerializeField] private Vector3 holdLocalRotation = new Vector3(-70f, 0f, 0f);
    [SerializeField] private Vector3 holdLocalScale = Vector3.one;

    [Header("Burger Stack Settings")]
    [SerializeField] private Vector3 burgerLocalRotation = Vector3.zero;
    [SerializeField] private Vector3 burgerLocalScale = Vector3.one;
    
    private Rigidbody[] _rigidbodies;
    private Collider[] _itemColliders;
    
    private Transform _currentCameraTransform;
    private bool _isHeld;

    private void Awake()
    {
        /*_rigidbodies = GetComponentsInChildren<Rigidbody>(true);
        _itemColliders = GetComponentsInChildren<Collider>(true);*/
        //CacheComponents();
    }

    public void PickUp(Transform cameraTransform)
    {
        if (cameraTransform == null)
        {
            Debug.LogError("IngredientItem.PickUp: cameraTransform == null на объекте ");
            return;
        }
        
        _currentCameraTransform = cameraTransform;
        _isHeld = true;

        transform.SetParent(null, true);
        
        DisablePhysics();
        SetCollidersEnabled(false);
        
        RefreshHoldTransform();

        Debug.Log("Предмет взят: " + gameObject.name);
    }
    
    public void RefreshHoldTransform()
    {
        if (!_isHeld)
        {
            return;
        }

        if (_currentCameraTransform == null)
        {
            Debug.LogError("IngredientItem.RefreshHoldTransform: currentCameraTransform == null на объекте ");
            return;
        }
        
        Vector3 targetPosition = _currentCameraTransform.TransformPoint(holdLocalPosition);
        Quaternion targetRotation = _currentCameraTransform.rotation * Quaternion.Euler(holdLocalRotation);

        transform.position = targetPosition;
        transform.rotation = targetRotation;
        transform.localScale = holdLocalScale;
    }
    
    public void PlaceOnBurger(Transform stackRoot, Vector3 localPosition)
    {
        if (stackRoot == null)
        {
            Debug.LogError("IngredientItem.PlaceOnBurger: stackRoot == null на объекте " + gameObject.name);
            return;
        }

        _isHeld = false;
        _currentCameraTransform = null;

        transform.SetParent(stackRoot, false);

        transform.localPosition = localPosition;
        transform.localRotation = Quaternion.Euler(burgerLocalRotation);
        transform.localScale = burgerLocalScale;

        DisablePhysics();
        SetCollidersEnabled(false);

        Debug.Log("Предмет положен: " + gameObject.name);
    }
    
    public bool TryGetRendererBounds(out Bounds bounds)
    {
        Renderer[] renderers = GetComponentsInChildren<Renderer>(true);

        bounds = new Bounds();

        bool hasRenderer = false;

        foreach (Renderer itemRenderer in renderers)
        {
            if (itemRenderer == null)
            {
                continue;
            }

            if (!hasRenderer)
            {
                bounds = itemRenderer.bounds;
                hasRenderer = true;
            }
            else
            {
                bounds.Encapsulate(itemRenderer.bounds);
            }
        }

        return hasRenderer;
    }

    public void Drop(Vector3 dropPosition)
    {
        _isHeld = false;
        _currentCameraTransform  = null;

        transform.SetParent(null, true);
        transform.position = dropPosition;

        SetCollidersEnabled(true);
        EnablePhysics();
    }
    
    private void DisablePhysics()
    {
        if (_rigidbodies == null)
        {
            _rigidbodies = GetComponentsInChildren<Rigidbody>(true);
        }

        foreach (Rigidbody rb in _rigidbodies)
        {
            if (rb == null)
            {
                continue;
            }

            rb.isKinematic = true;
            rb.useGravity = false;
            
        #if UNITY_6000_0_OR_NEWER
            rb.linearVelocity = Vector3.zero;
        #else
            rb.velocity = Vector3.zero;
        #endif

            rb.angularVelocity = Vector3.zero;
        }
    }

    private void EnablePhysics()
    {
        if (_rigidbodies == null)
        {
            return;
        }

        foreach (Rigidbody rb in _rigidbodies)
        {
            if (rb == null)
            {
                continue;
            }

            rb.isKinematic = false;
            rb.useGravity = true;
        }
    }

    private void SetCollidersEnabled(bool value)
    {
        if (_itemColliders == null)
        {
            _itemColliders = GetComponentsInChildren<Collider>(true);
        }

        foreach (Collider itemCollider in _itemColliders)
        {
            if (itemCollider != null)
            {
                itemCollider.enabled = value;
            }
        }
    }
}