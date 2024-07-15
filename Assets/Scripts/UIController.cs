using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    public bool isPaused;
    public GameObject pauseMenu;
    
    private void Start()
    {
        isPaused = false;
        pauseMenu.SetActive(false);
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && isPaused == false)
        {
            Time.timeScale = 0f;
            isPaused = true;
            pauseMenu.SetActive(true);

            Cursor.lockState = CursorLockMode.Confined;
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && isPaused == true)
        {
            Time.timeScale = 1f;
            isPaused = false;
            pauseMenu.SetActive(false);

            Cursor.lockState = CursorLockMode.Locked;
        }

        Cursor.visible = isPaused;
    }
}
