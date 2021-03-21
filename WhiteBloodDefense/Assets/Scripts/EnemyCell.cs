using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCell : Cell
{
    public GameObject goal;
    public GameObject playerCell;

    public bool isFleeing;
    public float checkCellTimer;

    //Asset for enemy cell
    public Sprite image;

    // Start is called before the first frame update
    void Start()
    {
        checkCellTimer = .5f;
    }

    // Update is called once per frame
    new void Update()
    {
        // runs Cell update
        base.Update();

        // timer to check if there is a cell to flee
        checkCellTimer -= Time.deltaTime;

        // 
        if (checkCellTimer <= 0 && emScript.playerCells.Count != 0)
        {
            playerCell = FindNearestPlayerCell();
            checkCellTimer = .5f;
        }
    }

    public override void CalculateForces()
    {
        // sets defualt to zero
        Vector3 ultimate = Vector3.zero;

        float seekScale = 3f;

        // gets the flee force if there is
        // a player cell to flee from
        if (playerCell != null)
        {
            float dist = Vector3.SqrMagnitude(this.transform.position - playerCell.transform.position);
            float scale = 0.0f;

            if (dist < 1)
            {
                scale = 20.0f;
            }
            else if (dist < 4)
            {
                scale = 15.0f;
            }
            else if (dist < 9)
            {
                scale = 10.0f;
            }
            else if (dist < 16)
            {
                scale = 5.0f;
            }

            ultimate += Flee(playerCell) * scale;
        }
        else
        {
            seekScale = 10.0f;
        }

        // finds the seek force
        ultimate += Seek(goal) * seekScale;

        // makes sure the z value doesn't change
        ultimate.z = 0;

        //ultimate.Normalize();
        // adjusts the scale back down to the max speed
        ultimate = Vector3.ClampMagnitude(ultimate, maxSpeed);

        // adds to acceleration
        acceleration += ultimate;
    }

    /// <summary>
    /// Gets the nearest player cell within the
    /// fleeing distance 
    /// </summary>
    /// <returns></returns>
    public GameObject FindNearestPlayerCell()
    {
        // sets the first element to the temp vars
        GameObject obj = emScript.playerCells[0].gameObject;
        float cDist = Vector3.SqrMagnitude(obj.transform.position - this.transform.position);

        // checks all the others against the first
        int count = emScript.playerCells.Count;
        for (int i = 1; i < count; i++)
        {
            // gets the distance
            float dist = Vector3.SqrMagnitude(emScript.playerCells[i].transform.position - this.transform.position);

            // checks if closer and updates
            if (dist < cDist)
            {
                cDist = dist;
                obj = emScript.playerCells[i].gameObject;
            }
        }

        // checks if within flee range
        if (cDist > 25)
        {
            return null;
        }
        return obj;
    }
}
