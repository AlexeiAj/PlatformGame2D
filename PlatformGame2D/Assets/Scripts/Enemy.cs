using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Actor
{
    private GameObject player;
    private Transform playerTf;
    public GameObject bloodSplash;
    public GameObject corpse;

    protected new void Start() {
        base.startWithWeapon = true;
        base.shakeCamera = false;
        base.knockbackForce = 800f;
        base.Start();
        player = GameObject.FindWithTag("Player");
        if(player) playerTf = player.transform;
        base.actorColor = Color.red;
    }

    void FixedUpdate(){
        if(!actor || !player) return;
        
        groundCheck.updateGrounded(playerInsideScreen());

        if(playerCloserThanX(4)) move.moveActor(0, playerInsideScreen());
        else move.moveActor(playerTf.position.x - gameObject.transform.position.x > 0 ? 1 : -1, playerInsideScreen());
        weapon.hit(playerCloserThanX(4));
        jumpVerify();
        wallJumpVerify();
        fallingTimeUpdate();
    }

    private void wallJumpVerify(){
        if(!playerCloserThanY(5)) jump.wallJump();
    }
    
    private void jumpVerify(){
        jump.jumpUpdate(!playerCloserThanY(5), playerInsideScreen());
        if(!playerCloserThanY(5)) jump.doJump();
        if(playerCloserThanY(5)) jump.releaseJump();
    }
    
    protected void fallingTimeUpdate(){
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
        if(playerInsideScreen()) SoundManager.PlaySound("die");
        effects.play("explosionEffect", actorTf.position, Quaternion.identity, 3f);

        GameObject.FindGameObjectWithTag("Player").gameObject.GetComponent<Player>().updateKills();
        
        GameObject bloodSplashInstance = Instantiate(bloodSplash, actorTf.position, Quaternion.identity);
        bloodSplashInstance.transform.localScale = new Vector3(Random.Range(4,7), Random.Range(4,7), 1);
        Destroy(bloodSplashInstance, 32f);
        
        GameObject corpseInstance = Instantiate(corpse, actorTf.position, Quaternion.identity);
        Destroy(corpseInstance, 3f);

        cameraShake();
        Destroy(actor);
	}

    bool playerCloserThanX(float distance){
        return Mathf.Abs(playerTf.position.x - gameObject.transform.position.x) < distance;
    }

    bool playerCloserThanY(float distance){
        return Mathf.Abs(playerTf.position.y - gameObject.transform.position.y) < distance;
    }

    void cameraShake(){
        if(playerInsideScreen()) CameraConfig.shake();
    }

    bool playerInsideScreen(){
        return playerCloserThanX(30) && playerCloserThanY(30);
    }
}
