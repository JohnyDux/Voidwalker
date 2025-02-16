using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class LevelButton : MonoBehaviour
{
    [SerializeField]string SceneToLoad;

    private PlayerInputActions inputActions;
    private bool clickInput;
    private float chooseInput;

    [SerializeField] GameObject levelsBoard;
    [SerializeField] GameObject loadingScreen;
    [SerializeField] int selectedFloorIndex;
    public RectTransform selector;
    bool selectorCanMove;
    public Image selectorImage;

    private void Awake()
    {
        inputActions = new PlayerInputActions();
        loadingScreen.SetActive(false);
        levelsBoard.SetActive(false);
        selectorImage.color = Color.black;
        selectorCanMove = true;
    }

    private void OnEnable()
    {
        inputActions.Player.ChooseOption.performed += ChooseFloor;
        inputActions.Player.SelectOption.performed += SelectOption;
        inputActions.Player.Enable();
        inputActions.Player.Select.performed += OnLoad;
    }

    private void OnDisable()
    {
        inputActions.Player.Select.performed -= OnLoad;
        inputActions.Player.ChooseOption.performed -= ChooseFloor;
        inputActions.Player.SelectOption.performed -= SelectOption;
        inputActions.Player.Disable();
    }

    void OnLoad(InputAction.CallbackContext context)
    {
        clickInput = context.ReadValueAsButton();
    }


    public void ChooseFloor(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if(selectorCanMove == true)
            {
                selectorImage.color = Color.black;

                if (selectedFloorIndex < 2)
                {
                    selectedFloorIndex++;
                }
                else
                {
                    selectedFloorIndex = 0;
                }
                Debug.Log("Selected floor: " + selectedFloorIndex);

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
        }
    }

    public void SelectOption(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if(selectorCanMove == true)
            {
                selectorCanMove = false;
                selectorImage.color = Color.red;
            }
        }
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
            levelsBoard.SetActive(true);

            if (clickInput == true)
            {
                loadScene(SceneToLoad);
            }

            if(chooseInput > 0)
            {
                selectedFloorIndex = (int)chooseInput;
            }
        }
    }
}
