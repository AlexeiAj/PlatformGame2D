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
    private float moveForce = 100.0f;
    private bool playerFacingRight = true;
    private float health = 100;
    private float fallingTime = 0f;
    private bool isJumping;
    private float startTimeBtwJump = 0.45f;
    private float timeBtwJump;
    private float jumpForce = 2f;
    private bool isAttacking = false;
    private int kills = 0;

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
    private float startTimeBtwAttacking = 0.4f;
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
        timeBtwJump = startTimeBtwJump;

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
        updateDisplay();
        fallingTimeUpdate();
    }

    void updateDisplay(){
        HealthDisplay.Health = health;
        KillDisplay.Kill = kills;
    }

    public void damage(int damage){
        health = health <= 0 ? 0 : health - damage;
        if(health <= 0){
            die();
            return;
        }
        
        GameObject instance = Instantiate(bloodEffect, playerTf.position, Quaternion.identity);
        Destroy(instance, 8f);
        StartCoroutine(DamageFlash());
    }

    private IEnumerator DamageFlash() {

        SpriteRenderer[] sprites = GetComponentsInChildren<SpriteRenderer>();
        
        for(int i = 0; i < sprites.Length; i++){
            sprites[i].color = Color.blue;
        }

        yield return new WaitForSeconds(0.07f);

        for(int i = 0; i < sprites.Length; i++){
            sprites[i].color = Color.white;
        }
    }

    void hitPlayer(){
        if((Input.GetMouseButton(0) || Input.GetKeyDown(KeyCode.LeftShift)) && timeBtwAttacking <= 0 && !isAttacking){
            swordTrail.emitting = true;
            animSword.SetBool("isAttacking", true);
            SoundManager.PlaySound("slash");
            timeBtwAttacking = startTimeBtwAttacking;
            isAttacking = true;
        } 
        
        timeBtwAttacking -= Time.deltaTime;

        if(timeBtwAttacking < 0){
            animSword.SetBool("isAttacking", false);
            swordTrail.emitting = false;
            isAttacking = false;
        }
    }

    public bool isPlayerAttacking(){
        return isAttacking;
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
        updateDisplay();
        fallingTime = 0;
        SoundManager.PlaySound("explosion");
        GameObject instance = Instantiate(explosionEffect, playerTf.position, Quaternion.identity);
        Destroy(instance, 8f);
        CameraConfig.shake();
        Destroy(player);
	}

    void jumpPlayer()
    {
        if (!isGrounded && timeBtwTrail <= 0){
            GameObject instance = Instantiate(jumpEffect, groundCheck.position, Quaternion.identity);
            Destroy(instance, 8f);
            timeBtwTrail = startTimeBtwTrail;
        }
        timeBtwTrail -= Time.deltaTime;

        if(isGrounded && Input.GetKeyDown(KeyCode.Space)){
            SoundManager.PlaySound("jump");
            anim.SetTrigger("takeOf");
            isJumping = true;
            playerRb.velocity = Vector2.up * jumpForce;
            timeBtwJump = startTimeBtwJump;
        }

        if(Input.GetKey(KeyCode.Space) && isJumping){
            if(timeBtwJump > 0){
                playerRb.velocity = new Vector2(playerRb.velocity.x, -Physics.gravity.y * jumpForce * playerRb.mass);
                timeBtwJump -= Time.deltaTime;
            }else{
                 isJumping = false;
            }
        }

        if(Input.GetKeyUp(KeyCode.Space)) isJumping = false;
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

    public void updateKills(){
        kills++;
    }

    void OnDrawGizmosSelected()
    {
        if(!render) return;

        Gizmos.color = Color.red;
        Gizmos.DrawSphere(groundCheck.position, checkGroundRadius);
    }

    
}
