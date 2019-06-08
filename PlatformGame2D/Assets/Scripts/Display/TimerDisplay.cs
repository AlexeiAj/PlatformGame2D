using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimerDisplay : MonoBehaviour
{
    private float timer;
    private float minutes;
    private float seconds;
    private Text timerText;

    void Start(){
        timerText = GetComponent<Text>();
    }

    void Update(){
        timer += Time.deltaTime;
        minutes = timer / 60;
        seconds = timer % 60;
        if(minutes > 60) timerText.text = "O-o";
        else timerText.text = string.Format("{0:00}-{1:00}", minutes, seconds);
    }
}
