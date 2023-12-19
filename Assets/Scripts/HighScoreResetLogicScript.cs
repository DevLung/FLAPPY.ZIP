using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HighScoreResetLogicScript : MonoBehaviour
{
    public LogicScript logic;
    public Button button;
    public Text text;

    void Start()
    {
        UpdateButton();
        text.text = "Reset High Score";
    }

    public void UpdateButton()
    {
        if (logic.highScore == 0)
        {
            button.interactable = false;
        }
        else
        {
            button.interactable = true;
        }
    }
}
