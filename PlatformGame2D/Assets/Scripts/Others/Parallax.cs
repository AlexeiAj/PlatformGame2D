using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    private float lenght, startpos;
    private GameObject cam;
    public float parallaxEffect;

    void Start(){
        startpos = transform.position.x;
        cam = GameObject.FindGameObjectWithTag("MainCamera");
        lenght = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    void FixedUpdate(){
        float temp = cam.transform.position.x * (1 - parallaxEffect);
        float dist = cam.transform.position.x * parallaxEffect;

        transform.position = new Vector3(startpos + dist, 10, transform.position.z);

        if(temp > startpos + lenght) startpos += lenght;
        else if(temp < startpos - lenght) startpos -= lenght;

    }
}