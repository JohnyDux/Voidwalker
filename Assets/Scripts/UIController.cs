using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEditor;
using Cinemachine;

public class UIController : MonoBehaviour
{
    public GameObject hud;

    public Texture2D mouseCursor;

    [Header("PAUSE")]
    public bool isPaused;
    public GameObject pauseMenu;
    public GameObject selector;
    public List<GameObject> optionPos;
    public int currentOptionIndex = 0;
    [SerializeField] GameObject loadingScreen;

    public bool weaponActive;
    public bool weaponAiming;
    public int weaponIndex;
    public List<Sprite> weaponUISprites;
    public Image WeaponIcon;
    public GameObject bulletCountObject;

    public List<Sprite> crosshairUISprites;
    public GameObject CrosshairGO;
    public Image crosshairImage;

    public CinemachineVirtualCamera virtualCamera;

    private void Awake()
    {
        isPaused = false;
        pauseMenu.SetActive(false);
        hud.SetActive(true);

        Cursor.SetCursor(mouseCursor, Vector2.zero, CursorMode.Auto);

        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;

        weaponIndex = 0;
        weaponActive = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && isPaused == false)
        {
            OnPause();
        }

        if(isPaused == true)
        {
            //Change between options
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                optionPos[currentOptionIndex].SetActive(false);

                if(currentOptionIndex < optionPos.Count - 1)
                {
                    currentOptionIndex = currentOptionIndex + 1;
                }
                else
                {
                    currentOptionIndex = 0;
                }

                optionPos[currentOptionIndex].SetActive(true);
            }

            if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                optionPos[currentOptionIndex].SetActive(false);
                if (currentOptionIndex > 0)
                {
                    currentOptionIndex = currentOptionIndex - 1;
                }
                else if (currentOptionIndex < 0)
                {
                    currentOptionIndex = optionPos.Count-1;
                }
                optionPos[currentOptionIndex].SetActive(true);
            }

            Debug.Log(currentOptionIndex);

            //Select options
            if (Input.GetKeyDown(KeyCode.Return)){
                //Resume - index 0
                if (currentOptionIndex == 0)
                {
                    OnPause();
                }
                //Exit to Main Menu - index 1
                else if(currentOptionIndex == 1)
                {
                    loadScene("Menu");
                }
                //Quit - index 2
                else if(currentOptionIndex == 2)
                {
                    Application.Quit();
                    EditorApplication.ExitPlaymode();
                }
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            aimWeapon();
        }
        if (Input.GetMouseButtonUp(1))
        {
            aimWeapon();
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (weaponActive)
            {
                changeWeapon();
            }  
        }

        if (Input.GetKeyDown(KeyCode.Z))
        {
            if (weaponActive == false)
            {
                weaponActive = true;
            }
            else
            {
                weaponActive = false;
            }
        }

        if(weaponActive == false)
        {
            WeaponIcon.GetComponent<Image>().enabled = false;
            bulletCountObject.SetActive(false);
        }
        else
        {
            WeaponIcon.GetComponent<Image>().enabled = true;
            bulletCountObject.SetActive(true);
        }       
    }

    private void OnPause()
    {
        isPaused = !isPaused;
        Time.timeScale = isPaused ? 0 : 1;

        bool check1 = isPaused ? true : false;
        pauseMenu.SetActive(check1);
        hud.SetActive(!check1);

        Cursor.visible = isPaused;
    }
    void loadScene(string newScene)
    {
        string currentScene = SceneManager.GetActiveScene().name;

        loadingScreen.SetActive(true);

        SceneManager.LoadSceneAsync(newScene);
        SceneManager.UnloadSceneAsync(currentScene);
    }

    void changeWeapon()
    {
        weaponActive = true;

        if (weaponIndex < weaponUISprites.Count-1)
        {
            weaponIndex++;
        }
        else
        {
            weaponIndex = 0;
        }

        WeaponIcon.sprite = weaponUISprites[weaponIndex];
        crosshairImage.sprite = crosshairUISprites[weaponIndex];
    }

    void aimWeapon()
    {
        if (weaponActive)
        {
            if (weaponAiming == false)
            {
                weaponAiming = true;
                CrosshairGO.SetActive(true);
            }
            else
            {
                weaponAiming = false;
                CrosshairGO.SetActive(false);
            }
        }
    }
}
