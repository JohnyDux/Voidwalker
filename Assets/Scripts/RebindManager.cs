using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class RebindManager : MonoBehaviour
{
    public Button rebindButton;
    public Button resetButton;
    public TMP_InputField keyInputField;
    public string actionName = "Move"; // Name of the action to rebind
    public int bindingIndex = 0; // Index of the binding to update
    public string defaultKeyBind = "w"; // Default key bind value


    private bool waitingForKey;

    private void Start()
    {
        // Initialize the UI with the current binding
        keyInputField.GetComponent<Image>().color = Color.white;

        // Add listeners to buttons
        rebindButton.onClick.AddListener(OnRebindButtonClicked);
        resetButton.onClick.AddListener(OnResetButtonClicked);
    }

    private void Update()
    {
        if (waitingForKey)
        {
            // Check for key presses
            foreach (KeyCode keyCode in System.Enum.GetValues(typeof(KeyCode)))
            {
                if (Input.GetKeyDown(keyCode))
                {
                    // Update the input field with the key pressed
                    keyInputField.text = keyCode.ToString().ToUpper();
                    keyInputField.GetComponent<Image>().color = Color.white;
                    UpdateKeyBinding(keyCode.ToString());
                    waitingForKey = false;
                    break;
                }
            }
        }
    }

    public void OnRebindButtonClicked()
    {
        // Change input field color to red and prompt the user
        keyInputField.text = "Press a key...";
        waitingForKey = true;
    }

    public void OnResetButtonClicked()
    {
        // Reset the input field to the default keybind
        keyInputField.text = defaultKeyBind.ToUpper();
        keyInputField.GetComponent<Image>().color = Color.white;
        waitingForKey = false;
        ResetKeyBinding();
    }

    private void UpdateKeyBinding(string newKey)
    {

    }

    private void ResetKeyBinding()
    {

    }

    private string ExtractKeyFromPath(string path)
    {
        if (string.IsNullOrEmpty(path)) return defaultKeyBind;
        var parts = path.Split('/');
        return parts.Length > 1 ? parts[1] : defaultKeyBind;
    }
}
