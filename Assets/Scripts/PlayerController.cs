using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    /**
     * Скорость передвижения
     */
    public float moveSpeed = 5.0f;
    
    /**
     * Контроллер игрока
     */
    private CharacterController _characterController;

    private void Start()
    {
        _characterController = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");
        
        Vector3 moveDirection = transform.forward * verticalInput + transform.right * horizontalInput;
        
        moveDirection.y -= 9.81f * Time.deltaTime;
        
        _characterController.Move(moveDirection * moveSpeed * Time.deltaTime);
    }
}