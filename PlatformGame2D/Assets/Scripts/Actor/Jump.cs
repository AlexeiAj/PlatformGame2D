using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jump
{
    private float jumpForce = 2f;
    private float timeBtwJumpTrail;
    private bool isJumping;
    private float startTimeBtwJumpTrail = 0.2f;
    private float startTimeBtwJump = 0.45f;
    private float timeBtwJump;
    private float destroyTime = 8f;

    private GameObject actor;
    private Transform actorTf;
    private Rigidbody2D actorRb;
    private GroundCheck groundCheck;
    private ActorAnimations animations;
    private ActorEffects effects;

    public Jump(GameObject actor, Rigidbody2D actorRb, GroundCheck groundCheck, ActorAnimations animations, ActorEffects effects){
        this.actor = actor;
        this.actorTf = actor.transform;
        this.actorRb = actorRb;
        this.groundCheck = groundCheck;
        this.animations = animations;
        this.effects = effects;

        timeBtwJump = startTimeBtwJump;
        timeBtwJumpTrail = startTimeBtwJumpTrail;
    }

    public void jumpUpdate(bool pressing){
        animations.play("isJumping", !groundCheck.isGrounded());
        jumpTrail();

        if(timeBtwJump > 0 && isJumping && pressing){
            actorRb.velocity = new Vector2(actorRb.velocity.x, -Physics.gravity.y * jumpForce * actorRb.mass);
            timeBtwJump -= Time.deltaTime;
        }else{
            isJumping = false;
        }
    }

    public void releaseJump(){
        isJumping = false;
    }

    public void doJump(){
        if(!groundCheck.isGrounded() || isJumping) return;

        SoundManager.PlaySound("jump");
        animations.play("takeOf", false);
        isJumping = true;
        actorRb.velocity = Vector2.up * jumpForce;
        timeBtwJump = startTimeBtwJump; 
    }

    private void jumpTrail(){
        if (!groundCheck.isGrounded() && timeBtwJumpTrail <= 0){
            effects.play("jumpEffect", groundCheck.getGroundCheckPosition(), Quaternion.identity, destroyTime);
            timeBtwJumpTrail = startTimeBtwJumpTrail;
        }
        timeBtwJumpTrail -= Time.deltaTime;
    } 
}
