using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ElevatorSpawner : MonoBehaviour
{
    [SerializeField]string currentSceneName;


    List<AsyncOperation> scenesToLoad = new List<AsyncOperation>();
    // Start is called before the first frame update
    void Start()
    {
        scenesToLoad.Add(SceneManager.LoadSceneAsync("LevelElevatorScene"));
        scenesToLoad.Add(SceneManager.LoadSceneAsync(currentSceneName, LoadSceneMode.Additive));
    }
}
