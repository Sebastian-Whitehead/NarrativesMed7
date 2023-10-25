using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Require the presence of a CharacterController component on the GameObject
[RequireComponent(typeof(CharacterController))]

public class Controller : MonoBehaviour
{
    // Public variables for controlling movement and camera settings
    public float walkingSpeed = 7.5f; // Walking speed
    public float gravity = 20.0f; // Gravity applied to the character
    public Camera playerCamera; // Reference to the player's camera
    public float lookSpeed = 2.0f; // Sensitivity for looking around
    public float lookXLimit = 45.0f; // Limit for looking up and down

    // Private variables for character control
    CharacterController characterController;
    Vector3 moveDirection = Vector3.zero; // The movement direction vector
    float rotationX = 0; // Current rotation on the X-axis

    // Hidden from the Unity Inspector - a flag to enable or disable movement
    [HideInInspector]
    public bool canMove = true;

    void Start()
    {
        // Get a reference to the CharacterController component attached to this GameObject
        characterController = GetComponent<CharacterController>();

        // Lock the cursor and make it invisible
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        // Calculate movement direction based on user input

        // Determine the forward and right vectors based on the player's orientation
        Vector3 forward = transform.TransformDirection(Vector3.forward);
        Vector3 right = transform.TransformDirection(Vector3.right);

        // Calculate the current speed in the X and Y directions based on input and movement flag
        float curSpeedX = canMove ? walkingSpeed * Input.GetAxis("Vertical") : 0;
        float curSpeedY = canMove ? walkingSpeed * Input.GetAxis("Horizontal") : 0;

        // Preserve the vertical movement direction
        float movementDirectionY = moveDirection.y;

        // Combine the movement vectors to create the final moveDirection vector
        moveDirection = (forward * curSpeedX) + (right * curSpeedY);

        // Apply gravity to the character if not grounded
        if (!characterController.isGrounded)
        {
            moveDirection.y -= gravity * Time.deltaTime;
        }

        // Move the character controller
        characterController.Move(moveDirection * Time.deltaTime);

        // Update the player's and camera's rotation
        
        // Check if movement is allowed
        if (canMove)
        {
            // Adjust the vertical rotation based on mouse input
            rotationX += -Input.GetAxis("Mouse Y") * lookSpeed;
            rotationX = Mathf.Clamp(rotationX, -lookXLimit, lookXLimit);

            // Apply the rotation to the camera's local rotation
            playerCamera.transform.localRotation = Quaternion.Euler(rotationX, 0, 0);

            // Adjust the horizontal rotation based on mouse input
            transform.rotation *= Quaternion.Euler(0, Input.GetAxis("Mouse X") * lookSpeed, 0);
        }
    }
}
