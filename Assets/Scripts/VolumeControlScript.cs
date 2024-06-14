using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeControlScript : MonoBehaviour
{
    public AudioMixer audioMixer;
    public Slider masterVolumeSlider;
    public Text masterVolumeText;
    public Slider SFXVolumeSlider;
    public Text SFXVolumeText;
    public Slider musicVolumeSlider;
    public Text musicVolumeText;
    readonly string[] mixerGroups = { "master volume", "SFX volume", "Music volume" };
    const float MaxVolume = 100.0f;
    public float DefaultMasterVolume = 50.0f;

    void Start()
    {
        SetInitialVolumeValues();

        // set volumes to saved values
        masterVolumeSlider.value = PlayerPrefs.GetFloat("master volume");
        ChangeMasterVolume();
        SFXVolumeSlider.value = PlayerPrefs.GetFloat("SFX volume");
        ChangeMusicVolume();
        musicVolumeSlider.value = PlayerPrefs.GetFloat("Music volume");
        ChangeSFXVolume();
    }

    public void SetInitialVolumeValues()
    {
        // if some of the keys don't exist, add them on max volume
        foreach (string item in mixerGroups)
        {
            if (!PlayerPrefs.HasKey(item))
            {
                PlayerPrefs.SetFloat(item, MaxVolume);
                if (item == "master volume")
                {
                    PlayerPrefs.SetFloat(item, DefaultMasterVolume);
                }
            }
        }
    }

    public void LoadVolumeSettings()
    {
        // if some of the keys don't exist, add them on max volume
        foreach (string item in mixerGroups)
        {
            ChangeVolume(item, PlayerPrefs.GetFloat(item));
        }
    }

    public void ChangeMasterVolume()
    {
        ChangeVolume("master volume", masterVolumeSlider.value);
        masterVolumeText.text = (masterVolumeSlider.value * (MaxVolume / DefaultMasterVolume)).ToString();
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

    private void ChangeVolume(string exposedVolumeParam, float volumeValue)
    {
        if (volumeValue <= 0.0001)
        {
            audioMixer.SetFloat(exposedVolumeParam, -80.0f);
        } else
        {
            float gain = Mathf.Log10(volumeValue / MaxVolume) * 20.0f;
            audioMixer.SetFloat(exposedVolumeParam, gain);
        }

        PlayerPrefs.SetFloat(exposedVolumeParam, volumeValue);
    }
}
