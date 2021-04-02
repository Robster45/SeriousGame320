using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SupportCell : Cell
{
    public bool isSupporting;
    public float supportTimer;
    public PlayerCell target;
    public List<PlayerCell> supportees;
    public float tetherRange;
    public bool aiMode;
    public float checktimer;

    // Start is called before the first frame update
    void Start()
    {
        supportTimer = 20f;
    }

    // Update is called once per frame
    new void Update()
    {
        base.Update();

        checktimer -= Time.deltaTime;
        
        if (aiMode)
        {
            if (target == null)
            {
                target = FindNearestPlayer();
            }
        }
    }

    public override void CalculateForces()
    {
        Vector3 ultimate = Vector3.zero;

        if (target != null)
        {
            ultimate += Accompany();
        }

        ultimate.z = 0;
        ultimate = Vector3.ClampMagnitude(ultimate, maxSpeed);

        acceleration += ultimate;
    }

    public Vector3 Accompany()
    {
        float dist = Vector3.SqrMagnitude(this.transform.position - target.transform.position);

        if (dist > (tetherRange * tetherRange) / 2)
        {
            return Seek(target.gameObject);
        }
        
        return Vector3.zero;
    }

    public PlayerCell FindNearestPlayer()
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

        return obj.GetComponent<PlayerCell>();
    }

    public bool CheckSupportee(PlayerCell cell)
    {
        if (Vector3.SqrMagnitude(cell.transform.position - this.transform.position) < tetherRange * tetherRange)
        { 
            return true;
        }
        return false;
    }

    public override Vector3 Seperation()
    {
        throw new System.NotImplementedException();
    }
}
