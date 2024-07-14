using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public GameObject LoadingScreen;

    private void Start()
    {
        LoadingScreen.SetActive(false);
    }

    public void LoadScene(string sceneName)
    {
        //Put Loading Screen
        LoadingScreen.SetActive(true);

        // Unload the current scene and load the new scene
        SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
        SceneManager.LoadSceneAsync(sceneName);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}