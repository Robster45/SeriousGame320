using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    //health of the goal
    public int health;
    public GameObject goal;
    public int waveCount;

    // Start is called before the first frame update
    void Start()
    {
        health = 50;
        waveCount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(health <= 0)
        {
            GameLose();
        }
        if (waveCount >= 10)
        {
            GameWin();
        }
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        health--;
    }

    void GameWin()
    {
        gameObject.GetComponent<SpriteRenderer>().color = Color.green;
    }

    void GameLose()
    {
        gameObject.GetComponent<SpriteRenderer>().color = Color.red;
    }
}
