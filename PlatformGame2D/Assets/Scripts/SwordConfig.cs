using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordConfig : MonoBehaviour
{
    public int swordDamage = 28;

    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject.tag == "Player"){
            if(col.gameObject.GetComponent<PlayerConfig>() != null && gameObject.GetComponentInParent<EnemyConfig>() != null && gameObject.GetComponentInParent<EnemyConfig>().isEnemyAttacking()){
                col.gameObject.GetComponent<PlayerConfig>().damage(swordDamage/10);
            }
        }
        if(col.gameObject.tag == "Enemy"){
            if(col.gameObject.GetComponent<EnemyConfig>() != null && gameObject.GetComponentInParent<PlayerConfig>() != null && gameObject.GetComponentInParent<PlayerConfig>().isPlayerAttacking()){
                col.gameObject.GetComponent<EnemyConfig>().damage(swordDamage);
            }
        }
    }
}
