using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityManager : MonoBehaviour
{
    // lists of cells to keep track of
    public List<EnemyCell> enemies;
    public List<PlayerCell> playerCells;
    public PlayerCell selectedCell;
    public List<GameObject> playerCellPrefabs;
    public List<GameObject> enemyCellPrefabs;
    public Goal goal;
    public int wave;
    public float waitTimer;
    public bool activeWave;

    // Start is called before the first frame update
    void Start()
    {
        //for later
        //instantiate lists
        //enemies = new List<EnemyCell>();
        //playerCells = new List<PlayerCell>();
        //clear the list just in case
        //playerCells.Clear();

        wave = 0; //for testing
        waitTimer = 60;
        activeWave = false;
    }


    // Update is called once per frame
    void Update()
    {
        //begin wave im not sure how to yet

        if(enemies.Count == 0)
        {
            EndWave();
        }
        if (waitTimer > 0 && activeWave == false)
        {
            waitTimer -= Time.deltaTime;
        }
        if ((waitTimer <= 0 && activeWave == false) || Input.GetKeyDown(KeyCode.Return))
        {
            BeginWave();
        }

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

        // loop that checks collision
        for(int i = 0; i < enemies.Count; i++)
        {
            // checks if the playercell of that enemy exists
            if(enemies[i].playerCell != null && !enemies[i].isStopped)
            {
                CheckCollide(enemies[i], enemies[i].playerCell.GetComponent<PlayerCell>());
            }
        }
    }


    /// <summary>
    /// Creates a new player cell
    /// </summary>
    public void NewPlayer(int cellType)
    {
        Vector3 tempPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        tempPos.z = 0;
        GameObject tempP = GameObject.Instantiate(playerCellPrefabs[cellType], tempPos, Quaternion.identity);
        PlayerCell script = tempP.GetComponent<PlayerCell>();
        script.aiMode = true;
        script.position = tempPos;
        //tempP.gameObject.transform.localScale = new Vector3((7/100), (7/100)); //it didnt like floats
        playerCells.Add(script);
    }


    /// <summary>
    /// Method to spawn enemies
    /// </summary>
    public void Spawn()
    {
        //should only spawn equal amount to wave, I want it to spawn more tho in the future
        for(int i = 0; i <= wave; i++)
        {
            GameObject tempE = Instantiate(enemyCellPrefabs[UnityEngine.Random.Range(0, enemyCellPrefabs.Count)], new Vector3(-9, Random.Range(-5, 5), 0), Quaternion.identity);
            EnemyCell enemy = tempE.GetComponent<EnemyCell>();
            enemy.goal = goal.goal;
            enemy.position = enemy.transform.position;
            enemies.Add(enemy);
        }
    }


    /// <summary>
    /// Gets the script of the player obj that
    /// was clicked so that it can be used for other things
    /// </summary>
    /// <returns>Player Cell's script</returns>
    private PlayerCell ClickedPlayer()
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
    private EnemyCell ClickedEnemy()
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
    public void CheckCollide(EnemyCell enemy, PlayerCell player)
    {
        if (Vector3.SqrMagnitude(enemy.transform.position - player.transform.position) <= enemy.GetComponent<CircleCollider2D>().radius / 20)
        {
            Debug.Log(Vector3.SqrMagnitude(enemy.transform.position - player.transform.position));
            enemy.isStopped = true;
            player.isStopped = true;
            player.SetKillTimer(enemy.type);
        }
    }

    public void BeginWave()
    {
        Spawn();
        //playerCells.Clear();
        activeWave = true;
    }

    public void EndWave()
    {
        if (activeWave)
        {
            enemies.Clear();
            wave++;
            waitTimer = 60;
            activeWave = false;
        }
        //end wave code
    }
}
