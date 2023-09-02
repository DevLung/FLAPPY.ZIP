using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterScript : MonoBehaviour
{
    public LogicScript logic;
    public Rigidbody2D myRigidbody;
    public float flapHeight = 10;
    public bool birdIsAlive = true;
    public float bottomDeadZone = -12;
    public float topDeadZone = 12;
    public AudioSource flapSound;

    void Start()
    {
        logic = GameObject.FindGameObjectWithTag("Logic").GetComponent<LogicScript>();
        // after pressing space in start menu
        flap();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && birdIsAlive)
        {
            flap();
        }

        // Game over if out of bounds
        if (transform.position.y < bottomDeadZone || transform.position.y > topDeadZone)
        {
            logic.GameOver("deadzone");
            if (transform.position.y < bottomDeadZone)
            {
                myRigidbody.constraints = RigidbodyConstraints2D.FreezeAll;
            }
        }
    }

    private void flap()
    {
        myRigidbody.velocity = Vector2.up * flapHeight;
        flapSound.Play();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        logic.GameOver("pipe");
    }
}