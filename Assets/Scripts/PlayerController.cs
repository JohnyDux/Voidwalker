using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 6.0f;
    public float jumpSpeed = 8.0f;
    public float gravity = 20.0f;
    public Transform cameraTransform; // Assign your main camera in the Inspector
    public Transform characterMesh; // Assign the character's mesh/model in the Inspector
    public float rotationSpeed = 10.0f; // Speed at which the character rotates

    private CharacterController controller;
    public Vector3 moveDirection = Vector3.zero;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;

        controller = GetComponent<CharacterController>();
        if (cameraTransform == null)
        {
            Debug.LogError("Camera transform not assigned! Please assign the main camera in the Inspector.");
            enabled = false; // Disable the script if no camera is assigned.
        }
        if (characterMesh == null)
        {
            Debug.LogError("Character mesh not assigned! Please assign the character's mesh in the Inspector.");
            enabled = false;
        }
    }

    void Update()
    {
        if (controller.isGrounded)
        {
            // Get input from WASD
            float horizontal = Input.GetAxisRaw("Horizontal");
            float vertical = Input.GetAxisRaw("Vertical");

            // Get the camera's forward vector (remove Y component for horizontal movement)
            Vector3 cameraForward = cameraTransform.forward;
            cameraForward.y = 0;
            cameraForward = cameraForward.normalized;

            // Get the camera's right vector (remove Y component for horizontal movement)
            Vector3 cameraRight = cameraTransform.right;
            cameraRight.y = 0;
            cameraRight = cameraRight.normalized;

            // Calculate movement direction based on camera's orientation
            moveDirection = (cameraForward * vertical + cameraRight * horizontal);

            //Only apply speed if there is input
            if (moveDirection.magnitude > 0.1f)
            {
                moveDirection = moveDirection.normalized * moveSpeed;
            }

            // Jump
            if (Input.GetButton("Jump"))
            {
                moveDirection.y = jumpSpeed;
            }

            // Rotate the character to face the movement direction
            if (moveDirection.magnitude > 0.1f) // Only rotate if moving
            {
                Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
                characterMesh.rotation = Quaternion.Slerp(characterMesh.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            }
        }

        // Apply gravity
        moveDirection.y -= gravity * Time.deltaTime;

        // Move the controller
        controller.Move(moveDirection * Time.deltaTime);
    }
}