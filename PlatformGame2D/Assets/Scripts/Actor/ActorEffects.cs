using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorEffects : MonoBehaviour
{
    public GameObject dustEffect;
    public GameObject jumpEffect;
    public GameObject explosionEffect;
    public GameObject bloodEffect;

    public void play(string effect, Vector3 position,  Quaternion quaternion, float destroyTime){
        GameObject gmEffect = null;
        
        switch(effect){
            case "dustEffect":{
                gmEffect = dustEffect;
                break;
            }
            case "jumpEffect":{
                gmEffect = jumpEffect;
                break;
            }
            case "explosionEffect":{
                gmEffect = explosionEffect;
                break;
            }
            case "bloodEffect":{
                gmEffect = bloodEffect;
                break;
            }
            default:{
                break;
            }
        }

        Destroy(Instantiate(gmEffect, position, quaternion), destroyTime);
    }
}
