using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class SettingsMenu : MonoBehaviour
{
    public RectTransform content;
    public GameObject settingsMenu;
    float startY;

    float minY; // Minimum Y position
    [Header("Bottom Limit - Increase if bigger menu")]
    public float maxY; // Maximum Y position

    [Header("Audio")]
    public AudioMixer audioMixer;
    public Slider masterAudioSlider;
    public Slider musicAudioSlider;
    public Slider soundFxAudioSlider;
    public Slider ambientNoiseAudioSlider;

    private void Start()
    {
        settingsMenu.SetActive(false);

        minY = startY;
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


        //VOLUME

        //Master Volume
        ChangeVolume(audioMixer, "masterVol", masterAudioSlider);

        //Music Volume
        ChangeVolume(audioMixer, "musicVol", musicAudioSlider);

        //SoundFx Volume
        ChangeVolume(audioMixer, "soundFxVol", soundFxAudioSlider);

        //Ambient Noise Volume
        ChangeVolume(audioMixer, "ambNoiseVol", ambientNoiseAudioSlider);
    }

    public void ActivateSettingsMenu()
    {
        Vector2 newPos = content.anchoredPosition;
        newPos.y = startY;
        content.anchoredPosition = newPos;
        settingsMenu.SetActive(true);
    }

    public void DeactivateSettingsMenu()
    {
        settingsMenu.SetActive(false);
    }

    public void ChangeVolume(AudioMixer audioMixer, string group, Slider slider)
    {
        audioMixer.SetFloat(group, slider.value);
    }
}
