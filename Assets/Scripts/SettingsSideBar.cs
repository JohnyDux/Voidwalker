using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsSideBar : MonoBehaviour
{
    [SerializeField] GameObject gameButtonGraphics;
    [SerializeField] GameObject creditsButtonGraphics;
    [SerializeField] GameObject displayButtonGraphics;
    [SerializeField] GameObject audioButtonGraphics;
    [SerializeField] GameObject keyBindingsButtonGraphics;

    [SerializeField] bool gameMenuActive;
    [SerializeField] bool creditsMenuActive;
    [SerializeField] bool displayMenuActive;
    [SerializeField] bool audioMenuActive;
    [SerializeField] bool keyBindingsMenuActive;

    [SerializeField] GameObject gameMenu;
    [SerializeField] GameObject creditsMenu;
    [SerializeField] GameObject displayMenu;
    [SerializeField] GameObject audioMenu;
    [SerializeField] GameObject keyBindingsMenu;

    private void Start()
    {
        //define Game Button and menu active
        gameButtonGraphics.SetActive(true);
        gameMenuActive = true;
        gameMenu.SetActive(gameMenuActive);
        creditsMenu.SetActive(creditsMenuActive);
        displayMenu.SetActive(displayMenuActive);
        audioMenu.SetActive(audioMenuActive);
        keyBindingsMenu.SetActive(keyBindingsMenuActive);

        //defines the remains to be false
        creditsButtonGraphics.SetActive(false);
        creditsMenuActive = false;
        displayButtonGraphics.SetActive(false);
        displayMenuActive = false;
        audioButtonGraphics.SetActive(false);
        audioMenuActive = false;
        keyBindingsButtonGraphics.SetActive(false);
        keyBindingsMenuActive = false;
    }

    
    public void GameBtnClick()
    {
        gameMenuActive = true;
        gameButtonGraphics.SetActive(gameMenuActive);

        //defines the remains to be false
        creditsButtonGraphics.SetActive(false);
        creditsMenuActive = false;
        displayButtonGraphics.SetActive(false);
        displayMenuActive = false;
        audioButtonGraphics.SetActive(false);
        audioMenuActive = false;
        keyBindingsButtonGraphics.SetActive(false);
        keyBindingsMenuActive = false;

        gameMenu.SetActive(gameMenuActive);
        creditsMenu.SetActive(creditsMenuActive);
        displayMenu.SetActive(displayMenuActive);
        audioMenu.SetActive(audioMenuActive);
        keyBindingsMenu.SetActive(keyBindingsMenuActive);

        
    }

    public void CntrlBtnClick()
    {
        keyBindingsMenuActive = true;
        keyBindingsButtonGraphics.SetActive(keyBindingsMenuActive);


        //defines the remains to be false
        gameButtonGraphics.SetActive(false);
        gameMenuActive = false;
        creditsButtonGraphics.SetActive(false);
        creditsMenuActive = false;
        displayButtonGraphics.SetActive(false);
        displayMenuActive = false;
        audioButtonGraphics.SetActive(false);
        audioMenuActive = false;

        gameMenu.SetActive(gameMenuActive);
        creditsMenu.SetActive(creditsMenuActive);
        displayMenu.SetActive(displayMenuActive);
        audioMenu.SetActive(audioMenuActive);
        keyBindingsMenu.SetActive(keyBindingsMenuActive);
    }

    public void DisplayBtnClick()
    {
        displayMenuActive = true;
        displayButtonGraphics.SetActive(displayMenuActive);
        
        //defines the remains to be false
        gameButtonGraphics.SetActive(false);
        gameMenuActive = false;
        creditsButtonGraphics.SetActive(false);
        creditsMenuActive = false;
        audioButtonGraphics.SetActive(false);
        audioMenuActive = false;
        keyBindingsButtonGraphics.SetActive(false);
        keyBindingsMenuActive = false;

        gameMenu.SetActive(gameMenuActive);
        creditsMenu.SetActive(creditsMenuActive);
        displayMenu.SetActive(displayMenuActive);
        audioMenu.SetActive(audioMenuActive);
        keyBindingsMenu.SetActive(keyBindingsMenuActive);
    }

    public void AudioBtnClick()
    {
        audioMenuActive = true;
        audioButtonGraphics.SetActive(audioMenuActive);

        //defines the remains to be false
        creditsButtonGraphics.SetActive(false);
        creditsMenuActive = false;
        displayButtonGraphics.SetActive(false);
        displayMenuActive = false;
        gameButtonGraphics.SetActive(false);
        gameMenuActive = false;
        keyBindingsButtonGraphics.SetActive(false);
        keyBindingsMenuActive = false;

        
        gameMenu.SetActive(gameMenuActive);
        creditsMenu.SetActive(creditsMenuActive);
        displayMenu.SetActive(displayMenuActive);
        audioMenu.SetActive(audioMenuActive);
        keyBindingsMenu.SetActive(keyBindingsMenuActive);
    }

    public void KeyBindBtnClick()
    {
        keyBindingsMenuActive = true;
        keyBindingsButtonGraphics.SetActive(keyBindingsMenuActive);

        //defines the remains to be false
        creditsButtonGraphics.SetActive(false);
        creditsMenuActive = false;
        displayButtonGraphics.SetActive(false);
        displayMenuActive = false;
        audioButtonGraphics.SetActive(false);
        audioMenuActive = false;
        gameButtonGraphics.SetActive(false);
        gameMenuActive = false;

        gameMenu.SetActive(gameMenuActive);
        creditsMenu.SetActive(creditsMenuActive);
        displayMenu.SetActive(displayMenuActive);
        audioMenu.SetActive(audioMenuActive);
        keyBindingsMenu.SetActive(keyBindingsMenuActive);
    }

    public void CreditsBtnClick()
    {
        creditsMenuActive = true;
        creditsButtonGraphics.SetActive(creditsMenuActive);

        //defines the remains to be false
        keyBindingsButtonGraphics.SetActive(false);
        keyBindingsMenuActive = false;
        displayButtonGraphics.SetActive(false);
        displayMenuActive = false;
        audioButtonGraphics.SetActive(false);
        audioMenuActive = false;
        gameButtonGraphics.SetActive(false);
        gameMenuActive = false;

        gameMenu.SetActive(gameMenuActive);
        creditsMenu.SetActive(creditsMenuActive);
        displayMenu.SetActive(displayMenuActive);
        audioMenu.SetActive(audioMenuActive);
        keyBindingsMenu.SetActive(keyBindingsMenuActive);
    }
}
