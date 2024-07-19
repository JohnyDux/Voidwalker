using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

public class UIController : MonoBehaviour
{
    //Pause
    public bool isPaused;
    public GameObject pauseMenu;

    //Input
    private PlayerInputActions inputActions;
    private Vector2 Input;
    private InputAction pauseAction;
    private InputAction scrollAction;
    [SerializeField]private float scrollValue;
    private float targetFieldOfView;

    //Minimap
    public Transform playerPos;
    public Transform PlayerLocationSphere;
    public float xPos;
    public float yPos;
    public float zPos;
    float scroll;
    public Camera mapCamera;
    public float smoothSpeed;
    public float scrollSensitivity = 0.1f; // Adjust scroll sensitivity

    private void Awake()
    {
        inputActions = new PlayerInputActions();
        isPaused = false;
        pauseMenu.SetActive(false);

        Cursor.lockState = CursorLockMode.Confined;
    }

    private void Update()
    {
        PlayerLocationSphere.position = new Vector3(playerPos.position.x, yPos, playerPos.position.z);

        // Smoothly interpolate the field of view towards the target value
        Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, targetFieldOfView, smoothSpeed * Time.deltaTime);
    }

    private void OnEnable()
    {
        inputActions.Player.Enable();
        pauseAction = inputActions.FindAction("Pause");

        pauseAction.Enable();

        pauseAction.performed += OnPause;

        // Find the Scroll action
        scrollAction = inputActions.FindAction("Scroll");
        // Enable the Scroll action
        scrollAction.Enable();
        // Subscribe to the Scroll action
        scrollAction.performed += OnScroll;
        // Initialize target field of view
        targetFieldOfView = Camera.main.fieldOfView;
    }

    private void OnDisable()
    {
        pauseAction.performed -= OnPause;
        inputActions.Player.Disable();

        // Unsubscribe from the Scroll action
        scrollAction.performed -= OnScroll;

        // Disable the Scroll action
        scrollAction.Disable();
        pauseAction.Disable();
    }

    private void OnPause(InputAction.CallbackContext context)
    {
        if(context.ReadValue<float>() > 0)
        {
            isPaused = !isPaused;
            Time.timeScale = isPaused ? 0 : 1;

            bool check1 = isPaused ? true : false;
            pauseMenu.SetActive(check1);
            Cursor.visible = check1;

            CursorLockMode lockMode = isPaused ? CursorLockMode.Confined : CursorLockMode.Locked;
            Cursor.lockState = lockMode;
        }
    }

    private void OnScroll(InputAction.CallbackContext context)
    {
        scrollValue = context.ReadValue<Vector2>().y;

        // Scale down the scroll value and accumulate it to the target field of view
        targetFieldOfView -= scrollValue * scrollSensitivity;

        // Clamp the targetFieldOfView to a specific range
        targetFieldOfView = Mathf.Clamp(targetFieldOfView, 20f, 100f); // Adjust min and max FOV values as needed
    }
}
