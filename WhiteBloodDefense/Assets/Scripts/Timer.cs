using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour
{
    private float timer;
    public Font font;
    GUIStyle style = new GUIStyle();
    

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
    }

    void OnGUI()
    {
        style.fontSize = 60;
        style.font = font;
        int minutes = Mathf.FloorToInt(timer / 60F);
        int seconds = Mathf.FloorToInt(timer - minutes * 60);
        string niceTime = string.Format("{0:0}:{1:00}", minutes, seconds);

        GUI.Label(new Rect(580, 10, 250, 100), niceTime, style);
    }
}
