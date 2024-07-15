using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

public class UIController : MonoBehaviour
{
    public bool isPaused;
    public GameObject pauseMenu;

    private PlayerInputActions inputActions;
    private Vector2 Input;

    private void Start()
    {
        inputActions = new PlayerInputActions();
        isPaused = false;
        pauseMenu.SetActive(false);

        Cursor.lockState = CursorLockMode.Confined;
    }

    private void OnEnable()
    {
        inputActions.Player.Enable();
        inputActions.Player.Pause.performed += OnPause; // Bind Pause action
        inputActions.Player.Pause.canceled += OnPause;
    }

    private void OnDisable()
    {
        inputActions.Player.Pause.performed -= OnPause; // Unbind Pause action
        inputActions.Player.Pause.canceled -= OnPause;
        inputActions.Player.Disable();
    }

    private void OnPause(InputAction.CallbackContext context)
    {
        Input = context.ReadValue<Vector2>();
        if(isPaused == false)
        {
            Time.timeScale = 0f;
            isPaused = true;
            pauseMenu.SetActive(true);

            Cursor.lockState = CursorLockMode.Confined;
        }
        else if (isPaused == true)
        {
            Time.timeScale = 1f;
            isPaused = false;
            pauseMenu.SetActive(false);

            Cursor.lockState = CursorLockMode.Locked;
        }

        Cursor.visible = isPaused;
    }
}
