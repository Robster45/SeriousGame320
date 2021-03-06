using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneController : MonoBehaviour
{
    //making the buttons to chnage the states
    public Button startButton;
    public Button exitButton;

    // Start is called before the first frame update
    void Start()
    { 
    }

    // Update is called once per frame
    void Update()
    {
        //functionality for scene switches

        //If youre on the start menu
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene(1);
            }
        }

        //If youre on the Main menu
        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            //button events
        }

        //If youre in the game
        if (SceneManager.GetActiveScene().buildIndex == 2)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                SceneManager.LoadScene(3);
            }
        }

        //If youre on the pause menu
        if (SceneManager.GetActiveScene().buildIndex == 3)
        {          
            // button
        }

        //If youre on the Tip menu
        if (SceneManager.GetActiveScene().buildIndex == 4)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                changeToGameScene();
            }
        }
    }

    //SCENES: 
    // 0 - Start Menu
    // 1 - Main Menu
    // 2 - Game
    // 3 - Pause Menu
    // 4 - Tip Menu
    public void changeToGameScene()
    {
        SceneManager.LoadScene(2);
    }

    public void exitScene()
    {
        Application.Quit();
    }

    public void toTipScreen()
    {
        SceneManager.LoadScene(4);
    }
}
