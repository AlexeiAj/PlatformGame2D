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
    private float maxVelocity = 20.0f;
    private float moveForce = 50.0f;
    private float jumpForce = 2.1f;
    private bool playerFacingRight = true;
    private float health = 100;
    private float fallingTime = 0f;

    //groundCheck
    private Transform groundCheck;
    private LayerMask groundLayer;
    private bool isGrounded = false;
    private float checkGroundRadius = 1f;
    private float fakeFrictionValue = 0.95f;

    //gameConfig
    private bool render = false;
    public GameObject dustEffect;
    public GameObject jumpEffect;
    public GameObject explosionEffect;
    public GameObject bloodEffect;
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
        timeBtwAttacking = -startTimeBtwAttacking;
        timeBtwRunSound = startTimeBtwRunSound;

        //groundCheck
        groundCheck = GameObject.FindWithTag("GroundCheck").transform;
        groundLayer = LayerMask.GetMask("Ground");
    }

    void FixedUpdate()
    {
        if(!player) return;

        verifyGrounded();
        movePlayer(Input.GetAxisRaw("Horizontal"));
        jumpPlayer();
        hitPlayer();
        updateHealth();
        fallingTimeUpdate();
    }

    void updateHealth(){
        HealthDisplay.Health = health;
    }

    void damage(int damage){
        health = health <= 0 ? 0 : health - damage;
        if(health <= 0){
            die();
            return;
        }
        
        GameObject instance = Instantiate(bloodEffect, playerTf.position, Quaternion.identity);
        Destroy(instance, 8f);
    }

    void hitPlayer(){
        if (!isGrounded && timeBtwTrail <= 0){
            GameObject instance = Instantiate(jumpEffect, groundCheck.position, Quaternion.identity);
            Destroy(instance, 8f);
            timeBtwTrail = startTimeBtwTrail;
        }
        timeBtwTrail -= Time.deltaTime;
        
        if((Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.LeftShift)) && timeBtwAttacking <= 0){
            damage(30);
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
        if(Mathf.Abs(playerRb.velocity.x) > maxVelocity) playerRb.velocity = new Vector2(maxVelocity * side, playerRb.velocity.y);
       
        flip(side);

        if(!isGrounded) return;

        if(side != 0 && timeBtwRunSound <= 0){
            timeBtwRunSound = startTimeBtwRunSound;
            SoundManager.PlaySound("run");
        }
        timeBtwRunSound -= Time.deltaTime;

        anim.SetBool("isRunning", side != 0);
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

    void fallingTimeUpdate(){
        if(fallingTime > 2) die();

        if (!isGrounded) fallingTime += Time.deltaTime;
        else fallingTime = 0;
    }

    void die(){
        health = 0;
        updateHealth();
        fallingTime = 0;
        SoundManager.PlaySound("explosion");
        GameObject instance = Instantiate(explosionEffect, playerTf.position, Quaternion.identity);
        Destroy(instance, 8f);
        CameraConfig.shake();
        Destroy(player);
	}

    void jumpPlayer()
    {
        if (!isGrounded || !Input.GetKeyDown(KeyCode.Space)) return;

        SoundManager.PlaySound("jump");
        anim.SetTrigger("takeOf");
        playerRb.velocity = new Vector2(playerRb.velocity.x, 0);
        playerRb.AddForce(transform.up * (-Physics.gravity.y * jumpForce * playerRb.mass), ForceMode2D.Impulse);
    }

    void verifyGrounded()
    {
        bool isGroundedPreviousState = isGrounded;
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, checkGroundRadius, groundLayer);

        anim.SetBool("isRunning", isGrounded);
        anim.SetBool("isJumping", !isGrounded);

        if(!isGroundedPreviousState && isGrounded){
            SoundManager.PlaySound("land");
            GameObject instance = Instantiate(dustEffect, groundCheck.position, Quaternion.identity);
            Destroy(instance, 8f);
            CameraConfig.shake();
        }
    }

    void OnDrawGizmosSelected()
    {
        if(!render) return;

        Gizmos.color = Color.red;
        Gizmos.DrawSphere(groundCheck.position, checkGroundRadius);
    }
}
