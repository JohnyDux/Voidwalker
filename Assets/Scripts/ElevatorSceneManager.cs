using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class ElevatorSceneManager : MonoBehaviour
{
    [Header("Scene Names")]
    public string elevatorScene = "LevelElevator";
    public string level1Scene = "Level1";
    public string level2Scene = "Level2";
    public string level3Scene = "Level3";

    private string currentLoadedLevel = "";
    private bool isLoading = false;

    private void Awake()
    {
        // Ensure this object persists between scene loads
        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        if (isLoading) return;

        // Check for number key presses
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            ToggleLevel(level1Scene);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            ToggleLevel(level2Scene);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            ToggleLevel(level3Scene);
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

    // For UI buttons if needed
    public void ButtonLoadLevel1() => ToggleLevel(level1Scene);
    public void ButtonLoadLevel2() => ToggleLevel(level2Scene);
    public void ButtonLoadLevel3() => ToggleLevel(level3Scene);
}