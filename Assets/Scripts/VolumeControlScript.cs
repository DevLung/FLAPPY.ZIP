using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeControlScript : MonoBehaviour
{
    public AudioMixer mixer;
    public Slider masterVolumeSlider;
    public Text masterVolumeText;
    public Slider SFXVolumeSlider;
    public Text SFXVolumeText;
    public Slider musicVolumeSlider;
    public Text musicVolumeText;
    private const int MaxSliderValue = 100;

    void Start()
    {
        // if some of the keys don't exist, add them on max volume
        string[] mixerGroups = { "master volume", "SFX volume", "Music volume" };
        foreach (string item in mixerGroups)
        {
            if (!PlayerPrefs.HasKey(item))
            {
                PlayerPrefs.SetFloat(item, MaxSliderValue);
            }
        }
        masterVolumeSlider.value = PlayerPrefs.GetFloat("master volume");
        ChangeMasterVolume();
        SFXVolumeSlider.value = PlayerPrefs.GetFloat("SFX volume");
        ChangeSFXVolume();
        musicVolumeSlider.value = PlayerPrefs.GetFloat("Music volume");
        ChangeMusicVolume();
    }

    public void ChangeMasterVolume()
    {
        ChangeVolume("master volume", masterVolumeSlider.value);
        masterVolumeText.text = (masterVolumeSlider.value).ToString();
    }
    public void ChangeSFXVolume()
    {
        ChangeVolume("SFX volume", SFXVolumeSlider.value);
        SFXVolumeText.text = (SFXVolumeSlider.value).ToString();
    }
    public void ChangeMusicVolume()
    {
        ChangeVolume("Music volume", musicVolumeSlider.value);
        musicVolumeText.text = (musicVolumeSlider.value).ToString();
    }

    void ChangeVolume(string group, float sliderValue)
    {
        if (sliderValue <= 0.0001)
        {
            mixer.SetFloat(group, -80.0f);
        } else
        {
            float Gain = Mathf.Log10(sliderValue / MaxSliderValue) * 20.0f;
            mixer.SetFloat(group, Gain);
        }

        PlayerPrefs.SetFloat(group, sliderValue);
    }
}
