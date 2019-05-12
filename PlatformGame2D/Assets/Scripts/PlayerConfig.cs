using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerConfig : MonoBehaviour
{
    //player
    private GameObject player;
    private Transform playerTf;
    private Rigidbody2D playerRb;
    private Animator anim;
    public Animator animSword;

    //playerConfig
    float maxVelocity = 40.0f;
    float moveForce = 50.0f;
    float jumpForce = 2.1f;
    private bool playerFacingRight = true;

    //groundCheck
    private Transform groundCheck;
    private LayerMask groundLayer;
    private bool isGrounded = false;
    private float checkGroundRadius = 0.1f;
    private float fakeFrictionValue = 0.95f;

    //gameConfig
    private bool render = false;
    public Animator animCam;
    public GameObject dustEffect;
    public GameObject jumpEffect;
    private float startTimeBtwTrail = 0.2f;
    private float timeBtwTrail;
    private float startTimeBtwAttacking = 0.2f;
    private float timeBtwAttacking;
    private float startTimeBtwRunSound = 0.3f;
    private float timeBtwRunSound;
    public TrailRenderer swordTrail;

    void Start(){
        //player
        player = GameObject.FindWithTag("Player");
        playerTf = player.transform;
        playerRb = player.GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        timeBtwTrail = startTimeBtwTrail;
        timeBtwAttacking = startTimeBtwAttacking;
        timeBtwRunSound = startTimeBtwRunSound;

        //groundCheck
        groundCheck = GameObject.FindWithTag("GroundCheck").transform;
        groundLayer = LayerMask.GetMask("Ground");
    }

    void FixedUpdate()
    {
        verifyGrounded();
        movePlayer(Input.GetAxisRaw("Horizontal"));
        jumpPlayer();
        hitPlayer();
    }

    void hitPlayer(){
        if((Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.LeftShift)) && timeBtwAttacking <= 0){
            SoundManager.PlaySound("slash");
            swordTrail.emitting = true;
            timeBtwAttacking = startTimeBtwAttacking;
        } 
        animSword.SetBool("isAttacking", timeBtwAttacking > 0);
        if(timeBtwAttacking < 0) swordTrail.emitting = false;
        timeBtwAttacking -= Time.deltaTime;
    }

    void movePlayer(float side)
    {
        fakeFriction();
        playerRb.AddForce((Vector2.right * moveForce * playerRb.mass) * side);

        if(side != 0 && timeBtwRunSound <= 0){
            timeBtwRunSound = startTimeBtwRunSound;
            SoundManager.PlaySound("run");
        }
        timeBtwRunSound -= Time.deltaTime;

        anim.SetBool("isRunning", side != 0);
        if(Mathf.Abs(playerRb.velocity.x) > maxVelocity) playerRb.velocity = new Vector2(maxVelocity * side, playerRb.velocity.y);
       
        flip(side);
    }

    void fakeFriction(){
		if(isGrounded) playerRb.velocity = new Vector3(playerRb.velocity.x * fakeFrictionValue, playerRb.velocity.y);
	}

    void flip(float side){
        if(side < 0 && playerFacingRight || side > 0 && !playerFacingRight){
            playerFacingRight = !playerFacingRight;
            playerTf.localScale = new Vector3(playerTf.localScale.x * -1, playerTf.localScale.y, playerTf.localScale.z);
        }
    }

    void jumpPlayer()
    {
        if (!isGrounded && timeBtwTrail <= 0){
            GameObject instance = Instantiate(jumpEffect, groundCheck.position, Quaternion.identity);
            Destroy(instance, 8f);
            timeBtwTrail = startTimeBtwTrail;
        }
        timeBtwTrail -= Time.deltaTime;

        if (!isGrounded || !Input.GetKey(KeyCode.Space)) return;

        SoundManager.PlaySound("jump");
        anim.SetTrigger("takeOf");
        playerRb.velocity = new Vector2(playerRb.velocity.x, 0);
        playerRb.AddForce(transform.up * (-Physics.gravity.y * jumpForce * playerRb.mass), ForceMode2D.Impulse);
    }

    void verifyGrounded()
    {
        bool isGroundedPreviousState = isGrounded;
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkGroundRadius, groundLayer);

        anim.SetBool("isJumping", !isGrounded);

        if(!isGroundedPreviousState && isGrounded){
            SoundManager.PlaySound("land");
            GameObject instance = Instantiate(dustEffect, groundCheck.position, Quaternion.identity);
            Destroy(instance, 8f);
            animCam.SetTrigger("shake");
        }
    }

    void OnDrawGizmosSelected()
    {
        if(!render) return;

        Gizmos.color = Color.red;
        Gizmos.DrawSphere(groundCheck.position, checkGroundRadius);
    }
}
