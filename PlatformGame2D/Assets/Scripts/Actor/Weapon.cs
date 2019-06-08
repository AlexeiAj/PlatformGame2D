using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon
{
    private bool attacking = false;
    private float startTimeBtwAttacking = 0.4f;
    private float timeBtwAttacking;
    private TrailRenderer swordTrail;
    private Animator animSword;
    private LayerMask weaponLayer;
    private GameObject weapon;
    private float checkWeaponRadius = 8f;
    private bool hasWeapon = false;
    public GameObject hand;

    public Weapon(GameObject actor, LayerMask weaponLayer, GameObject hand, bool startWithWeapon){
        this.weaponLayer = weaponLayer;
        this.hand = hand;
        
        if(startWithWeapon) startWeapon(actor);
        timeBtwAttacking = -startTimeBtwAttacking;
    }

    public void startWeapon(GameObject actor){
        if(!actor) return;
        weapon = getChildGameObject(actor, "Sword");
        
        if(!weapon || !weapon.activeSelf) return;

        hasWeapon = true;
        swordTrail = weapon.GetComponentInChildren<TrailRenderer>();
        swordTrail.emitting = false;
        weapon.gameObject.GetComponent<Sword>().setEquipped(hasWeapon);
        weapon.gameObject.GetComponent<Rigidbody2D>().isKinematic = true;
        weapon.gameObject.GetComponent<CapsuleCollider2D>().isTrigger = true;
        animSword = weapon.GetComponent<Animator>();
        animSword.enabled = true;
    }

    public void weaponUpdate(){
        hit(Input.GetMouseButton(0));
        pickupWeapon();
        throwWeapon();
    }

    public void hit(bool pressing){
        if(!hasWeapon) return;

        if(timeBtwAttacking <= 0 && !attacking && pressing){
            swordTrail.emitting = true;
            animSword.SetBool("isAttacking", true);
            SoundManager.PlaySound("slash");
            timeBtwAttacking = startTimeBtwAttacking;
            attacking = true;
        } 
        
        timeBtwAttacking -= Time.deltaTime;

        if(timeBtwAttacking < 0){
            animSword.SetBool("isAttacking", false);
            swordTrail.emitting = false;
            attacking = false;
        }
    }

    void pickupWeapon(){
        if(hasWeapon) return;

        Collider2D[] colliders = Physics2D.OverlapCircleAll(hand.transform.position, checkWeaponRadius, weaponLayer);
        Collider2D col = null;
        
        for (int i = 0; i < colliders.Length; i++){
            if(colliders[i].gameObject.tag == "Sword" && !colliders[i].gameObject.GetComponent<Sword>().isEquipped()){
                col = colliders[i];
            }
        }

        if(col == null) return;

        if(Input.GetKey(KeyCode.E)){
            hasWeapon = true;

            weapon = col.gameObject;
            weapon.gameObject.GetComponent<Sword>().setEquipped(hasWeapon);
            swordTrail = weapon.GetComponentInChildren<TrailRenderer>();
            swordTrail.emitting = false;
            
            weapon.gameObject.GetComponent<Rigidbody2D>().isKinematic = true;
            weapon.gameObject.GetComponent<CapsuleCollider2D>().isTrigger = true;
            animSword = weapon.GetComponent<Animator>();
            animSword.enabled = true;

            weapon.transform.parent = hand.gameObject.transform;
            weapon.transform.localPosition = Vector3.zero;
            weapon.transform.localRotation = Quaternion.identity;
        }
    }

    void throwWeapon(){
        if(!hasWeapon) return;

        if(Input.GetKey(KeyCode.F)){
            hasWeapon = false;

            weapon.gameObject.GetComponent<Sword>().setEquipped(hasWeapon);
            weapon.gameObject.GetComponent<Rigidbody2D>().isKinematic = false;
            weapon.gameObject.GetComponent<CapsuleCollider2D>().isTrigger = false;
            weapon.transform.parent = null;
            weapon.transform.position = hand.transform.position;
            weapon.transform.rotation = Quaternion.identity;
            animSword.enabled = false;

            weapon = null;
            swordTrail = null;
            animSword = null;
        }
    }

    public bool isAttacking(){
        return attacking;
    }

    public bool isWithWeapon(){
        return hasWeapon;
    }

    public GameObject getChildGameObject(GameObject fromGameObject, string withName) {
         Transform[] ts = fromGameObject.transform.GetComponentsInChildren<Transform>(true);
         foreach (Transform t in ts) if (t.gameObject.name == withName) return t.gameObject;
         return null;
    }
}
