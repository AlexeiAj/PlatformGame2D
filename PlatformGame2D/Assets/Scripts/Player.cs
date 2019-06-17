using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Actor
{
    protected new void Start(){
        base.Start();
        base.fallingTimeToDie = 3f;
        base.knockbackForce = 100f;
    }

    void FixedUpdate(){
        if(!actor) return;

        groundCheck.updateGrounded();
        move.moveActor(Input.GetAxisRaw("Horizontal"));
        jumpVerify();
        weapon.weaponUpdate();
        fallingTimeUpdate();
        updateDisplay();
        wallJumpVerify();
    }

    private void wallJumpVerify(){
        if(Input.GetButton("Jump")) jump.wallJump();
    }

    private void jumpVerify(){
        jump.jumpUpdate(Input.GetButton("Jump"));
        if(Input.GetButton("Jump")) jump.doJump();
        if(Input.GetButtonUp("Jump")) jump.releaseJump();
    }
    
    private void fallingTimeUpdate(){
        if(fallingTime > fallingTimeToDie && gameObject.GetComponent<Rigidbody2D>().velocity.y < -100) die();

        if (groundCheck.isFallingAboveEnemy()) fallingTime += Time.deltaTime;
        else fallingTime = 0;
    }

    public void damage(int damage){
        health = health <= 0 ? 0 : health - damage;
        
        if(health <= 0){
            die();
            return;
        }
        
        //knockback
        actorRb.velocity = Vector2.zero;
		actorRb.AddForce(new Vector2(0, knockbackForce));

        effects.play("bloodEffect", actorTf.position, Quaternion.identity, destroyTime);
        StartCoroutine(DamageFlash());
    }
    
    public void die(){
        cleanData();
        SoundManager.PlaySound("explosion");
        effects.play("explosionEffect", actorTf.position, Quaternion.identity, destroyTime);
        CameraConfig.shake();
        Destroy(actor);
	}

    private void cleanData(){
        health = 0;
        fallingTime = 0;
        updateDisplay();
    }

    private void updateDisplay(){
        HealthDisplay.Health = health;
        KillDisplay.Kill = kills;
    }
}
