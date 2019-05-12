using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneGeneration : MonoBehaviour
{
    public GameObject sceneLevel;

    void Start()
    {
        Instantiate(sceneLevel, transform.position, Quaternion.identity);
        
        //size of 3 platform
        float distanceBtwScene = 38.4f;
        Vector3 inicio = transform.position;

        for(int i = 1; i < 10; i++){
            Instantiate(sceneLevel, new Vector3(inicio.x * (i*distanceBtwScene), inicio.y, inicio.z), Quaternion.identity);
            Instantiate(sceneLevel, new Vector3(inicio.x * (-i*distanceBtwScene), inicio.y, inicio.z), Quaternion.identity);
        }
    }
}
