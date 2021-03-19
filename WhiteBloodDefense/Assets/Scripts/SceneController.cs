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
            //adding on click functionality
            Button startbtn = startButton.GetComponent<Button>();
            startbtn.onClick.AddListener(changeToGameScene);
            
            //adding on click functionality
            Button exitbtn = startButton.GetComponent<Button>();
            exitbtn.onClick.AddListener(exitScene);
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
            //adding on click functionality
            Button startbtn = startButton.GetComponent<Button>();
            startbtn.onClick.AddListener(changeToGameScene);

            //adding on click functionality
            Button exitbtn = startButton.GetComponent<Button>();
            exitbtn.onClick.AddListener(exitScene);
        }
    }

    //SCENES: 
    // 0 - Start Menu
    // 1 - Main Menu
    // 2 - Game
    // 3 - Pause Menu
    public void changeToGameScene()
    {
        SceneManager.LoadScene(2);
    }

    public void exitScene()
    {
        Application.Quit();
    }
}
