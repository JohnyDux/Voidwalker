using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    public GameObject hud;

    public Texture2D mouseCursor;

    [Header("PAUSE")]
    public bool isPaused;
    public GameObject pauseMenu;
    public GameObject selector;
    public List<GameObject> optionPos;
    public int currentOptionIndex = 0;
    [SerializeField] GameObject loadingScreen;

    [Header("MINIMAP")]
    public GameObject mapMenu;
    bool mapActive;
    

    [Header("/Player Pos")]
    public Transform playerPos;
    public int currentPlayerFloor;
    public GameObject PlayerLocationSphere;
    public float xPos;
    public float yPos;
    public float zPos;

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

    [Header("/Scrolling Manager")]
    public float scrollSpeed = 1f; // Adjust this value to control the scroll sensitivity
    public float minOrthoSize = 1f; // Minimum orthographic size
    public float maxOrthoSize = 20f; // Maximum orthographic size

    [Header("/Floor Manager")]
    public int currentMinimapFloor = 0;
    public List<GameObject> FloorMaps;
    public List<GameObject> ActiveFloorImages;
    public List<GameObject> InactiveFloorImages;

    [Header("/Destination Marker")]
    public GameObject prefabToInstantiate;
    public float markerScale;
    public List<GameObject> markerList;
    public float raycastDistance;
    public Vector3 rayOrigin;
    public Vector3 rayDirection;

    private void Awake()
    {
        isPaused = false;
        mapActive = false;
        pauseMenu.SetActive(false);
        mapMenu.SetActive(false);
        hud.SetActive(true);

        Cursor.SetCursor(mouseCursor, Vector2.zero, CursorMode.Auto);

        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;
        originalCameraPosition = mapCamera.transform.position;
        ShowFloor(currentMinimapFloor);
    }

    private void Update()
    {
        PlayerLocationSphere.transform.position = new Vector3(playerPos.position.x, yPos, playerPos.position.z);

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OnPause();
        }

        if (Input.GetKeyDown(KeyCode.M))
        {
            OnMap();
        }

        if (mapActive == true)
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

            //Set Marker
            if (Input.GetKeyDown(KeyCode.Return))
            {
                SetMarker();
            }
        }

        if(isPaused == true)
        {
            //Change between options
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                optionPos[currentOptionIndex].SetActive(false);

                currentOptionIndex = (currentOptionIndex + 1) % optionPos.Count;

                optionPos[currentOptionIndex].SetActive(true);
            }

            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                optionPos[currentOptionIndex].SetActive(false);
               
                currentOptionIndex = (currentOptionIndex - 1 + optionPos.Count) % optionPos.Count;

                optionPos[currentOptionIndex].SetActive(true);
            }

            //Select options
            if (Input.GetKeyDown(KeyCode.Return)){
                //Resume - index 0
                if (currentOptionIndex == 0)
                {
                    OnPause();
                }
                //Exit to Main Menu - index 1
                else if(currentOptionIndex == 1)
                {
                    loadScene("Menu");
                }
                //Quit - index 2
                else if(currentOptionIndex == 2)
                {
                    Application.Quit();
                }
            }
        }

    }

    private void MoveCamera(Vector3 direction)
    {
        if (mapActive == true)
        {
            mapCamera.transform.Translate(camMoveSpeed * direction);  
        }
    }

    private void OnPause()
    {
        isPaused = !isPaused;
        Time.timeScale = isPaused ? 0 : 1;

        bool check1 = isPaused ? true : false;
        pauseMenu.SetActive(check1);
        mapMenu.SetActive(check1);
        hud.SetActive(!check1);

        Cursor.visible = false;
    }

    private void OnMap()
    {
        mapActive = !mapActive;
        Time.timeScale = isPaused ? 0 : 1;

        mapCamera.transform.position = originalCameraPosition;

        bool check1 = mapActive ? true : false;
        mapMenu.SetActive(check1);
        hud.SetActive(!check1);
        Cursor.visible = check1;

        CursorLockMode lockMode = mapActive ? CursorLockMode.Confined : CursorLockMode.Locked;
        Cursor.lockState = lockMode;
    }

    private void OnScroll()
    {
        if (isPaused == true)
        {
            // Read the scroll value
            Vector2 scrollInput = Input.mouseScrollDelta;

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

    private void SetMarker()
    {
        Ray ray = new Ray(rayOrigin, rayDirection);


        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, raycastDistance))
        {
            if (hit.collider.CompareTag("MapTrigger"))
            {
                //Delete previous marker
                foreach(GameObject go in markerList)
                {
                    if (go != null)
                    {
                        Destroy(go);
                    }
                }

                markerList.Clear();

                Debug.Log("Hit trigger. Pos x:" + hit.transform.position.x + ", Pos y:" + (hit.transform.position.z));

                Vector3 spawnPosition = new Vector3(hit.transform.position.x, -170f, hit.transform.position.z-20);

                // Instantiate the prefab at the specified position and rotation.
                GameObject instantiatedObject = Instantiate(prefabToInstantiate, spawnPosition, Quaternion.identity);
                instantiatedObject.transform.localScale = new Vector3(markerScale, markerScale, markerScale); // Set the scale

                //Optional:  Rename the instantiated object (useful for debugging or organization)
                instantiatedObject.name = "Instantiated_" + prefabToInstantiate.name;

                markerList.Add(instantiatedObject);
            }
        }
        else
        {
            Debug.Log("No object hit");
        }
    }

    void loadScene(string newScene)
    {
        string currentScene = SceneManager.GetActiveScene().name;

        loadingScreen.SetActive(true);

        SceneManager.LoadSceneAsync(newScene);
        SceneManager.UnloadSceneAsync(currentScene);
    }

    private void OnDrawGizmos()
    {
        Ray ray = new Ray(rayOrigin, rayDirection);

        Debug.DrawRay(ray.origin, ray.origin + ray.direction * raycastDistance, Color.red);

        if (mapCamera == null) return;

        // Get the camera's viewport dimensions
        Vector3 topLeft = mapCamera.ViewportToWorldPoint(new Vector3(0, 1, mapCamera.nearClipPlane));
        Vector3 topRight = mapCamera.ViewportToWorldPoint(new Vector3(1, 1, mapCamera.nearClipPlane));
        Vector3 bottomLeft = mapCamera.ViewportToWorldPoint(new Vector3(0, 0, mapCamera.nearClipPlane));
        Vector3 bottomRight = mapCamera.ViewportToWorldPoint(new Vector3(1, 0, mapCamera.nearClipPlane));

        // Calculate the edge rectangle dimensions
        float edgeWidth = Mathf.Abs(topRight.x - topLeft.x) * edgeThreshold;

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
