using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    //health of the goal
    public int health;
    public GameObject goal;

    // Start is called before the first frame update
    void Start()
    {
        health = 50;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        health--;
    }
}
