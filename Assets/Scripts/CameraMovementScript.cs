using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class CameraMovementScript : MonoBehaviour
{
    public Animator animator;
    public GameObject settingsMenu;
    public GameObject credits;
    public LogicScript logic;
    public AudioSource creditsMusic;

    void Update()
    {
        if (animator.GetBool("inCredits") && Input.GetKeyDown(KeyCode.Escape))
        {
            logic.RestartGame();
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
        logic.menuMusic.Stop();
        creditsMusic.Play();
    }
    public void StopCredits()
    {
        logic.RestartGame();
    }
}
