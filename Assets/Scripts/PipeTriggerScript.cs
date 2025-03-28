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
            }
        }
    }
}
