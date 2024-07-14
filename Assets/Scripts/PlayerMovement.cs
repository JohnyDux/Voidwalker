using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    public float speed;
    public float jumpForce;
    public Camera referenceCamera;
    public GameObject playerMesh;

    //Jump
    public LayerMask groundLayer;
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;

    private Vector3 direction = Vector3.zero;
    public Rigidbody rb;
    public bool isGrounded;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;
    }

    void Update()
    {
        CheckGroundStatus();

        if (referenceCamera == null)
        {
            Debug.LogError("Reference Camera not assigned.");
            return;
        }
        if (isGrounded == true) { 

            Vector3 direction = Vector3.zero;

            if (Input.GetKey(KeyCode.W))
        {
            direction += referenceCamera.transform.forward;
        }
            if (Input.GetKey(KeyCode.S))
        {
            direction -= referenceCamera.transform.forward;
        }
            if (Input.GetKey(KeyCode.A))
        {
            MoveLeft(ref direction);
        }
            if (Input.GetKey(KeyCode.D))
        {
            MoveRight(ref direction);
        }

            direction.y = 0; // Keep movement in the horizontal plane

            if (direction != Vector3.zero)
        {
            direction.Normalize();
            // Move the character in the calculated direction
            transform.position += direction * speed * Time.deltaTime;
            // Rotate the mesh towards the movement direction
            RotateMeshTowardsDirection(direction);
        }
            rb.isKinematic = true;
        }
        if (Input.GetKey(KeyCode.Space) && isGrounded == true)
        {
            rb.isKinematic = false;
            Jump();
        }
    }

    void MoveLeft(ref Vector3 direction)
    {
        direction -= referenceCamera.transform.right;
    }

    void MoveRight(ref Vector3 direction)
    {
        direction += referenceCamera.transform.right;
    }

    void RotateMeshTowardsDirection(Vector3 direction)
    {
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        playerMesh.transform.rotation = Quaternion.Slerp(playerMesh.transform.rotation, targetRotation, Time.deltaTime * speed);
    }

    void Jump()
    {
        rb.velocity = new Vector3(0, jumpForce, 0);
    }

    void CheckGroundStatus()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundCheckRadius, groundLayer, QueryTriggerInteraction.Ignore);
    }

    void OnDrawGizmosSelected()
    {
        if (groundCheck == null) return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }
}