using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField]float horizontal;
    [SerializeField]float vertical;
    [SerializeField] private float speed;
    [SerializeField] private float walk;
    [SerializeField] private float run;
    [SerializeField] private float crouch;
    public bool isMoving, isCrouching, isRunning;
    private float X, Y;

    [Header("Jump/Gravity Settings")]
    [SerializeField] private float jumpHeight = 2f;
    [SerializeField] private float gravity = -9.81f;
    [SerializeField] private float groundCheckDistance = 0.4f;
    [SerializeField] private LayerMask groundMask;
    private Vector3 velocity;
    [SerializeField] private bool isGrounded;

    [Header("Flight Settings")]
    [SerializeField] private float flySpeed = 10f;
    [SerializeField] private KeyCode flyKey = KeyCode.V;
    [SerializeField] private KeyCode goUpKey = KeyCode.R;
    [SerializeField] private KeyCode goDownKey = KeyCode.F;
    public float ascendSpeed = 2f;
    public float descendSpeed = 2f;
    public float flyHeight;
    private bool isFlying = false;
    [SerializeField] private GameObject[] particleFire;
    [SerializeField] private float turnSpeed;

    [Header("Life Settings")]
    public PlayerStats stats;
    private bool isAlive = true;

    [Header("Weapons")]
    public bool pistolActive;
    public bool shotgunActive;

    public UIController uiRef;

    public CharacterController controller;
    public Transform groundCheck;

    public CinemachineFreeLook virtualCamera; // Reference to the Cinemachine Virtual Camera
    float turnSmoothVelocity;
    public float turnSmoothTime = 0.1f;


    public Animator animator;

    private void Awake()
    {
        // Create ground check object
        GameObject check = new GameObject("GroundCheck");
        check.transform.SetParent(transform);
        check.transform.localPosition = Vector3.zero;
        groundCheck = check.transform;

        //Disable fire
        foreach (GameObject i in particleFire)
        {
            i.SetActive(false);
        }
    }


    private void Update()
    {
        if (!isAlive) return;

        HandleGroundCheck();
        HandleMovement();
        HandleJump();
        HandleCrouch();
        HandleFlightToggle();
        HandleFlight();
        HandleHacking();

        if(uiRef.weaponIndex == 0)
        {
            HandlePistol();
        }
        else if(uiRef.weaponIndex == 1)
        {
            HandleShotgun();
        }
    }
    private void HandleGroundCheck()
    {
        isGrounded = Physics.CheckSphere(groundCheck.position, groundCheckDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }
    }

    private void HandleMovement()
    {
        if (isFlying) return;

        Transform cam = virtualCamera.transform;

        //walk
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");
        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        if (direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            controller.Move(moveDir.normalized * speed * Time.deltaTime);
            animator.SetBool("moveForward", true);
            animator.SetFloat("walkRunBlend", 0);
        }
        else
        {
            animator.SetBool("moveForward", false);
        }

        if (Input.GetKey(KeyCode.LeftShift))
        {
            isRunning = true;
            animator.SetFloat("walkRunBlend", 1);
        }
        else
        {
            animator.SetFloat("walkRunBlend", 0);
        }

        if(isRunning == true)
        {
            speed = run;
        }
        else
        {
            speed = walk;
        }

        //Crouch
        if (Input.GetKey(KeyCode.LeftControl))
        {
            HandleCrouch();
        }
        //Jump
        else if (Input.GetKey(KeyCode.Space))
        {
            HandleJump();
        }


        // Apply gravity
        velocity.y -= gravity * Time.deltaTime;
        
        controller.Move(velocity * Time.deltaTime);
    }

    private void HandleJump()
    {
        if (isFlying) return;

        if (isGrounded && Input.GetButtonDown("Jump"))
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            animator.SetTrigger("Jump");
        }
    }

    private void HandleCrouch()
    {
        if (isFlying) return;

        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            isCrouching = true;
            isRunning = false;
            speed = 0;
            animator.SetBool("Crouch", true);
        }
        else if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            isRunning = false;
            isCrouching = false;
            speed = walk;
            animator.SetBool("Crouch", false);
        } 
    }

    private void HandleFlightToggle()
    {
        if (Input.GetKeyDown(flyKey) && isFlying == false)
        {
            isFlying = true;
        }
        else if (Input.GetKeyUp(flyKey) && isFlying == true)
        {
            isFlying = false;
        }
        animator.SetBool("Fly", isFlying);
        foreach (GameObject i in particleFire)
        {
            i.SetActive(isFlying);
        }
    }

    private void HandleFlight()
    {
        if (isFlying == true)
        {
            gravity = 0;

            // Get the horizontal and vertical input
            float horizontalInput = Input.GetAxis("Horizontal"); // A/D or Left/Right arrow keys
            float verticalInput = Input.GetAxis("Vertical"); // W/S or Up/Down arrow keys

            // Calculate the movement direction
            Vector3 moveDirection = new Vector3(horizontalInput, 0, verticalInput).normalized;

            // Move the character in the direction of the camera
            transform.position += moveDirection * flySpeed * Time.deltaTime;

            if (Input.GetKey(goUpKey)) // Ascend
            {
                transform.Translate(Vector3.up * flyHeight/10);
            }
            else if (Input.GetKey(goDownKey)) // Descend
            {
                transform.Translate(Vector3.down * flyHeight/10);
            }
        }
        else
        {
            gravity = 20;
        }
    }

    public void TakeDamage(float damage)
    {
        if (!isAlive) return;

        stats.lifeValue -= damage;
        if (stats.lifeValue <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        isAlive = false;
        // Add death animation or effects here
        animator.SetBool("Die", true);
        Debug.Log("Player has died!");
    }

    private void HandlePistol()
    {
        if (Input.GetKeyDown(KeyCode.Z) && pistolActive == false)
        {
            //draw pistol animation
            pistolActive = true;
        }
        else if (Input.GetKeyDown(KeyCode.Z) && pistolActive == true)
        {
            //draw pistol animation
            pistolActive = false;
        }
        animator.SetBool("DrawPistol", pistolActive);

        if (pistolActive == true)
        {
            if (Input.GetMouseButtonDown(0))
            {
                //shoot
                animator.SetTrigger("Shoot");
            }

            else if (Input.GetMouseButtonUp(0))
            {
                //stop shooting
                animator.ResetTrigger("Shoot");
            }
        }
    }

    private void HandleShotgun()
    {
        if (Input.GetKeyDown(KeyCode.K) && shotgunActive == false)
        {
            //draw pistol animation
            shotgunActive = true;
        }
        else if (Input.GetKeyDown(KeyCode.K) && shotgunActive == true)
        {
            //draw pistol animation
            shotgunActive = false;
        }
        animator.SetBool("DrawShotgun", shotgunActive);

        if (shotgunActive == true)
        {
            if (Input.GetMouseButtonDown(0))
            {
                //shoot
                animator.SetTrigger("Shoot");
            }

            else if (Input.GetMouseButtonUp(0))
            {
                //stop shooting
                animator.ResetTrigger("Shoot");
            }
        }
    }
    private void HandleHacking()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            //find something to hack

            //play the animation
            animator.SetBool("Hack", true);
        }
        else if (Input.GetKeyUp(KeyCode.P))
        {
            animator.SetBool("Hack", false);
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (groundCheck != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckDistance);
        }
    }
}