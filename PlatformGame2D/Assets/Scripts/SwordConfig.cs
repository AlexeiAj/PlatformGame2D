using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordConfig : MonoBehaviour
{
    public int swordDamage = 28;
    private float checkPlayerRadius = 8f;
    private GameObject glow;
    private bool isEquiped = false;

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

    void Start()
    {
        glow = GameObject.FindWithTag("Glow");      
    }

    void Update()
    {
        if(gameObject.tag == "Enemy") return;

        if(isEquiped){
            if(glow) glow.SetActive(false);
            return;
        }
        Collider2D col = Physics2D.OverlapCircle(gameObject.transform.position, checkPlayerRadius, LayerMask.GetMask("Player"));
        if(glow) glow.SetActive(col != null && col.gameObject.tag == "Player");
    }

    public void setEquiped(bool eq){
        this.isEquiped = eq;
    }
}
