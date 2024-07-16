using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

public class UIController : MonoBehaviour
{
    //Pause
    public bool isPaused;
    public GameObject pauseMenu;

    //Input
    private PlayerInputActions inputActions;
    private Vector2 Input;

    //Map-Minimap
    public RectTransform PlayerUIPositionObj; // The UI object to move
    public float movementSpeed;
    public Canvas canvas; // The Canvas
    public TestValues testValues;

    private void Awake()
    {
        inputActions = new PlayerInputActions();
        isPaused = false;
        pauseMenu.SetActive(false);

        Cursor.lockState = CursorLockMode.Confined;
    }

    private void Update()
    {
        //MINIMAP
        if (PlayerUIPositionObj != null && canvas != null)
        {
            //Set UI position
            Vector3 playerPosition = testValues.player.transform.position;
            Vector2 targetPosition = new Vector2(movementSpeed * playerPosition.x, -movementSpeed * playerPosition.z);

            // Set the UI object's position
            PlayerUIPositionObj.anchoredPosition = Vector2.Lerp(PlayerUIPositionObj.anchoredPosition, targetPosition, 2f * Time.deltaTime);
        }
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
