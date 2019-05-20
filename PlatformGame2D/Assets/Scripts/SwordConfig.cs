using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordConfig : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject.tag == "Player"){
            col.gameObject.GetComponent<PlayerConfig>().damage(Random.Range(5,1));
        }
        if(col.gameObject.tag == "Enemy"){
            col.gameObject.GetComponent<EnemyConfig>().damage(Random.Range(10,30));
        }
    }
}
