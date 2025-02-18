using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
using UnityEngine.UI; 

public class UIController : MonoBehaviour
{
    public GameObject hud;

    [Header("Pause")]
    public bool isPaused;
    public GameObject pauseMenu;

    [Header("Input")]
    private PlayerInputActions inputActions;
    private InputAction scrollAction;

    [Header("Minimap")]
    //Player Pos
    public Transform playerPos;
    public int currentPlayerFloor;
    public GameObject PlayerLocationSphere;
    public float xPos;
    public float yPos;
    public float zPos;

    [Header("MINIMAP")]
    //Mouse Scroll
    [Header("/Camera Constraint")]
    public Vector3 originalCameraPosition;
    public float zOffset = 0f;
    public float minX, maxX; // Minimum and maximum X position for the camera
    public float minY, maxY; // Minimum and maximum Y position for the camera
    public float MapCamMoveSpeed; // Speed factor for movement
    public float edgeThreshold = 0.1f; // Threshold for detecting edge of the screen
    public Color edgeColor = Color.red; // Color for the edge threshold visualization
    public float edgeThickness = 2f; // Thickness of the edge rectangle lines
    public Camera mapCamera;
    [SerializeField] private Vector2 moveInput;
    public float camMoveSpeed;

    public Vector3 targetPosition;

    [Header("/Scrolling Manager")]
    public float scrollSpeed = 1f; // Adjust this value to control the scroll sensitivity
    public float minOrthoSize = 1f; // Minimum orthographic size
    public float maxOrthoSize = 20f; // Maximum orthographic size

    [Header("/Floor Manager")]
    public int currentMinimapFloor = 0;
    public List<GameObject> FloorMaps;
    public List<GameObject> ActiveFloorImages;
    public List<GameObject> InactiveFloorImages;

    private void Awake()
    {
        inputActions = new PlayerInputActions();
        isPaused = false;
        pauseMenu.SetActive(false);
        hud.SetActive(true);

        Cursor.lockState = CursorLockMode.Confined;
        originalCameraPosition = mapCamera.transform.position;
        ShowFloor(currentMinimapFloor);
    }

    private void OnEnable()
    {
        inputActions.Player.Enable();

        //Scroll
        scrollAction = inputActions.FindAction("Scroll");
        scrollAction.Enable();
        scrollAction.performed += OnScroll;
        scrollAction.canceled += OnScroll;
    }

    private void OnDisable()
    {
        // Scroll
        scrollAction.performed -= OnScroll;
        scrollAction.canceled -= OnScroll;
        scrollAction.Disable();

        inputActions.Player.Disable();
    }

    private void Update()
    {
        PlayerLocationSphere.transform.position = new Vector3(playerPos.position.x, yPos, playerPos.position.z);

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OnPause();
        }

        if (isPaused == true)
        {
            //Center on Player
            if (Input.GetKeyDown(KeyCode.Backspace))
            {
                mapCamera.transform.position = new Vector3(PlayerLocationSphere.transform.position.x, originalCameraPosition.y, PlayerLocationSphere.transform.position.z);
                mapCamera.orthographicSize = 130.39f;
                Debug.Log("Center camera");
            }

            //Minimap Movement

            // Check if the mouse is near the edge of the screen
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                // Move left
                moveInput = Vector3.left;
            }
            else if (Input.GetKeyUp(KeyCode.LeftArrow))
            {
                moveInput = Vector3.zero;
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                // Move right
                moveInput = Vector3.right;
            }
            else if (Input.GetKeyUp(KeyCode.RightArrow))
            {
                moveInput = Vector3.zero;
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                // Move down
                moveInput = Vector3.down;
            }
            else if (Input.GetKeyUp(KeyCode.DownArrow))
            {
                moveInput = Vector3.zero;
            }
            else if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                // Move up
                moveInput = Vector3.up;
            }
            else if (Input.GetKeyUp(KeyCode.UpArrow))
            {
                moveInput = Vector3.zero;
            }
            MoveCamera(moveInput);

            //Zoom
            if (Input.GetKeyDown(KeyCode.Minus) || Input.GetKeyDown(KeyCode.KeypadMinus))
            {
                Debug.Log("Minus key pressed");
                mapCamera.orthographicSize += 10;
                mapCamera.orthographicSize = Mathf.Clamp(mapCamera.orthographicSize, minOrthoSize, maxOrthoSize);
            }
            else if (Input.GetKeyDown(KeyCode.Plus) || Input.GetKeyDown(KeyCode.KeypadPlus))
            {
                Debug.Log("Plus key pressed");
                mapCamera.orthographicSize -= 10;
                mapCamera.orthographicSize = Mathf.Clamp(mapCamera.orthographicSize, minOrthoSize, maxOrthoSize);
            }

            //Choosing Floors
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                ShowFloor(0);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                ShowFloor(1);
            }
            else if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                ShowFloor(2);
            }
        }

    }

    private void MoveCamera(Vector3 direction)
    {
        if (isPaused == true)
        {
            mapCamera.transform.Translate(camMoveSpeed * direction);  
        }
    }

    private void OnPause()
    {
        isPaused = !isPaused;
        Time.timeScale = isPaused ? 0 : 1;

        mapCamera.transform.position = originalCameraPosition;

        bool check1 = isPaused ? true : false;
        pauseMenu.SetActive(check1);
        hud.SetActive(!check1);
        Cursor.visible = check1;

        CursorLockMode lockMode = isPaused ? CursorLockMode.Confined : CursorLockMode.Locked;
        Cursor.lockState = lockMode;
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

    private void ShowFloor(int i)
    {
        int max = ActiveFloorImages.Count;
        if (i >= 0 && i <= max)
        {
            for (int j = 0; j < ActiveFloorImages.Count; j++)
            {
                RectTransform rectTransform = ActiveFloorImages[j].GetComponent<RectTransform>();
                if (rectTransform != null)
                {
                    if (j == i)
                    {
                        ActiveFloorImages[j].SetActive(true);
                        FloorMaps[i].SetActive(true);
                        InactiveFloorImages[j].SetActive(false);
                        currentMinimapFloor = i;
                    }
                    else
                    {
                        FloorMaps[j].SetActive(false);
                        InactiveFloorImages[j].SetActive(true);
                        ActiveFloorImages[j].SetActive(false);
                    }
                }
                else
                {
                    Debug.LogError("RectTransform not found on Image at index " + j);
                }
            }
        }

        if(currentMinimapFloor != currentPlayerFloor)
        {
            PlayerLocationSphere.SetActive(false);
        }
        else
        {
            PlayerLocationSphere.SetActive(true);
        }
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
