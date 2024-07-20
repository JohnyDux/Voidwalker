using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

public class UIController : MonoBehaviour
{
    [Header("Pause")]
    public bool isPaused;
    public GameObject pauseMenu;

    [Header("Input")]
    private PlayerInputActions inputActions;
    private InputAction pauseAction;
    private InputAction scrollAction;

    [Header("Minimap")]
    //Player Pos
    public Transform playerPos;
    public Transform PlayerLocationSphere;
    public float xPos;
    public float yPos;
    public float zPos;

    [Header("Minimap")]
    //Mouse Scroll
    Vector3 originalCameraPosition;
    public float zOffset = 0f;
    public float minX, maxX; // Minimum and maximum X position for the camera
    public float minY, maxY; // Minimum and maximum Y position for the camera
    public float MapCamMoveSpeed; // Speed factor for movement
    public float edgeThreshold = 0.1f; // Threshold for detecting edge of the screen
    public Color edgeColor = Color.red; // Color for the edge threshold visualization
    public float edgeThickness = 2f; // Thickness of the edge rectangle lines
    float scroll;
    public Camera mapCamera;
    public float scrollSpeed = 1f; // Adjust this value to control the scroll sensitivity
    public float curOrthoSize;
    public float minOrthoSize = 1f; // Minimum orthographic size
    public float maxOrthoSize = 20f; // Maximum orthographic size


    private void Awake()
    {
        inputActions = new PlayerInputActions();
        isPaused = false;
        pauseMenu.SetActive(false);

        Cursor.lockState = CursorLockMode.Confined;

        originalCameraPosition = mapCamera.transform.position;
    }

    private void OnEnable()
    {
        inputActions.Player.Enable();

        //Pause
        pauseAction = inputActions.FindAction("Pause");
        pauseAction.Enable();
        pauseAction.performed += OnPause;

        //Scroll
        scrollAction = inputActions.FindAction("Scroll");
        scrollAction.Enable();
        scrollAction.performed += OnScroll;
        scrollAction.canceled += OnScroll;
    }

    private void OnDisable()
    {
        //Pause
        pauseAction.performed -= OnPause;
        pauseAction.Disable();


        // Scroll
        scrollAction.performed -= OnScroll;
        scrollAction.canceled -= OnScroll;
        scrollAction.Disable();

        inputActions.Player.Disable();
    }

    private void Update()
    {
        PlayerLocationSphere.position = new Vector3(playerPos.position.x, yPos, playerPos.position.z);
        curOrthoSize = mapCamera.orthographicSize;

        //Minimap Mouse
        if (isPaused)
        {
            if (Input.GetMouseButton(0))
            {
                // Get the mouse position in screen coordinates
                Vector3 mouseScreenPos = Input.mousePosition;

                // Convert mouse position to normalized coordinates (0 to 1)
                Vector2 normalizedMousePos = new Vector2(mouseScreenPos.x / Screen.width, mouseScreenPos.y / Screen.height);

                // Calculate edge positions
                float leftEdge = edgeThreshold * Screen.width;
                float rightEdge = (1 - edgeThreshold) * Screen.width;
                float topEdge = edgeThreshold * Screen.height;
                float bottomEdge = (1 - edgeThreshold) * Screen.height;

                // Check if the mouse is near the edge of the screen
                if (normalizedMousePos.x < edgeThreshold)
                {
                    // Move left
                    MoveCamera(Vector3.left);
                }
                else if (normalizedMousePos.x > 1 - edgeThreshold)
                {
                    // Move right
                    MoveCamera(Vector3.right);
                }

                if (normalizedMousePos.y < edgeThreshold)
                {
                    // Move down
                    MoveCamera(Vector3.down);
                }
                else if (normalizedMousePos.y > 1 - edgeThreshold)
                {
                    // Move up
                    MoveCamera(Vector3.up);
                }
            }  
        }
    }

