using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class ElevatorSceneManager : MonoBehaviour
{
    bool playerInTrigger;

    [Header("Scene Names")]
    public string elevatorScene = "LevelElevator";
    string[] levelSceneNames = { "Level1", "Level2", "Level3" };

    private string currentLoadedLevel = "";
    private bool isLoading = false;

    [SerializeField] GameObject levelsBoard;
    [SerializeField] GameObject loadingScreen;
    [SerializeField] int selectedFloorIndex;
    public RectTransform selector;
    public Image selectorImage;

    private void Start()
    {
        levelsBoard.SetActive(false);
    }

    private void Update()
    {
        if (isLoading) return;

        if (playerInTrigger)
        {
            levelsBoard.SetActive(true);

            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                ChooseFloor();
            }

            // Check for number key presses
            if (Input.GetKeyDown(KeyCode.Return))
            {
                ToggleLevel(levelSceneNames[selectedFloorIndex]);
            }
        }
        else
        {
            levelsBoard.SetActive(false);
        }
    }

    public void ChooseFloor()
    {
        Debug.Log("Chosse Floor");

         selectorImage.color = Color.black;
         
         if (selectedFloorIndex < 2)
         {
             selectedFloorIndex++;
         }
         else
         {
             selectedFloorIndex = 0;
         }
         
         if (selectedFloorIndex == 0)
         {
             // Change the top and bottom properties
             selector.offsetMin = new Vector2(selector.offsetMin.x, 139); //bottom
             selector.offsetMax = new Vector2(selector.offsetMax.x, -2); //-top
         }
         else if (selectedFloorIndex == 1)
         {
             // Change the top and bottom properties
             selector.offsetMin = new Vector2(selector.offsetMin.x, 75); //bottom
             selector.offsetMax = new Vector2(selector.offsetMax.x, -65); //-top
         }
         else if (selectedFloorIndex == 2)
         {
             // Change the top and bottom properties
             selector.offsetMin = new Vector2(selector.offsetMin.x, 9); //bottom
             selector.offsetMax = new Vector2(selector.offsetMax.x, -131); //-top
         }
    }

    private void ToggleLevel(string level)
    {
        if (currentLoadedLevel == level)
        {
            // If pressing the same number, unload the level
            StartCoroutine(UnloadCurrentLevel());
        }
        else
        {
            // Load new level (will automatically unload previous)
            StartCoroutine(SwitchLevel(level));
        }
    }

    private IEnumerator SwitchLevel(string levelToLoad)
    {
        isLoading = true;

        // First unload current level if one exists
        if (!string.IsNullOrEmpty(currentLoadedLevel))
        {
            yield return StartCoroutine(UnloadCurrentLevel());
        }

        // Load new level additively
        AsyncOperation loadOperation = SceneManager.LoadSceneAsync(levelToLoad, LoadSceneMode.Additive);
        loadOperation.allowSceneActivation = true;

        while (!loadOperation.isDone)
        {
            yield return null;
        }

        currentLoadedLevel = levelToLoad;
        isLoading = false;

        Debug.Log($"Level loaded: {levelToLoad}");
    }

    private IEnumerator UnloadCurrentLevel()
    {
        if (string.IsNullOrEmpty(currentLoadedLevel)) yield break;

        AsyncOperation unloadOperation = SceneManager.UnloadSceneAsync(currentLoadedLevel);

        while (!unloadOperation.isDone)
        {
            yield return null;
        }

        Resources.UnloadUnusedAssets();
        currentLoadedLevel = "";

        Debug.Log("Level unloaded");
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInTrigger = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInTrigger = false;
        }
    }
}