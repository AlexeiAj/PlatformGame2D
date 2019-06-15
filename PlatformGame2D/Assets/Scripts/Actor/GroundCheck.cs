﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheck
{
    private bool grounded = false;
    private bool groundedPreviousState = false;
    private float radius = 1f;
    private float destroyTime = 8f;
    private float timeBtwLand;
    private float startTimeBtwLand = 0.2f;

    private Transform groundCheck;
    private LayerMask groundLayer;
    private ActorEffects effects;

    public GroundCheck(Transform groundCheck, LayerMask groundLayer, ActorEffects effects){
        this.groundCheck = groundCheck;
        this.groundLayer = groundLayer;
        this.effects = effects;

        timeBtwLand = startTimeBtwLand;
    }

    public void updateGrounded(){
        grounded = Physics2D.OverlapCircle(groundCheck.position, radius, groundLayer);
    }

    public bool isGrounded(){
        land();
        groundedPreviousState = grounded;
        return grounded;
    }

    private void land(){
        if(!groundedPreviousState && grounded && timeBtwLand <= 0){
            SoundManager.PlaySound("land");
            effects.play("dustEffect", groundCheck.transform.position, Quaternion.identity, destroyTime);
            CameraConfig.shake();
            timeBtwLand = startTimeBtwLand;
        }
        timeBtwLand -= Time.deltaTime;
    }

    public bool isGroundedPreviousState(){
        return groundedPreviousState;
    }

    public Vector3 getGroundCheckPosition(){
        return groundCheck.transform.position;
    }

    public bool isFallingAboveEnemy(){
        return !grounded;
    }
}
