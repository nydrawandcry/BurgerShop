using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    /**
     * Скорость передвижения
     */
    public float moveSpeed = 100.0f;
    
    /**
     * Гравитация
     */
    public float gravity = -9.81f;
    
    /**
     * Контроллер игрока
     */
    private CharacterController _characterController;
    
    private float verticalVelocity;

    private void Start()
    {
        _characterController = GetComponent<CharacterController>();
        
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        if(Keyboard.current == null)
        {
            return;
        }
        
        Vector2 input = ReadMovementInput();
        
        Vector3 moveDirection = transform.forward * input.y + transform.right * input.x;
        
        if (moveDirection.magnitude > 1f)
        {
            moveDirection.Normalize();
        }

        ApplyGravity();

        Vector3 velocity = moveDirection * moveSpeed;
        velocity.y = verticalVelocity;

        _characterController.Move(velocity * Time.deltaTime);
    }
    
    private Vector2 ReadMovementInput()
    {
        Vector2 input = Vector2.zero;

        if (Keyboard.current.wKey.isPressed)
        {
            input.y += 1f;
        }

        if (Keyboard.current.sKey.isPressed)
        {
            input.y -= 1f;
        }

        if (Keyboard.current.dKey.isPressed)
        {
            input.x += 1f;
        }

        if (Keyboard.current.aKey.isPressed)
        {
            input.x -= 1f;
        }

        return input;
    }
    
    private void ApplyGravity()
    {
        if (_characterController.isGrounded && verticalVelocity < 0f)
        {
            verticalVelocity = -2f;
        }

        verticalVelocity += gravity * Time.deltaTime;
    }
}