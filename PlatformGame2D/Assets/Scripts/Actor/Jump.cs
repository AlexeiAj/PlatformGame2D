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
    private float startTimeBtwWallJump = 0.45f;
    private float timeBtwWallJump;
    private float timeBtwJump;
    private float destroyTime = 8f;

    private GameObject actor;
    private Transform actorTf;
    private Rigidbody2D actorRb;
    private GroundCheck groundCheck;
    private ActorAnimations animations;
    private ActorEffects effects;
    private GameObject hand;
    private Weapon weapon;
    private bool playerClose = true;

    public Jump(GameObject actor, Rigidbody2D actorRb, GroundCheck groundCheck, ActorAnimations animations, ActorEffects effects, GameObject hand, Weapon weapon){
        this.actor = actor;
        this.actorTf = actor.transform;
        this.actorRb = actorRb;
        this.groundCheck = groundCheck;
        this.animations = animations;
        this.effects = effects;
        this.hand = hand;
        this.weapon = weapon;

        timeBtwJump = startTimeBtwJump;
        timeBtwWallJump = startTimeBtwWallJump;
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

    public void jumpUpdate(bool pressing, bool playerClose){
        this.playerClose = playerClose;
        jumpUpdate(pressing);
    }

    public void releaseJump(){
        isJumping = false;
    }

    public void wallJump(){
        bool isNextWall = Physics2D.OverlapCircle(hand.transform.position, 1f, LayerMask.GetMask("Wall"));
        if(timeBtwWallJump < 0 && !groundCheck.isGrounded() && weapon.isAttacking() && isNextWall){
            releaseJump();
            float force = -Physics.gravity.y * jumpForce * 2 * actorRb.mass;
            actorRb.velocity = new Vector2(force * -actor.transform.localScale.x, force);
            timeBtwWallJump = startTimeBtwWallJump;
        }
        timeBtwWallJump -= Time.deltaTime;
    }

    public void doJump(){
        if(!groundCheck.isGrounded() || isJumping) return;

        isJumping = true;
        if(playerClose) SoundManager.PlaySound("jump");
        animations.play("takeOf", false);
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
