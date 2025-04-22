using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using TMPro;

public class SettingsMenu : MonoBehaviour
{
    public Texture2D mouseCursor;

    public RectTransform content;
    public GameObject settingsMenu;
    public SettingsSideBar settingsSideBar;
    float startY;

    float minY; // Minimum Y position
    [Header("Bottom Limit - Increase if bigger menu")]
    public float maxY; // Maximum Y position

    [Header("Audio")]
    public AudioMixer audioMixer;
    public TextMeshProUGUI[] valueText;
    float currentMasterVolume;
    float currentMusicVolume;
    float currentSoundFXVolume;
    float currentAmbientNoiseVolume;

    private void Start()
    {
        settingsMenu.SetActive(false);

        minY = startY;
        currentMasterVolume = 0f;
        currentMusicVolume = 0f;
        currentSoundFXVolume = 0f;
        currentAmbientNoiseVolume = 0f;
        valueText[0].text = currentMasterVolume.ToString();
        valueText[1].text = currentMusicVolume.ToString();
        valueText[2].text = currentSoundFXVolume.ToString();
        valueText[3].text = currentAmbientNoiseVolume.ToString();

        Cursor.SetCursor(mouseCursor, Vector2.zero, CursorMode.Auto);
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;
    }

    private void Update()
    {
        //CONSTRAINT MENU Y POSITION

        // Get the current position of the GameObject
        Vector3 currentPosition = content.anchoredPosition;

        // Clamp the Y position between minY and maxY
        currentPosition.y = Mathf.Clamp(currentPosition.y, minY, maxY);

        // Apply the clamped position back to the GameObject
        content.anchoredPosition = currentPosition;
    }

    public void ActivateSettingsMenu()
    {
        Vector2 newPos = content.anchoredPosition;
        newPos.y = startY;
        content.anchoredPosition = newPos;
        settingsSideBar.GameBtnClick();
        settingsMenu.SetActive(true);
    }

    public void DeactivateSettingsMenu()
    {
        settingsMenu.SetActive(false);
    }

    public void ChangeMasterVolume(float value)
    {
        string group = "masterVol";

        //change volume
        audioMixer.GetFloat(group, out currentMasterVolume);

        currentMasterVolume = currentMasterVolume + value;

        if (currentMasterVolume >= -80 && currentMasterVolume <= 20)
        {
            audioMixer.SetFloat(group, currentMasterVolume);
        }
        
        //change the text
        if((currentMasterVolume + 80) >= 0)
        {
            valueText[0].text = (currentMasterVolume + 80).ToString();
        }
        else
        {
            valueText[0].text = "0";
        }
    }

    public void ChangeMusicVolume(float value)
    {
        string group = "musicVol";

        //change volume
        audioMixer.GetFloat(group, out currentMusicVolume);

        currentMusicVolume = currentMusicVolume + value;

        if (currentMusicVolume >= -80 && currentMusicVolume <= 20)
        {
            audioMixer.SetFloat(group, currentMusicVolume);
        }

        //change the text
        if ((currentMusicVolume + 80) >= 0)
        {
            valueText[1].text = (currentMusicVolume + 80).ToString();
        }
        else
        {
            valueText[1].text = "0";
        }
    }

    public void ChangeSoundFXVolume(float value)
    {
        string group = "soundFxVol";

        //change volume
        audioMixer.GetFloat(group, out currentSoundFXVolume);

        currentSoundFXVolume = currentSoundFXVolume + value;

        if (currentSoundFXVolume >= -80 && currentSoundFXVolume <= 20)
        {
            audioMixer.SetFloat(group, currentSoundFXVolume);
        }

        //change the text
        if ((currentSoundFXVolume + 80) >= 0)
        {
            valueText[2].text = (currentSoundFXVolume + 80).ToString();
        }
        else
        {
            valueText[2].text = "0";
        }
    }

    public void ChangeAmbientNoiseVolume(float value)
    {
        string group = "ambNoiseVol";

        //change volume
        audioMixer.GetFloat(group, out currentAmbientNoiseVolume);

        currentAmbientNoiseVolume = currentAmbientNoiseVolume + value;

        if (currentAmbientNoiseVolume >= -80 && currentAmbientNoiseVolume <= 20)
        {
            audioMixer.SetFloat(group, currentAmbientNoiseVolume);
        }

        //change the text
        if ((currentAmbientNoiseVolume + 80) >= 0)
        {
            valueText[3].text = (currentAmbientNoiseVolume + 80).ToString();
        }
        else
        {
            valueText[3].text = "0";
        }
    }

    public void SetQuality(int count)
    {
        QualitySettings.SetQualityLevel(count);
    }
}
