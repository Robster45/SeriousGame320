using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCell : Cell
{
    public GameObject goal;
    public GameObject playerCell;

    public bool isFleeing;
    public float checkCellTimer;

    public EnemyType type;
    public int strength;

    // Start is called before the first frame update
    void Start()
    {
        checkCellTimer = .5f;

        if (managerObj == null)
        {
            managerObj = GameObject.Find("Manager");
            emScript = managerObj.GetComponent<EntityManager>();
            ecoScript = managerObj.GetComponent<EconomyManager>();
        }
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

        // if the cell gets too close to the outside the map, it 
        // will start to seek a point closer to the center
        // X Position on left
        if (transform.position.x < -7.5f)
        {
            ultimate += Seek(new Vector3(transform.position.x + 1.0f, transform.position.y, 0)) * 5.0f;
        }
        else if (transform.position.x < -8.5f)
        {
            ultimate += Seek(new Vector3(transform.position.x + 1.0f, transform.position.y, 0)) * 10.0f;
        }
        else if (transform.position.x <= -9.5f)
        {
            ultimate += Seek(new Vector3(transform.position.x + 1.0f, transform.position.y, 0)) * 20.0f;
        }

        // Y Position on the bottom
        if (transform.position.y < -3.5f)
        {
            ultimate += Seek(new Vector3(transform.position.x, transform.position.y + 1.0f, 0)) * 5.0f;
        }
        else if (transform.position.y < -4.5f)
        {
            ultimate += Seek(new Vector3(transform.position.x, transform.position.y + 1.0f, 0)) * 10.0f;
        }
        else if (transform.position.y <= -5.0f)
        {
            ultimate += Seek(new Vector3(transform.position.x, transform.position.y + 1.0f, 0)) * 20.0f;
        }

        // Y Position on top
        if (transform.position.y > 5.0f)
        {
            ultimate += Seek(new Vector3(transform.position.x, transform.position.y - 1.0f, 0)) * 5.0f;
        }
        else if (transform.position.y > 4.5f)
        {
            ultimate += Seek(new Vector3(transform.position.x, transform.position.y - 1.0f, 0)) * 10.0f;
        }
        else if (transform.position.y >= 3.5f)
        {
            ultimate += Seek(new Vector3(transform.position.x, transform.position.y - 1.0f, 0)) * 20.0f;
        }

        // gets the flee force if there is
        // a player cell to flee from
        if (playerCell != null)
        {
            // checks the distance from their player cell
            float dist = Vector3.SqrMagnitude(this.transform.position - playerCell.transform.position);
            float scale = 0.0f;

            // scales the flee force based on the distance
            if (dist < 1)
            {
                scale = 20.0f;
            }
            else if (dist < 4)
            {
                scale = 10.0f;
            }
            else if (dist < 9)
            {
                scale = 5.0f;
            }

            // calls the flee force
            ultimate += Flee(playerCell) * scale;
        }

        // finds the seek force
        ultimate += Seek(goal) * 5.0f;

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
        if (emScript.playerCells.Count == 0)
        {
            return null;
        }

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
