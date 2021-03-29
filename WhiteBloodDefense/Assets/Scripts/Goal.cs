using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    //health of the goal
    public int health;
    public GameObject goal;
    public bool winGame;
    public EntityManager EntityManager;
    public int cellsLeft;

    // Start is called before the first frame update
    void Start()
    {
        health = 50;
        winGame = false;
    }

    // Update is called once per frame
    void Update()
    {
        cellsLeft = EntityManager.enemies.Count;

        if(health <= 0)
        {
            GameLose();
        }
        if (cellsLeft <= 0)
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

    }

    void GameLose()
    {

    }
}
