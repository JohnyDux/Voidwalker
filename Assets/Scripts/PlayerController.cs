using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlayerController : MonoBehaviour
{
    [Header("Input")]
    public float horizontal;
    public float vertical;

    [Header("Camera")]
    Vector3 cameraForward;
    Vector3 cameraRight;

    [Header("Ground Movement")]
    public float moveSpeed = 6.0f;
    bool isRunning;
    public float jumpSpeed = 8.0f;
    public float flightSpeed = 5f;
    public float gravity = 20.0f;
    public Transform cameraTransform; // Assign your main camera in the Inspector
    public Transform characterMesh; // Assign the character's mesh/model in the Inspector
    public float rotationSpeed = 10.0f; // Speed at which the character rotates
    public CharacterController controller;
    public Vector3 moveDirection = Vector3.zero;

    [Header("Jetpack Movement")]
    public float flightHeight;
    

    [Header("Shooting")]
    public CinemachineVirtualCamera mainCamera;
    public int bulletCount;
    public GameObject prefabShooting;
    public RectTransform uICrosshairRect;
    public GameObject uICrosshairGO;
    public UIController uIController;
    public CinemachineVirtualCamera aimingCamera;
    public Transform aimPointer;
    public Transform playerObj;

    [Header("Extras")]
    public Animator playerAnim;
    public PlayerStats playerStats;
    void Start()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;

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
        if(playerStats.lifeValue<=0){
            playerAnim.SetBool("Die", true);
        }

        // Get input from WASD
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");

        // Get the camera's forward vector (remove Y component for horizontal movement)
        cameraForward = cameraTransform.forward;
        cameraForward.y = 0;
        cameraForward = cameraForward.normalized;

        // Get the camera's right vector (remove Y component for horizontal movement)
        cameraRight = cameraTransform.right;
        cameraRight.y = 0;
        cameraRight = cameraRight.normalized;

        // Calculate movement direction based on camera's orientation
        moveDirection = (cameraForward * vertical + cameraRight * horizontal);

        if (controller.isGrounded)
        {
            if (Input.GetKeyDown(KeyCode.LeftShift))
            {
                moveSpeed = 18.0f;
                isRunning = true;
            }
            else if (Input.GetKeyUp(KeyCode.LeftShift))
            {
                moveSpeed = 6.0f;
                isRunning = false;
            }

            //Only apply speed if there is input
            if (vertical != 0)
                {
                    moveDirection = moveDirection.normalized * moveSpeed;

                    playerAnim.SetBool("moveForward", true);

                    if (isRunning == false)
                    {
                        playerAnim.SetFloat("walkRunBlend", 0);
                    }
                    else
                    {
                        playerAnim.SetFloat("walkRunBlend", 1);
                    }
                }
            else
                {
                    playerAnim.SetBool("moveForward", false);
                }

            // Jump
            if (Input.GetButton("Jump"))
            {
                playerAnim.SetTrigger("Jump");

                moveDirection.y = jumpSpeed;
            }

            //Crouch
            if (Input.GetKeyDown(KeyCode.LeftControl))
            {
                moveSpeed = 0f;
                playerAnim.SetBool("Crouch", true);
            }
            else if(Input.GetKeyUp(KeyCode.LeftControl))
            {
                moveSpeed = 6.0f;
                playerAnim.SetBool("Crouch", false);
            }

            //Fly with Jetpack
            if (Input.GetKeyDown(KeyCode.Q))
            {
                // Start flying
                Fly();
                playerAnim.SetBool("Fly", true);
            }
            else if(Input.GetKeyUp(KeyCode.Q))
            {
                playerAnim.SetBool("Fly", false);
            }

            // Rotate the character to face the movement direction
            if (moveDirection.magnitude > 0.1f) // Only rotate if moving
            {
                // Obtém a posição da câmera
                Vector3 cameraPosition = cameraTransform.position;

                // Define a nova posição do objeto para olhar na direção da câmera
                Vector3 directionToCamera = cameraPosition - transform.position;
                directionToCamera.y = 0;

                if(directionToCamera != Vector3.zero)
                {
                    // Calcula a rotação necessária
                    Quaternion targetRotation = Quaternion.LookRotation(-directionToCamera);

                    // Aplica a rotação ao objeto
                    characterMesh.rotation = Quaternion.Slerp(characterMesh.rotation, targetRotation, rotationSpeed * Time.deltaTime);
                }
            }
        }

        // Apply gravity
        moveDirection.y -= gravity * Time.deltaTime;

        // Move the controller
        controller.Move(moveDirection * moveSpeed * Time.deltaTime);
         
        if (Input.GetMouseButtonDown(0))
        {
            ShootAtTarget();
        }

        if (uIController.weaponAiming == true)
        {
            PointAtTarget(true);
        }
        else if (uIController.weaponAiming == false)
        {
            PointAtTarget(false);
        }
    }

    void ShootAtTarget()
    {
        if(uIController.weaponActive == true && bulletCount > 0)
        {
            // Get the world position of the UI element
            Vector3 worldPosition;
            RectTransformUtility.ScreenPointToWorldPointInRectangle(uICrosshairRect, uICrosshairRect.position, Camera.main, out worldPosition);

            // Create a ray from the camera to the world position
            Ray ray = Camera.main.ScreenPointToRay(uICrosshairRect.position);
            RaycastHit hit;

            // Perform the raycast
            if (Physics.Raycast(ray, out hit))
            {
                // Calculate the position in front of the hit point
                Vector3 spawnPosition = hit.point + hit.normal;

                // Instantiate the prefab at the calculated position with no rotation
                GameObject instantiatedPrefab = Instantiate(prefabShooting, spawnPosition, Quaternion.identity);

                // Destroy the instantiated prefab after a delay
                Destroy(instantiatedPrefab, 5);
            }
        }
    }

    void PointAtTarget(bool aiming)
    {
        if(aiming == true)
        {
            //activate crosshair
            uICrosshairGO.SetActive(true);
            //adjust camera
            aimingCamera.Priority = 10; // Set priority for camera 1
            mainCamera.Priority = 0;  // Set priority for camera 2
            //activate point character animation
        }
        else if(aiming == false)
        {
            //deactivate crosshair
            uICrosshairGO.SetActive(false);
            //adjust camera
            aimingCamera.Priority = 0; // Set priority for camera 1
            mainCamera.Priority = 10;  // Set priority for camera 2
            //deactivate point character animation
        }


    }

    void Fly()
    {
        
    }
}