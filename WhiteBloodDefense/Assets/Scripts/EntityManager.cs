using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityManager : MonoBehaviour
{
    // lists of cells to keep track of
    public List<EnemyCell> enemies;
    public List<PlayerCell> playerCells;

    public int wave;


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
        for(int i = 0; i < enemies.Count; i++)
        {
            //CheckCollide(enemies[i], playerCells. )
        }
    }



    void Spawn()
    {
        //code to spawn and maybe add into the list
        //assign id number when spawned
    }

    void CheckCollide(EnemyCell enemy, PlayerCell player)
    {
        if (enemy.image.rect.Overlaps(player.image.rect))
        {
            enemy.isStopped = true;
            player.isStopped = true;

            player.maxKillTimer = 60;
        }
    }
}
