using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityManager : MonoBehaviour
{
    // lists of cells to keep track of
    public List<EnemyCell> enemies;
    public List<PlayerCell> playerCells;
    public PlayerCell selectedCell;
    public GameObject playerCellPrefab;
    public GameObject enemyCellPrefab;
    public Goal goal;
    public int wave;


    // Start is called before the first frame update
    void Start()
    {
        //for later
        //instantiate lists
        //enemies = new List<EnemyCell>();
        //playerCells = new List<PlayerCell>();
        //clear the list just in case
        //playerCells.Clear();

        wave = 2; //for testing
    }


    // Update is called once per frame
    void Update()
    {
        NewPlayer();
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
            if(enemies[i].playerCell == null)
            {
                CheckCollide(enemies[i], enemies[i].playerCell.GetComponent<PlayerCell>());
            }
        }

        if(enemies.Count == 0)
        {
            EndWave();
        }
    }


    /// <summary>
    /// Creates a new player cell
    /// </summary>
    void NewPlayer()
    {
        if(Input.GetKeyDown(KeyCode.C))
        {
            Vector3 tempPos = Input.mousePosition;
            GameObject tempP = GameObject.Instantiate(playerCellPrefab, tempPos, Quaternion.identity);
            //tempP.gameObject.transform.localScale = new Vector3((7/100), (7/100)); //it didnt like floats
            playerCells.Add(tempP.GetComponent<PlayerCell>());
        }
    }


    /// <summary>
    /// Method to spawn enemies
    /// </summary>
    void Spawn()
    {
        //should only spawn equal amount to wave, I want it to spawn more tho in the future
        for(int i = 0; i <= wave; i++)
        {
            GameObject tempE = GameObject.Instantiate(enemyCellPrefab, new Vector3(Random.Range(-9, 0), Random.Range(-5, 5)), Quaternion.identity);
            enemies.Add(tempE.GetComponent<EnemyCell>());
        }
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


    /// <summary>
    /// Checks for collision between enemies and player cells
    /// </summary>
    /// <param name="enemy">Enemy cell to be checked</param>
    /// <param name="player">Nearest Player Cell</param>
    void CheckCollide(EnemyCell enemy, PlayerCell player)
    {
        if (Vector3.SqrMagnitude(enemy.transform.position - player.transform.position) <= enemy.GetComponent<CircleCollider2D>().radius)
        {
            enemy.isStopped = true;
            player.isStopped = true;
        }
    }

    void BeginWave()
    {
        Spawn();
    }

    void EndWave()
    {


        wave++;
    }



}
