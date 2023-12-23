using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PipeTriggerScript : MonoBehaviour
{
    public LogicScript logic;
    public CharacterScript characterScript;

    void Start()
    {
        logic = GameObject.FindGameObjectWithTag("Logic").GetComponent<LogicScript>();
        characterScript = GameObject.FindGameObjectWithTag("character").GetComponent<CharacterScript>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 3 && characterScript.birdIsAlive)
        {
            logic.AddScore(1);
            if (logic.playerScore > logic.highScore)
            {
                logic.AddHighScore(logic.playerScore - logic.highScore);
                Color darkYellow = new(0.6f, 0.6f, 0);
                logic.highScoreText.color = darkYellow;
                logic.highScoreDescription.color = darkYellow;
                logic.highScoreDescription.text = "New High Score";
            }
        }
    }
}
