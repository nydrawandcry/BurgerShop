using UnityEngine;
using UnityEngine.InputSystem;

public class CameraController : MonoBehaviour
{
    /**
     * Чувствительность
     */
    public float sensitivity = 3.0f;

    /**
     * Максимальный угол вращения по вертикали
     */
    public float maxYAngle = 80.0f;

    private float rotationX = 0.0f;

    private void Update()
    {
        if (Mouse.current == null)
        {
            return;
        }
        
        Vector2 mouseDelta = Mouse.current.delta.ReadValue();
        
        float mouseX = mouseDelta.x * sensitivity;
        float mouseY = mouseDelta.y * sensitivity;

        if (transform.parent != null)
        {
            transform.parent.Rotate(Vector3.up * mouseX);
        }
        
        rotationX += mouseY;
        rotationX = Mathf.Clamp(rotationX, -maxYAngle, maxYAngle);
        transform.localRotation = Quaternion.Euler(rotationX, 0.0f, 0.0f);
    }
}