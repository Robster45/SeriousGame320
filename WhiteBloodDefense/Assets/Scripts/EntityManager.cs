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
    public BoxCollider2D goalCollider;
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
        waitTimer = 30;
        activeWave = false;
        goalCollider = goal.gameObject.GetComponent<BoxCollider2D>();
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
                //selectedCell.aiMode = false;
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
                    selectedCell.toPoint = false;
                }
                else
                {
                    // formats the cell to target new point
                    selectedCell.targetPoint = pos;
                    selectedCell.targetPoint.z = 0;
                    selectedCell.targetCell = null;
                    selectedCell.toPoint = true;
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
            if(enemies[i].playerCell != null && !enemies[i].isStopped && enemies[i].playerCell.GetComponent<PlayerCell>().targetCell == enemies[i])
            {
                CheckCollide(enemies[i], enemies[i].playerCell.GetComponent<PlayerCell>());
            }

            if (CheckGoal(enemies[i]))
            {
                goal.health -= 5;
                EnemyCell enemy = enemies[i];
                enemies.Remove(enemy);
                Destroy(enemy.gameObject);
            }
        }
    }


    /// <summary>
    /// Creates a new player cell
    /// </summary>
    public void NewPlayer(int cellType)
    {
        // this is the bottom of the bone
        Vector3 startPos = new Vector3(7.3f, -1.5f, 0.001f);//Camera.main.ScreenToWorldPoint(Input.mousePosition);
        //tempPos.z = 0;
        GameObject tempP = GameObject.Instantiate(playerCellPrefabs[cellType], startPos, Quaternion.identity);
        PlayerCell script = tempP.GetComponent<PlayerCell>();
        //script.aiMode = true;
        script.position = startPos;
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
            GameObject tempE = Instantiate(enemyCellPrefabs[UnityEngine.Random.Range(0, enemyCellPrefabs.Count)], new Vector3(-9f, Random.Range(-5f, 5f), 0.001f), Quaternion.identity);
            EnemyCell enemy = tempE.GetComponent<EnemyCell>();
            enemy.goal = goal.gameObject;
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
            goal.waveCount = wave;
            waitTimer = 60;
            activeWave = false;
        }
        //end wave code
    }

    /// <summary>
    /// returns if the enemy is colliding
    /// with the goal using the 2D colliders
    /// </summary>
    /// <param name="e"></param>
    /// <returns></returns>
    public bool CheckGoal(EnemyCell e)
    {
        CircleCollider2D circle = e.gameObject.GetComponent<CircleCollider2D>();

        if (e.transform.position.x + circle.bounds.extents.x < goal.transform.position.x + goalCollider.bounds.extents.x &&
            e.transform.position.y + circle.bounds.extents.y < goal.transform.position.y + goalCollider.bounds.extents.y &&
            e.transform.position.x - circle.bounds.extents.x > goal.transform.position.x - goalCollider.bounds.extents.x &&
            e.transform.position.y - circle.bounds.extents.y > goal.transform.position.y - goalCollider.bounds.extents.y)
        {
            return true;
        }
        //return Vector3.SqrMagnitude(e.gameObject.transform.position - goal.transform.position) <= circle.radius ? true : false;
        return false;
    }
}
