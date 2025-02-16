using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class SceneLoader : MonoBehaviour
{
    [Header("Insert name of scene to be loaded")]
    public string sceneName; // The name of the scene to load
    public GameObject LoadingScreen;


    public Image image;
    public Sprite[] LoadingImages;
    private int currentSpriteIndex = 0;
    private float timer = 0f;


    public TextMeshProUGUI progressText;
    public float loadingDelay;

    private void Start()
    {
        LoadingScreen.SetActive(false);
    }

    private void Update()
    {
        timer += Time.deltaTime;

        if(image != null)
        {
            image.sprite = LoadingImages[currentSpriteIndex];

            if (timer >= 5f)
            {
                timer = 0f;
                currentSpriteIndex = (currentSpriteIndex + 1) % LoadingImages.Length;
            }
        }
    }

    // Start loading the scene asynchronously
    public void LoadNewScene()
    {
        StartCoroutine(LoadAndUnloadScene());
    }

    public void QuitGame()
    {
        //Stop in both the Editor and in the Build
        EditorApplication.isPlaying = false;
        Application.Quit();
    }

    private IEnumerator LoadAndUnloadScene()
    {
        // Start loading the scene asynchronously
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);

        // Ensure the scene doesn't activate immediately after loading
        asyncOperation.allowSceneActivation = false;

        // While the scene is still loading, update the progress UI
        while (!asyncOperation.isDone)
        {
            LoadingScreen.SetActive(true);

            // Calculate the loading progress (asyncOperation.progress goes from 0 to 0.9)
            float progress = Mathf.Clamp01(asyncOperation.progress / 0.9f);

            if(asyncOperation.allowSceneActivation == false)
            {
                //change images of loading screen

            }

            // Update the progress text UI
            if (progressText != null)
            {
                progressText.text = (progress * 100).ToString() + "%";
            }

            // Check if the load has finished
            if (asyncOperation.progress >= 0.9f)
            {
                // Wait for an additional 4 seconds delay
                yield return new WaitForSeconds(loadingDelay);

                // Optionally, allow the scene to activate when loading is done
                asyncOperation.allowSceneActivation = true;
            }

            yield return null;
        }

        // Unload the current scene
        Scene currentScene = SceneManager.GetActiveScene();
        AsyncOperation asyncUnload = SceneManager.UnloadSceneAsync(currentScene);

        // Wait until the current scene is completely unloaded
        while (!asyncUnload.isDone)
        {
            yield return null;
        }

        // Optionally, you can reset the progress text UI after the transition is complete
        if (progressText != null)
        {
            progressText.text = "Load Complete";
        }
    }
}