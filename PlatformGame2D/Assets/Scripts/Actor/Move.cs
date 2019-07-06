using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move
{
    private bool playerFacingRight = true;
    private float maxVelocity = 20.0f;
    private float moveForce = 100.0f;
    private float fakeFrictionValue = 0.95f;
    private bool isRunning;
    private float startTimeBtwRunSound = 0.3f;
    private float timeBtwRunSound;
    private bool playerClose = true;
    
    private GameObject actor;
    private Transform actorTf;
    private Rigidbody2D actorRb;
    private GroundCheck groundCheck;
    private ActorAnimations animations;

    public Move(GameObject actor, Rigidbody2D actorRb, GroundCheck groundCheck, ActorAnimations animations){
        this.actor = actor;
        this.actorTf = actor.transform;
        this.actorRb = actorRb;
        this.groundCheck = groundCheck;
        this.animations = animations;

        timeBtwRunSound = startTimeBtwRunSound;
    }

    public void moveActor(float side){
        isRunning = side != 0 && groundCheck.isGrounded();
        
        actorRb.AddForce((Vector2.right * moveForce * actorRb.mass) * side);
        if(Mathf.Abs(actorRb.velocity.x) > maxVelocity) actorRb.velocity = new Vector2(maxVelocity * side, actorRb.velocity.y);
        animations.play("isRunning", isRunning);

        flip(side);
        walkSound();
        fakeFriction();
    }

    public void moveActor(float side, bool playerClose){
        this.playerClose = playerClose;
        moveActor(side);
    }

    private void walkSound(){
        if(!isRunning) return;

        if(timeBtwRunSound <= 0){
            timeBtwRunSound = startTimeBtwRunSound;
            if(playerClose) SoundManager.PlaySound("run");
        }
        timeBtwRunSound -= Time.deltaTime;
    }
    
    private void fakeFriction(){
		if(isRunning) actorRb.velocity = new Vector3(actorRb.velocity.x * fakeFrictionValue, actorRb.velocity.y);
	}

    private void flip(float side){
        if(side < 0 && playerFacingRight || side > 0 && !playerFacingRight){
            playerFacingRight = !playerFacingRight;
            actorTf.localScale = new Vector3(actorTf.localScale.x * -1, actorTf.localScale.y, actorTf.localScale.z);
        }
    }
}
