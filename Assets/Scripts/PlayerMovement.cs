using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float Speed;
    public float jumpForce = 5f;
    public Transform groundCheck;
    public LayerMask groundMask;
    public Transform cameraTransform; // Reference to the camera transform

    [SerializeField]private bool isGrounded;
    private PlayerInputActions inputActions;
    [SerializeField]private Vector2 moveInput;
    private Rigidbody rb;

    //public Animator animator;

    private void Awake()
    {
        inputActions = new PlayerInputActions();
        rb = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        inputActions.Player.Enable();
        inputActions.Player.Move.performed += OnMove;
        inputActions.Player.Move.canceled += OnMove;
        inputActions.Player.Jump.performed += OnJump;
    }

    private void OnDisable()
    {
        inputActions.Player.Move.performed -= OnMove;
        inputActions.Player.Move.canceled -= OnMove;
        inputActions.Player.Jump.performed -= OnJump;
        inputActions.Player.Disable();
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.ReadValue<Vector2>();
        Speed = moveSpeed;

        Debug.Log("Walk Key Pressed");
    }

    private void OnJump(InputAction.CallbackContext context)
    {
        Debug.Log("Jump Key Pressed");
        if (isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    private void Update()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, 0.1f, groundMask);
    }

    private void FixedUpdate()
    {
        Vector3 direction = new Vector3(moveInput.x, 0f, moveInput.y);
        Vector3 cameraForward = cameraTransform.forward;
        Vector3 cameraRight = cameraTransform.right;

        // Flatten the camera forward and right vectors
        cameraForward.y = 0f;
        cameraRight.y = 0f;

        cameraForward.Normalize();
        cameraRight.Normalize();

        Vector3 moveDirection = cameraForward * direction.z + cameraRight * direction.x;

        rb.MovePosition(rb.position + moveDirection * Speed * Time.fixedDeltaTime);
        
        if(moveInput.x > 0)
        {
            //animator.SetFloat("moveInput", moveInput.x);
        }
        else if(moveInput.x < 0)
        {
            //animator.SetFloat("moveInput", -moveInput.x);
        }
        else if(moveInput.y > 0)
        {
           //animator.SetFloat("moveInput", moveInput.y);
        }
        else
        {
            //animator.SetFloat("moveInput", -moveInput.y);
        }

        // Rotate the character to face the direction of movement
        if (moveDirection != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(moveDirection, Vector3.up);
            rb.rotation = Quaternion.RotateTowards(transform.rotation, toRotation, 720 * Time.fixedDeltaTime);
        }
    }

    void OnDrawGizmosSelected()
    {
        // Draw a yellow sphere at the transform's position
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(groundCheck.transform.position, 1);
    }
}