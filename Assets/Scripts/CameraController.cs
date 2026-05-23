using UnityEngine;

public class CameraController : MonoBehaviour
{
    /**
     * Чувствительность
     */
    public float sensitivity = 2.0f;

    /**
     * Максимальный угол вращения по вертикали
     */
    public float maxYAngle = 80.0f;

    private float rotationX = 0.0f;

    private void Update()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");
        
        transform.parent.Rotate(Vector3.up * mouseX * sensitivity);
        
        rotationX -= mouseY * sensitivity;
        rotationX = Mathf.Clamp(rotationX, -maxYAngle, maxYAngle);
        transform.localRotation = Quaternion.Euler(rotationX, 0.0f, 0.0f);
    }
}