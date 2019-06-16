using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Actor : MonoBehaviour
{
    protected float destroyTime = 8f;
    protected float damageFlashTime = 0.07f;
    protected float knockbackForce = 300f;
    protected float fallingTime = 0f;
    protected float fallingTimeToDie = 2f;
    protected Color actorColor = Color.blue;
    protected GameObject hand;
    protected bool startWithWeapon = false;
    protected bool shakeCamera = true;

    protected float health = 100;
    protected int kills = 0;

    protected GameObject actor;
    protected Transform actorTf;
    protected Rigidbody2D actorRb;
    protected GroundCheck groundCheck;
    protected ActorAnimations animations;
    protected ActorEffects effects;
    protected Move move;
    protected Jump jump;
    protected Weapon weapon;

    protected void Start(){
        actor = gameObject;
        actorTf = actor.transform;
        actorRb = actor.GetComponent<Rigidbody2D>();
        effects = GetComponent<ActorEffects>();
        hand = getChildGameObject(gameObject, "Hand");

        groundCheck = new GroundCheck((getChildGameObject(gameObject, "GroundCheck")).transform, LayerMask.GetMask("Ground"), effects, shakeCamera);
        animations = new ActorAnimations(GetComponent<Animator>());
        move = new Move(actor, actorRb, groundCheck, animations);
        weapon = new Weapon(actor, LayerMask.GetMask("Sword"), hand, startWithWeapon);
        jump = new Jump(actor, actorRb, groundCheck, animations, effects, hand, weapon);
    }
    
    protected IEnumerator DamageFlash() {
        SpriteRenderer[] sprites = GetComponentsInChildren<SpriteRenderer>();
        
        for(int i = 0; i < sprites.Length; i++){
            sprites[i].color = actorColor;
        }

        yield return new WaitForSeconds(damageFlashTime);

        for(int i = 0; i < sprites.Length; i++){
            sprites[i].color = Color.white;
        }
    }

    public bool isAttacking(){
        return weapon.isAttacking();
    }

    public void updateKills(){
        kills++;
    }

    public GameObject getChildGameObject(GameObject fromGameObject, string withName) {
         Transform[] ts = fromGameObject.transform.GetComponentsInChildren<Transform>(true);
         foreach (Transform t in ts) if (t.gameObject.name == withName) return t.gameObject;
         return null;
    }
}