    private void MoveCamera(Vector3 direction)
    {
        // Adjust direction based on camera type
        if (mapCamera.orthographic)
        {
            // Orthographic camera: move in X and Y axes
            direction = new Vector3(direction.x, 0, direction.y);
        }
        else
        {
            // Perspective camera: move in X and Y axes
            direction = new Vector3(direction.x, 0, direction.y);
        }

        if (direction.x < minX + 5 || direction.x > maxX - 5 && direction.y < minY + 5 && direction.y > maxY - 5)
        {
            MapCamMoveSpeed = 0f;
        }
        else
        {
            MapCamMoveSpeed = 20f;
        }

        // Move the camera in the specified direction
        mapCamera.transform.position += direction * MapCamMoveSpeed * Time.deltaTime;
    }

    private void OnPause(InputAction.CallbackContext context)
    {
        if (context.ReadValue<float>() > 0)
        {
            isPaused = !isPaused;
            //Time.timeScale = isPaused ? 0 : 1;

            mapCamera.transform.position = originalCameraPosition;

            bool check1 = isPaused ? true : false;
            pauseMenu.SetActive(check1);
            Cursor.visible = check1;

            CursorLockMode lockMode = isPaused ? CursorLockMode.Confined : CursorLockMode.Locked;
            Cursor.lockState = lockMode;
        }
    }

    private void OnScroll(InputAction.CallbackContext context)
    {
        if (isPaused == true)
        {
            // Read the scroll value
            Vector2 scrollInput = context.ReadValue<Vector2>();

            // Adjust the orthographic size based on the scroll input
            mapCamera.orthographicSize -= scrollInput.y * scrollSpeed;
            mapCamera.orthographicSize = Mathf.Clamp(mapCamera.orthographicSize, minOrthoSize, maxOrthoSize);
        }
    }

    private void OnMouseClickStarted(InputAction.CallbackContext context)
    {
        // Mouse button was pressed
        Debug.Log("Left mouse button pressed.");
    }

    private void OnMouseClickCanceled(InputAction.CallbackContext context)
    {
        // Mouse button was released
        Debug.Log("Left mouse button released.");
    }

    private void OnDrawGizmos()
    {
        if (mapCamera == null) return;

        // Get the camera's viewport dimensions
        Vector3 topLeft = mapCamera.ViewportToWorldPoint(new Vector3(0, 1, mapCamera.nearClipPlane));
        Vector3 topRight = mapCamera.ViewportToWorldPoint(new Vector3(1, 1, mapCamera.nearClipPlane));
        Vector3 bottomLeft = mapCamera.ViewportToWorldPoint(new Vector3(0, 0, mapCamera.nearClipPlane));
        Vector3 bottomRight = mapCamera.ViewportToWorldPoint(new Vector3(1, 0, mapCamera.nearClipPlane));

        // Calculate the edge rectangle dimensions
        float edgeWidth = Mathf.Abs(topRight.x - topLeft.x) * edgeThreshold;
        float edgeHeight = Mathf.Abs(topLeft.y - bottomLeft.y) * edgeThreshold;

        // Define the corners of the rectangle
        Vector3 rectangleTopLeft = new Vector3(topLeft.x + edgeWidth, topLeft.y, topLeft.z);
        Vector3 rectangleTopRight = new Vector3(topRight.x - edgeWidth, topRight.y, topRight.z);
        Vector3 rectangleBottomLeft = new Vector3(bottomLeft.x + edgeWidth, bottomLeft.y, bottomLeft.z);
        Vector3 rectangleBottomRight = new Vector3(bottomRight.x - edgeWidth, bottomRight.y, bottomRight.z);

        Gizmos.color = edgeColor;

        // Draw the rectangle
        Gizmos.DrawLine(rectangleTopLeft, rectangleTopRight); // Top edge
        Gizmos.DrawLine(rectangleTopRight, rectangleBottomRight); // Right edge
        Gizmos.DrawLine(rectangleBottomRight, rectangleBottomLeft); // Bottom edge
        Gizmos.DrawLine(rectangleBottomLeft, rectangleTopLeft); // Left edge
    }
}
