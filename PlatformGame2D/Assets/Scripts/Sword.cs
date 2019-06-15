using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
    public int swordDamage = 28;
    private float checkPlayerRadius = 8f;
    private SpriteRenderer glow;
    private bool equipped = false;

    void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject.tag == "Player"){
            if(col.gameObject.GetComponent<Player>() != null && gameObject.GetComponentInParent<Enemy>() != null && gameObject.GetComponentInParent<Actor>().isAttacking()){
                col.gameObject.GetComponent<Player>().damage(swordDamage/10);
            }
        }
        
        if(col.gameObject.tag == "Enemy"){
            if(col.gameObject.GetComponent<Enemy>() != null && gameObject.GetComponentInParent<Player>() != null && gameObject.GetComponentInParent<Actor>().isAttacking()){
                col.gameObject.GetComponent<Enemy>().damage(swordDamage);
            }
        }
    }

    void Start(){
        glow = getChildGameObject(gameObject, "Glow").GetComponent<SpriteRenderer>();
        if(glow) glow.enabled = false;
    }

    void Update(){
        if(!glow) return;

        if(equipped){
            glow.enabled = false;
            return;
        }

        Collider2D[] colliders = Physics2D.OverlapCircleAll(gameObject.transform.position, checkPlayerRadius, LayerMask.GetMask("Player"));
        bool show = false;
        for (int i = 0; i < colliders.Length; i++) if(colliders[i].gameObject.tag == "Player") show = true;

        glow.enabled = show;
    }

    public void setEquipped(bool equipped){
        this.equipped = equipped;
    }

    public bool isEquipped(){
        return equipped;
    }

    public GameObject getChildGameObject(GameObject fromGameObject, string withName) {
         Transform[] ts = fromGameObject.transform.GetComponentsInChildren<Transform>(true);
         foreach (Transform t in ts) if (t.gameObject.name == withName) return t.gameObject;
         return null;
    }
}
