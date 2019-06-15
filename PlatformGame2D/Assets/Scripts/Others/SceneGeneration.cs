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
        float distanceBtwScene = 300;
        Vector3 inicio = transform.position;
        Vector3 inicioBack = new Vector3(transform.position.x-0.027f,transform.position.y,transform.position.z);

        int i;
        for(i = 1; i < 4; i++){
            Instantiate(sceneLevel, new Vector3(inicio.x + (i*distanceBtwScene), inicio.y, inicio.z), Quaternion.identity);
            Instantiate(sceneLevel, new Vector3(inicioBack.x * (-i*distanceBtwScene), inicioBack.y, inicioBack.z), Quaternion.identity);
        }
    }
}
