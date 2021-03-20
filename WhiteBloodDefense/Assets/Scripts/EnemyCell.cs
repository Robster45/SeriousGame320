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
        checkCellTimer = 1f;
    }

    // Update is called once per frame
    new void Update()
    {
        base.Update();

        checkCellTimer -= Time.deltaTime;

        if (checkCellTimer <= 0 && emScript.playerCells.Count != 0)
        {
            playerCell = FindNearestPlayerCell();
            checkCellTimer = 1f;
        }
    }

    public override void CalculateForces()
    {
        Vector3 ultimate = Vector3.zero;

        ultimate += Seek(goal) * 2;

        if (playerCell != null)
        { ultimate += Flee(playerCell); }

        ultimate.z = 0;
        ultimate = Vector3.ClampMagnitude(ultimate, maxSpeed);

        acceleration += ultimate;
    }

    public GameObject FindNearestPlayerCell()
    {
        GameObject obj = emScript.playerCells[0].gameObject;
        float cDist = Vector3.Distance(obj.transform.position, this.transform.position);

        int count = emScript.playerCells.Count;
        for (int i = 1; i < count; i++)
        {
            float dist = Vector3.Distance(emScript.playerCells[i].transform.position, this.transform.position);

            if (dist < cDist)
            {
                cDist = dist;
                obj = emScript.playerCells[i].gameObject;
            }
        }

        return obj;
    }
}
