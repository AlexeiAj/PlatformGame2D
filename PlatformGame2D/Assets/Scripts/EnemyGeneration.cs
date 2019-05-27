﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGeneration : MonoBehaviour
{
    Vector3 inicio;
    public GameObject enemyLevel;
    private float distanceBtwScene = 38.4f;
    private int platformSize = 2;

    void Start()
    {
        inicio = transform.position;
        Instantiate(enemyLevel, new Vector3(inicio.x + Random.Range(-distanceBtwScene*platformSize,distanceBtwScene*platformSize), inicio.y+30, inicio.z), Quaternion.identity);
        InvokeRepeating("invokeEnemyRepeat", 5, 5);
    }

    void invokeEnemyRepeat()
    {
        Instantiate(enemyLevel, new Vector3(inicio.x + Random.Range(-distanceBtwScene*platformSize,distanceBtwScene*platformSize), inicio.y+30, inicio.z), Quaternion.identity);
    }
}