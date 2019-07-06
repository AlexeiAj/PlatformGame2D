using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
    public int swordDamage = 28;
    public GameObject keyE;
    private float checkPlayerRadius = 8f;
    private SpriteRenderer glow;
    private bool equipped = false;
    private GameObject KeyEInstance;

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
        addKeyE();

        if(equipped){
            glow.enabled = false;
            return;
        }

        Collider2D[] colliders = Physics2D.OverlapCircleAll(gameObject.transform.position, checkPlayerRadius, LayerMask.GetMask("Player"));
        bool show = false;
        for (int i = 0; i < colliders.Length; i++) if(colliders[i].gameObject.tag == "Player") show = true;

        glow.enabled = show;
    }

    public void addKeyE(){
        var pos = new Vector3(glow.gameObject.transform.position.x, glow.gameObject.transform.position.y + 1, glow.gameObject.transform.position.z);
        if(isGlowwing() && !KeyEInstance) KeyEInstance = Instantiate(keyE, pos, Quaternion.identity);
        if(!isGlowwing() && KeyEInstance) Destroy(KeyEInstance);
        if(KeyEInstance) KeyEInstance.transform.position = pos;
    }

    public bool isGlowwing(){
        return glow.enabled;
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
