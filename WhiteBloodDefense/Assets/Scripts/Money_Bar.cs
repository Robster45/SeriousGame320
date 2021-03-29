using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Money_Bar : MonoBehaviour
{
    public Slider slider;
    //getting the money from the economy script
    public GameObject money;
    private EconomyManager economyManager;
    //Getting wave number
    public GameObject wave;
    private EntityManager entityManager;
    private int waveDisplay;
    public Font font;
    GUIStyle style = new GUIStyle();

    public Button defaultCell;
    public Button specialCell;
    public Button specialCell2;
    public Button specialCell3;

    public Text WaveLabel;
    //disabling the buttons with the money

    void Start()
    {
        economyManager = money.GetComponent<EconomyManager>();
        entityManager = wave.GetComponent<EntityManager>();

    }
    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        //setting it
        slider.value = economyManager.money;
        waveDisplay = entityManager.wave;
        WaveLabel.text = waveDisplay.ToString();

        if(economyManager.money <2)
        {
            defaultCell.interactable = false;
        }
        else
        {
            defaultCell.interactable = true;
        }

        if (economyManager.money < 3)
        {
           specialCell.interactable = false;
           specialCell2.interactable = false;
           specialCell3.interactable = false;
        }
        else
        {
            specialCell.interactable = true;
            specialCell2.interactable = true;
            specialCell3.interactable = true;
        }
    }

    void OnGUI()
    {    
        WaveLabel.font = font;
        WaveLabel.fontSize = 80;
    }
}
