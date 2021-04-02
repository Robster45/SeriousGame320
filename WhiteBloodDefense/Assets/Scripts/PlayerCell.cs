using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCell : Cell
{
    // used for killing enemies
    public float killTimer;
    public int kills;

    // used for moving
    public EnemyCell targetCell;
    public Vector3 targetPoint;
    public bool isWaiting;
    public float waitTimer;

    public CircleCollider2D circleCollider;
    public bool toPoint;

    // type
    public CellType type;

    // Start is called before the first frame update
    void Start()
    {
        if (managerObj == null)
        {
            managerObj = GameObject.Find("Manager");
            emScript = managerObj.GetComponent<EntityManager>();
            ecoScript = managerObj.GetComponent<EconomyManager>();
        }

        circleCollider = gameObject.GetComponent<CircleCollider2D>();
    }

    // Update is called once per frame
    new void Update()
    {
   
        // runs cell update
        base.Update();

        AIBehavior();

        if (kills == 3)
        {
            emScript.playerCells.Remove(this);
            Destroy(this.gameObject);
        }

        if (isStopped)
        {
            killTimer -= Time.deltaTime;

            if (killTimer <= 0)
            {
                kills++;
                EnemyCell temp = targetCell;
                emScript.enemies.Remove(temp);
                Destroy(temp.gameObject); 
                isStopped = false;
                ecoScript.money++;
            }

            if (targetCell == null)
            {
                isStopped = false;
            }
        }
    }

    /// <summary>
    /// The method that runs when the cell is in AI mode
    /// </summary>
    public virtual void AIBehavior()
    {
        if (!isWaiting)
        {
            if (toPoint)
            {
                if (Vector3.SqrMagnitude(transform.position - targetPoint) <= circleCollider.radius / 20)
                {
                    isWaiting = true;
                    waitTimer = 1.5f;
                    velocity = Vector3.zero;
                }
            }

            // checks if it need to find a target
            if (targetCell == null)
            {
                targetCell = FindClosestEnemy();

                if (targetCell == null)
                {
                    toPoint = true;
                }
            }
            else
            {
                if (targetCell.isStopped && !isStopped)
                {
                    targetCell = null;
                }
            }
        }
        else
        {
            waitTimer -= Time.deltaTime;
            if (waitTimer <= 0)
            {
                isWaiting = false;
                targetCell = FindClosestEnemy();
                toPoint = false;
            }
        }
    }

    /// <summary>
    /// Playercell override of the Calculate force method
    /// </summary>
    public override void CalculateForces()
    {
        // temp vec to hold force
        Vector3 finalForce = Vector3.zero;

        finalForce += Seperation();

        // checks if there is a target cell/position
        if (targetCell != null && !toPoint)
        {
            finalForce += Pursue(targetCell.gameObject);
        }
        else if (!isWaiting)
        {
            finalForce += Seek(targetPoint);
        }
        

        // scales the force
        finalForce.z = 0;
        finalForce = Vector3.ClampMagnitude(finalForce, maxSpeed);

        // adds to acceleration
        acceleration += finalForce;
    }

    public override Vector3 Seperation()
    {
        Vector3 sepForce = Vector3.zero;

        foreach (PlayerCell p in emScript.playerCells)
        {
            float dist = Vector3.SqrMagnitude(p.transform.position - transform.position);

            if (dist < .8f)
            {
                sepForce += Flee(p.gameObject) * .3f;
            }
        }

        return sepForce * maxSpeed;
    }

    /// <summary>
    /// Finds the closest enemy to pursue
    /// </summary>
    /// <returns>The enemy cell that's closest</returns>
    public EnemyCell FindClosestEnemy()
    {
        // temp vars
        EnemyCell currentCell = null;
        float currentDist = 0f;

        // loop that goes through all the enemies
        int count = emScript.enemies.Count;
        for (int i = 0; i < count; i++)
        {
            switch (type)
            {
                case CellType.Neutrophil:
                    if (emScript.enemies[i].type != EnemyType.Bacteria && emScript.enemies[i].type != EnemyType.Normal)
                    { continue; }
                    else if (currentCell == null)
                    {
                        currentDist = Vector3.SqrMagnitude(this.transform.position - emScript.enemies[i].transform.position);
                        currentCell = emScript.enemies[i];
                        continue;
                    }
                    break;

                case CellType.Eosinophil:
                    if (emScript.enemies[i].type != EnemyType.Paracyte && emScript.enemies[i].type != EnemyType.Normal)
                    { continue; }
                    else if (currentCell == null)
                    {
                        currentDist = Vector3.SqrMagnitude(this.transform.position - emScript.enemies[i].transform.position);
                        currentCell = emScript.enemies[i];
                        continue;
                    }
                    break;

                case CellType.Basophil:
                    if (emScript.enemies[i].type != EnemyType.Paracyte && emScript.enemies[i].type != EnemyType.Allergen)
                    { continue; }
                    else if (currentCell == null)
                    {
                        currentDist = Vector3.SqrMagnitude(this.transform.position - emScript.enemies[i].transform.position);
                        currentCell = emScript.enemies[i];
                        continue;
                    }
                    break;
            }

            // gets the dist
            float dist = Vector3.SqrMagnitude(this.transform.position - emScript.enemies[i].transform.position);

            if (currentCell == null)
            {
                currentDist = Vector3.SqrMagnitude(this.transform.position - emScript.enemies[i].transform.position);
                currentCell = emScript.enemies[i];
                continue;
            }

            // checks current dist for closer
            if (dist < currentDist)
            {
                currentCell = emScript.enemies[i];
                currentDist = dist;
            }
        }

        // returns closest cell
        return currentCell;
    }

    /// <summary>
    /// Sets the kill timer for the cell when it makes
    /// contact with the enemy cell
    /// </summary>
    /// <param name="enemyType"></param>
    public void SetKillTimer(EnemyType enemyType)
    {
        switch (type)
        {
            case CellType.Neutrophil:
                if (enemyType != EnemyType.Bacteria)
                {
                    killTimer = 2.0f;
                }
                else if (enemyType != EnemyType.Normal)
                {
                    killTimer = 1.5f;
                }
                break;

            case CellType.Eosinophil:
                if (enemyType != EnemyType.Normal)
                {
                    killTimer = .5f;
                }
                else if (enemyType != EnemyType.Paracyte)
                {
                    killTimer = 3.0f;
                }
                break;

            case CellType.Basophil:
                if (enemyType != EnemyType.Paracyte)
                {
                    killTimer = 2.0f;
                }
                else if (enemyType != EnemyType.Allergen)
                {
                    killTimer = 3.0f;
                }
                break;

            default:
                if (enemyType == EnemyType.Normal)
                {
                    killTimer = 2.0f;
                }
                else
                {
                    killTimer = 4.0f;
                }
                break;
        }
    }
}
