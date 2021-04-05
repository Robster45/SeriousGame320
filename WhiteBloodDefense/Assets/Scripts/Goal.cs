using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    //health of the goal
    public int health;
    public GameObject goal;
    public int waveCount;
    public bool hasEnded;

    // Start is called before the first frame update
    void Start()
    {
        health = 50;

        hasEnded = false;
        //waveCount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(health <= 0 && !hasEnded)
        {
            GameLose();
            hasEnded = true;
        }
        else if (waveCount >= 10 && !hasEnded)
        {
            GameWin();
            hasEnded = true;
        }
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        health--;
    }

    void GameWin()
    {
        gameObject.GetComponent<SpriteRenderer>().color = Color.green;

        // put scene changing here?
    }

    void GameLose()
    {
        gameObject.GetComponent<SpriteRenderer>().color = Color.red;

        // put scene changing here?
    }
}
