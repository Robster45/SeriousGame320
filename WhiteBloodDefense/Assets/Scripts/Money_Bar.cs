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
    void Start()
    {
        economyManager = money.GetComponent<EconomyManager>();
    }
    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        //setting it
        slider.value = economyManager.money;
    }
}
