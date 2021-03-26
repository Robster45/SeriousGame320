using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EconomyManager : MonoBehaviour
{
    // Start is called before the first frame update
    public List<int> costs;
    public int money;
    private float timer;
    public Slider moneyBar;
    public Font font;
    private GUIStyle guiStyle = new GUIStyle();
    public EntityManager emScript;

    void Start()
    {
        emScript = gameObject.GetComponent<EntityManager>();
        money = 5;
        
    }

    // Update is called once per frame
    void Update()
    {
        // Basic Player Cell
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            BuyCell(0);
        }
        
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            BuyCell(1);
        }
        
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            BuyCell(2);
        }
        
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            BuyCell(3);
        }
        moneyBar.value = money;
        timer += Time.deltaTime;
    }


    public void BuyCell(int cellType)
    {
        // checks if the player has the money to buy the cell
        if (costs[cellType] <= money)
        {
            // subtracts the money then calls for a new cell
            money -= costs[cellType];
            emScript.NewPlayer(cellType);
        }
    }

    //Stuff for the timer
    void OnGUI()
    {
        int minutes = Mathf.FloorToInt(timer / 60f);
        int seconds = Mathf.FloorToInt(timer - minutes * 60);
        string niceTime = string.Format("{0:0}:{1:00}", minutes, seconds);
        guiStyle.fontSize = 60;
        guiStyle.font = font;
        GUI.Label(new Rect(555, 10, 100, 10), niceTime.ToString(),guiStyle);
    }
}
