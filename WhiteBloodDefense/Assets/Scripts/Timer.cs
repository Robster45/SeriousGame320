using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    private float timer;
    public Font font;
    public Text TimerLabel;
    

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        TimerLabel.fontSize = 100;
        TimerLabel.font = font;
        int minutes = Mathf.FloorToInt(timer / 60F);
        int seconds = Mathf.FloorToInt(timer - minutes * 60);
        string niceTime = string.Format("{0:0}:{1:00}", minutes, seconds);
        TimerLabel.text = niceTime;
    }
}
