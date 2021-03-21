using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityManager : MonoBehaviour
{
    // lists of cells to keep track of
    public List<EnemyCell> enemies;
    public List<PlayerCell> playerCells;
    public PlayerCell selectedCell;
    //public  screen;

    public int wave;


    // Start is called before the first frame update
    void Start()
    {
        //instantiate lists
        //enemies = new List<EnemyCell>();
        //playerCells = new List<PlayerCell>();
        //clear the list just in case
        //playerCells.Clear();
    }

    // Update is called once per frame
    void Update()
    {
        // for targeting cells
        if (Input.GetMouseButtonDown(0))
        {
            // gets the mouse position
            Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            
            // gets player script if its obj was clicked
            PlayerCell tempPlayer = ClickedPlayer();
            
            // checks if new player cell was clicked
            if (tempPlayer != null)
            {
                // sets to new player cell
                selectedCell = tempPlayer;
                selectedCell.aiMode = false;
            }
            else if (selectedCell != null)
            {
                // gets the enemy script if it clicked on an enemy obj
                EnemyCell tempEnemy = ClickedEnemy();

                // checks if they clicked an enemy
                if (tempEnemy != null)
                {
                    // sets the player cell to target new enemy
                    selectedCell.targetCell = tempEnemy;
                }
                else
                {
                    // formats the cell to target new point
                    selectedCell.targetPoint = pos;
                    selectedCell.targetPoint.z = 0;
                    selectedCell.targetCell = null;
                }
            }
        }

        // resets the cell to null so you can click elsewhere
        if (Input.GetMouseButtonDown(1))
        {
            selectedCell = null;
        }

        for(int i = 0; i < enemies.Count; i++)
        {
            // check the enimies with the playercell
            // the are keeping track of because its more effienct
            // but make sure to check if the playercell is null
            //CheckCollide(enemies[i], playerCells. )
        }
    }

    private void OnMouseDown()
    {
        // you can't do this, you need to use the instantiate method
        // so that you spawn an entire game obj, not just a 
        // new instance of a script
        playerCells.Add(new PlayerCell());
    }

    void Spawn()
    {
        //code to spawn and maybe add into the list
        //assign id number when spawned
    }

    /// <summary>
    /// Gets the script of the player obj that
    /// was clicked so that it can be used for other things
    /// </summary>
    /// <returns>Player Cell's script</returns>
    PlayerCell ClickedPlayer()
    {
        // gets the collider
        Collider2D clicked_collider = Physics2D.OverlapPoint(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        
        // checks if it got a collider
        if (clicked_collider != null)
        {
            // if it got a collider, it returns the cell it got from the click
            return clicked_collider.gameObject.GetComponent<PlayerCell>();
        }
        return null;
    }

    /// <summary>
    /// Click method for the Enemy cells, gets the
    /// script of the obj of an enemy cell that 
    /// was clicked
    /// </summary>
    /// <returns>Enemy Cell's script</returns>
    EnemyCell ClickedEnemy()
    {
        Collider2D clicked_collider = Physics2D.OverlapPoint(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        if (clicked_collider != null)
        {
            return clicked_collider.gameObject.GetComponent<EnemyCell>();
        }
        return null;
    }

    void CheckCollide(EnemyCell enemy, PlayerCell player)
    {
        // I would make this a game obj vrs game obj,
        // that way you can check with circle colision (dist vs a radius)
        // so that we don't need to mess with white space,
        // we also need a way to test for the goal too
        // P.S. I added 2dCircle colliders to all of the enemies
        // and players so that I could click on them so maybe use those??
        if (enemy.image.rect.Overlaps(player.image.rect))
        {
            enemy.isStopped = true;
            player.isStopped = true;

            // Don't worry about this, gonna handle it
            // in each cell file.
            //player.maxKillTimer = 60;
        }
    }
}
