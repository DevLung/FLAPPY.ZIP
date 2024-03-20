using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CameraMovementScript : MonoBehaviour
{
    public Animator animator;
    public GameObject settingsMenu;
    public EventSystem eventSystem;
    public GameObject credits;
    public LogicScript logic;
    public AudioSource creditsMusic;
    const int FadeIn = 1;
    const int FadeOut = 0;

    void Update()
    {
        if (animator.GetBool("inCredits") && Input.GetKeyDown(KeyCode.Escape))
        {
            StopCredits();
        }
    }

    public void OpenSettings()
    {
        // Default -> open settings animation
        animator.SetBool("inSettingsMenu", true);
    }
    public void CloseSettings()
    {
        // open settings animation -> close settings animation -> Default
        animator.SetBool("inSettingsMenu", false);
    }
    public void LoadSettingsMenu()
    {
        settingsMenu.SetActive(true);
    }
    public void UnoadSettingsMenu()
    {
        settingsMenu.SetActive(false);
    }

    public void StartCredits()
    {
        credits.SetActive(true);
        // any open settings animation -> credits animation
        animator.SetBool("inCredits", true);
        // deselect any buttons so credits can't be interrupted by accidental button presses
        eventSystem.SetSelectedGameObject(null);
        // crossfade music
        logic.FadeMusicInOrOut("menu", 1.0f, FadeOut);
        logic.FadeMusicInOrOut("credits", 3.0f, FadeIn);
    }
    public void StopCredits()
    {
        logic.RestartGame();
    }
}
