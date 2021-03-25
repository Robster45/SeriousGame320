using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EconomyManager : MonoBehaviour
{
    // Start is called before the first frame update
    public List<int> costs;
    public int money;

    public EntityManager emScript;

    void Start()
    {
        emScript = gameObject.GetComponent<EntityManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            BuyCell(0);
        }
    }


    public void BuyCell(int cellType)
    {
        if (costs[cellType] <= money)
        {
            money -= costs[cellType];
            emScript.NewPlayer(cellType);
        }
    }
}
