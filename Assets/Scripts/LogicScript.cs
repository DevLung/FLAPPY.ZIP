using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Diagnostics;

public class LogicScript : MonoBehaviour
{
    public int playerScore;
    public int highScore;
    private int overOldHighScore = 0;
    public Text scoreText;
    public Text highScoreText;
    public Text highScoreDescription;
    public GameObject scoreTextObject;
    public GameObject gameOverScreen;
    public CharacterScript characterScript;
    public GameObject character;
    public Animator characterLeftWingAnimator;
    public Animator characterRightWingAnimator;
    public GameObject pipeSpawner;
    public AudioSource scoreSound;
    public AudioSource newHighScoreSound;
    public AudioSource pipeDeathSound;
    public AudioSource deadZoneDeathSound;
    public GameObject startMenu;
    public Text resetHighScoreButtonText;
    public HighScoreResetLogicScript resetHighScoreLogicScript;
    public GameObject uninstallConfirmationButtons;
    public Text uninstallButtonText;

    void Start()
    {
        // load high score
        if (!PlayerPrefs.HasKey("high score"))
        {
            PlayerPrefs.SetInt("high score", 0);
        }
        highScore = PlayerPrefs.GetInt("high score");
        highScoreText.text = highScore.ToString();
    }

    void Update()
    {
        // when in start menu or in no start menu mode
        if (startMenu.activeSelf)
        {
            // set character, pipe Spawner, score text inactive
            if (character.activeSelf || pipeSpawner.activeSelf || scoreTextObject.activeSelf)
            {
                character.SetActive(false);
                pipeSpawner.SetActive(false);
                scoreTextObject.SetActive(false);
            }

            // when starting game
            if (Input.GetKeyDown(KeyCode.Space))
            {
                startMenu.SetActive(false);
                character.SetActive(true);
                scoreTextObject.SetActive(true);
                pipeSpawner.SetActive(true);
            }
        }
    }

    public void AddScore(int toAdd)
    {
        playerScore += toAdd;
        scoreText.text = playerScore.ToString();
        scoreSound.Play();

    }
    public void AddHighScore(int toAdd)
    {
        highScore += toAdd;
        PlayerPrefs.SetInt("high score", highScore);
        highScoreText.text = highScore.ToString();
        overOldHighScore++;
        if (overOldHighScore == 1)
        {
            newHighScoreSound.PlayDelayed(0.3f);
        }
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void GameOver(string reason)
    {
        if (characterScript.birdIsAlive)
        {
            characterScript.birdIsAlive = false;

            if (reason == "pipe")
            {
                pipeDeathSound.Play();
            } else if (reason == "deadzone")
            {
                deadZoneDeathSound.Play();
            }
        }
        characterLeftWingAnimator.SetBool("noWingAnimation", true);
        characterRightWingAnimator.SetBool("noWingAnimation", true);
        gameOverScreen.SetActive(true);
    }

    public void ResetHighscore()
    {
        PlayerPrefs.SetInt("high score", 0);
        highScore = PlayerPrefs.GetInt("high score");
        highScoreText.text = highScore.ToString();
        resetHighScoreButtonText.text = "High Score reset.";
        resetHighScoreLogicScript.UpdateButton();

    }
    public void ResetSettings()
    {
        PlayerPrefs.DeleteAll();
        RestartGame();
    }

    public void UninstallQuestion()
    {
        uninstallConfirmationButtons.SetActive(true);
        uninstallButtonText.text = "ARE YOU SURE?";
    }
    public void UninstallAbort()
    {
        uninstallConfirmationButtons.SetActive(false);
        uninstallButtonText.text = "Uninstall Game";
    }
    public void Uninstall()
    {
        PlayerPrefs.DeleteAll();
        Process.Start("powershell.exe", "-Command timeout /NOBREAK 5 ; rm -Recurse ./*");
        Application.Quit();
    }
}
