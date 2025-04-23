using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEditor;

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
    

    [Header("/Player Pos")]


    [Header("/Camera Constraint")]


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
        isPaused = false;
        pauseMenu.SetActive(false);
        hud.SetActive(true);

        Cursor.SetCursor(mouseCursor, Vector2.zero, CursorMode.Auto);

        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            OnPause();
        }

        if(isPaused == true)
        {
            //Change between options
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                optionPos[currentOptionIndex].SetActive(false);

                if(currentOptionIndex < optionPos.Count - 1)
                {
                    currentOptionIndex = currentOptionIndex + 1;
                }
                else
                {
                    currentOptionIndex = 0;
                }

                optionPos[currentOptionIndex].SetActive(true);
            }

            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                optionPos[currentOptionIndex].SetActive(false);
                if (currentOptionIndex > 0)
                {
                    currentOptionIndex = currentOptionIndex - 1;
                }
                else if (currentOptionIndex < 0)
                {
                    currentOptionIndex = optionPos.Count-1;
                }
                optionPos[currentOptionIndex].SetActive(true);
            }

            Debug.Log(currentOptionIndex);

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
                    EditorApplication.ExitPlaymode();
                }
            }
        }

    }

    private void OnPause()
    {
        isPaused = !isPaused;
        Time.timeScale = isPaused ? 0 : 1;

        bool check1 = isPaused ? true : false;
        pauseMenu.SetActive(check1);
        hud.SetActive(!check1);

        Cursor.visible = false;
    }

    void loadScene(string newScene)
    {
        string currentScene = SceneManager.GetActiveScene().name;

        loadingScreen.SetActive(true);

        SceneManager.LoadSceneAsync(newScene);
        SceneManager.UnloadSceneAsync(currentScene);
    }
}
