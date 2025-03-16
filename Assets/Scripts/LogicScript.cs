using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.EventSystems;

public class LogicScript : MonoBehaviour
{
    public int playerScore = 0;
    public int highScore;
    private int overOldHighScore = 0;
    public Text scoreText;
    public Text highScoreText;
    public Text highScoreDescription;
    public GameObject scoreTextObject;
    public GameObject gameOverScreen;
    public CharacterScript characterScript;
    public CameraMovementScript cameraMovementScript;
    public GameObject character;
    public Animator characterLeftWingAnimator;
    public Animator characterRightWingAnimator;
    public GameObject pipeSpawner;
    public AudioMixer audioMixer;
    public AudioSource scoreSound;
    public AudioSource newHighScoreSound;
    public AudioSource pipeDeathSound;
    public AudioSource deadZoneDeathSound;
    public AudioSource menuMusic;
    public AudioSource levelMusic;
    public AudioSource creditsMusic;
    public GameObject startMenu;
    public GameObject settingsMenu;
    public Text resetHighScoreButtonText;
    public HighScoreResetLogicScript resetHighScoreLogicScript;
    public GameObject uninstallConfirmationButtons;
    public Text uninstallButtonText;
    public DiscordManagerScript discordManagerScript;
    public VolumeControlScript volumeControlScript;
    const int FadeIn = 1;
    const int FadeOut = 0;

    void Start()
    {
        // reset music volumes
        audioMixer.SetFloat("Menu Music volume", -80.0f);
        audioMixer.SetFloat("Level Music volume", -80.0f);
        audioMixer.SetFloat("Credits Music volume", -80.0f);

        // load high score
        if (!PlayerPrefs.HasKey("high score"))
        {
            PlayerPrefs.SetInt("high score", 0);
        }
        highScore = PlayerPrefs.GetInt("high score");
        highScoreText.text = highScore.ToString();

        // load sound settings
        volumeControlScript.LoadVolumeSettings();
        FadeMusicInOrOut("menu", 1.5f, FadeIn);
    }

    void Update()
    {
        if (startMenu.activeSelf)
        {
            // set character, pipe Spawner, score text inactive
            if (character.activeSelf || pipeSpawner.activeSelf || scoreTextObject.activeSelf)
            {
                character.SetActive(false);
                pipeSpawner.SetActive(false);
                scoreTextObject.SetActive(false);
            }

            if (Input.GetKeyDown(KeyCode.Space) || TouchInputExcludingUI(0, TouchPhase.Began)
                && !cameraMovementScript.animator.GetBool("inCredits"))
            {
                StartGame();
            }
        }
    }

    public bool TouchInput(int index, TouchPhase phase)
    {
        if (Input.touchCount > 0)
        {
            return Input.GetTouch(index).phase == phase;
        }
        return false;
    }

    public bool TouchInputExcludingUI(int index, TouchPhase phase)
    {
        if (TouchInput(index, phase))
        {
            return !EventSystem.current.IsPointerOverGameObject(Input.GetTouch(index).fingerId);
        }
        return false;
    }

    private void StartGame()
    {
        if (settingsMenu.activeSelf)
        {
            cameraMovementScript.CloseSettings();
        }
        startMenu.SetActive(false);
        character.SetActive(true);
        scoreTextObject.SetActive(true);
        pipeSpawner.SetActive(true);
        // crossfade music
        FadeMusicInOrOut("menu", 1.5f, FadeOut);
        FadeMusicInOrOut("level", 3.0f, FadeIn);

        discordManagerScript.UpdateActivity(true);
    }

    public void AddScore(int amount)
    {
        playerScore += amount;
        scoreText.text = playerScore.ToString();
        scoreSound.Play();

        discordManagerScript.UpdateActivity(true);
    }
    public void AddHighScore(int amount)
    {
        highScore += amount;
        PlayerPrefs.SetInt("high score", highScore);

        highScoreText.text = highScore.ToString();
        Color darkYellow = new(0.6f, 0.6f, 0);
        highScoreText.color = darkYellow;
        highScoreDescription.color = darkYellow;
        highScoreDescription.text = "New High Score";
        overOldHighScore++;
        // only play new high score sound when high score is initially beaten
        if (overOldHighScore == 1)
        {
            newHighScoreSound.PlayDelayed(0.3f);
        }
    }

    public void RestartGame()
    {
        // reload scene
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
        FadeMusicInOrOut("level", 3.0f, FadeOut);

        // update high score in case it didn't properly update during game
        int savedHighScore = PlayerPrefs.GetInt("high score");
        if (savedHighScore < playerScore)
        {
            AddHighScore(playerScore - savedHighScore);
        }
    }

    private static IEnumerator FadeAudio(AudioMixer audioMixer, string exposedVolumeParam, AudioSource audioSource, float duration, float targetVolume)
    {
        float currentTime = 0;
        audioMixer.GetFloat(exposedVolumeParam, out float currentVol);
        currentVol = Mathf.Pow(10, currentVol / 20);
        float targetValue = Mathf.Clamp(targetVolume, 0.0001f, 1);
        
        // start audio source if fade is a fade in
        if (targetVolume == 1)
        {
            audioSource.Play();
        }

        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            float newVol = Mathf.Lerp(currentVol, targetValue, currentTime / duration);
            audioMixer.SetFloat(exposedVolumeParam, Mathf.Log10(newVol) * 20);
            yield return null;
        }

        // stop audio source if fade is a fade out
        if (targetVolume == 0)
        {
            audioSource.Stop();
        }

        yield break;
    }
    public void FadeMusicInOrOut(string music, float duration, int fadeInOrOut)
    {
        if (music == "menu")
        {
            StartCoroutine(FadeAudio(audioMixer, "Menu Music volume", menuMusic, duration, fadeInOrOut));
        }
        else if (music == "level")
        {
            StartCoroutine(FadeAudio(audioMixer, "Level Music volume", levelMusic, duration, fadeInOrOut));
        }
        else if (music == "credits")
        {
            StartCoroutine(FadeAudio(audioMixer, "Credits Music volume", creditsMusic, duration, fadeInOrOut));
        }
    }

    public void ResetHighscore()
    {
        PlayerPrefs.SetInt("high score", 0);
        highScore = PlayerPrefs.GetInt("high score");
        highScoreText.text = highScore.ToString();
        resetHighScoreButtonText.text = "High Score reset.";
        resetHighScoreLogicScript.UpdateButton();

        discordManagerScript.UpdateActivity(false);

    }
    public void ResetSettings()
    {
        PlayerPrefs.DeleteAll();
        RestartGame();
    }
}
