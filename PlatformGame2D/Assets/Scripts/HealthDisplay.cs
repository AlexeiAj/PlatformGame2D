using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthDisplay : MonoBehaviour
{
    static private float health;
    private Transform healthBar;

    public static float Health { get => health; set => health = value; }

    void Start(){
        healthBar = GetComponent<Transform>();
    }

    void Update(){
        healthBar.localScale = new Vector3(health/100,1f,1f);
    }
}
