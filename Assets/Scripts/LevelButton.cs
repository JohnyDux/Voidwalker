using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class LevelButton : MonoBehaviour
{
    [SerializeField]string SceneToLoad;

    private PlayerInputActions inputActions;
    [SerializeField] private bool clickInput;

    [SerializeField] GameObject loadingScreen;

    private void Awake()
    {
        inputActions = new PlayerInputActions();
        loadingScreen.SetActive(false);
    }

    private void OnEnable()
    {
        inputActions.Player.Enable();
        inputActions.Player.Interact.performed += OnLoad;
    }

    private void OnDisable()
    {
        inputActions.Player.Interact.performed -= OnLoad;
        inputActions.Player.Disable();
    }

    void OnLoad(InputAction.CallbackContext context)
    {
        clickInput = context.ReadValueAsButton();
    }

    void loadScene(string newScene)
    {
        string currentScene = SceneManager.GetActiveScene().name;

        loadingScreen.SetActive(true);

        SceneManager.LoadSceneAsync(newScene);
        SceneManager.UnloadSceneAsync(currentScene);
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (clickInput == true)
            {
                loadScene(SceneToLoad);
            }
        }
    }
}
