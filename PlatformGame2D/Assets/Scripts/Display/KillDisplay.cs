using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KillDisplay : MonoBehaviour
{
    static private float kill;
    private Text killText;

    public static float Kill { get => kill; set => kill = value; }

    void Start(){
        killText = GetComponent<Text>();
    }

    void Update(){
        killText.text = string.Format("{000}", kill);
    }
}
