﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneGeneration : MonoBehaviour
{
    public GameObject sceneLevel;
    public GameObject enemyLevel;

    void Start()
    {
        Instantiate(sceneLevel, transform.position, Quaternion.identity);
        
        //size of 3 platform 
        float distanceBtwScene = 38.4f;
        Vector3 inicio = transform.position;
        Vector3 inicioBack = new Vector3(transform.position.x-0.027f,transform.position.y,transform.position.z);

        int i;
        for(i = 1; i < 2; i++){
            Instantiate(sceneLevel, new Vector3(inicio.x + (i*distanceBtwScene), inicio.y, inicio.z), Quaternion.identity);
            Instantiate(sceneLevel, new Vector3(inicioBack.x * (-i*distanceBtwScene), inicioBack.y, inicioBack.z), Quaternion.identity);
        }

        Instantiate(enemyLevel, new Vector3(inicio.x + Random.Range(-distanceBtwScene*i,distanceBtwScene*i), inicio.y+30, inicio.z), Quaternion.identity);
    }
}
