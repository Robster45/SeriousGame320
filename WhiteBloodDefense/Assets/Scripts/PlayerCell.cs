using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCell : Cell
{
    // used for killing enemies
    public float killTimer;
    public float maxKillTimer;
    public int kills;

    // used for moving
    public EnemyCell targetCell;
    public Vector3 targetPoint;
    public bool aiMode;


    // Start is called before the first frame update
    void Start()
    {
        //aiMode = false;
    }

    // Update is called once per frame
    new void Update()
    {
        // runs cell update
        base.Update();

        // checks if the AI is on
        if (aiMode)
        {
            AIBehavior();
        }
    }

    /// <summary>
    /// The method that runs when the cell is in AI mode
    /// </summary>
    public virtual void AIBehavior()
    {
        // checks if it need to find a target
        if (targetCell == null)
        {
            targetCell = FindClosestEnemy();
        }
    }

    /// <summary>
    /// Playercell override of the Calculate force method
    /// </summary>
    public override void CalculateForces()
    {
        // temp vec to hold force
        Vector3 finalForce = Vector3.zero;

        // checks if there is a target cell/position
        if (targetCell != null)
        {
            finalForce += Pursue(targetCell.gameObject);
        }
        else if (targetPoint != null)
        {
            finalForce += Seek(targetPoint);
        }

        // scales the force
        finalForce.z = 0;
        finalForce = Vector3.ClampMagnitude(finalForce, maxSpeed);

        // adds to acceleration
        acceleration += finalForce;
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
            // gets the dist
            float dist = Vector3.SqrMagnitude(this.transform.position - emScript.enemies[i].transform.position);

            // checks if first iteration
            if (i == 0)
            {
                currentDist = dist;
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
}
