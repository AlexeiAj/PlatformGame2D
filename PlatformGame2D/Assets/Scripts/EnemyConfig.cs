using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyConfig : MonoBehaviour
{
    //enemy
    private GameObject enemy;
    private Transform enemyTf;
    private Rigidbody2D enemyRb;
    private Animator anim;
    public Animator animSword;

    //enemyConfig
    private float maxVelocity = 20.0f;
    private float moveForce = 50.0f;
    private float jumpForce = 2.1f;
    private bool enemyFacingRight = true;
    private float health = 100;
    private float fallingTime = 0f;

    //groundCheck
    public Transform groundCheck;
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

    private GameObject player;
    private Transform playerTf;

    void Start(){
        //enemy
        enemy = this.gameObject;
        enemyTf = enemy.transform;
        enemyRb = enemy.GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        timeBtwTrail = startTimeBtwTrail;
        timeBtwAttacking = -startTimeBtwAttacking;
        timeBtwRunSound = startTimeBtwRunSound;

        //player
        player = GameObject.FindWithTag("Player");
        playerTf = player.transform;

        //groundCheck
        groundLayer = LayerMask.GetMask("Ground");
    }

    void FixedUpdate()
    {
        if(!enemy || !player) return;

        verifyGrounded();
        moveEnemy(playerTf.position.x - enemyTf.position.x > 0 ? 1 : -1);
        jumpEnemy();
        hitEnemy();
        fallingTimeUpdate();
    }


    public void damage(int damage){
        health = health <= 0 ? 0 : health - damage;
        if(health <= 0){
            die();
            return;
        }
        
        GameObject instance = Instantiate(bloodEffect, enemyTf.position, Quaternion.identity);
        Destroy(instance, 8f);
        StartCoroutine(DamageFlash());
    }

    private IEnumerator DamageFlash() {

        SpriteRenderer[] sprites = GetComponentsInChildren<SpriteRenderer>();
        
        for(int i = 0; i < sprites.Length; i++){
            sprites[i].color = Color.red;
        }

        yield return new WaitForSeconds(0.07f);

        for(int i = 0; i < sprites.Length; i++){
            sprites[i].color = Color.white;
        }
    }

    void hitEnemy(){
        if (!isGrounded && timeBtwTrail <= 0){
            GameObject instance = Instantiate(jumpEffect, groundCheck.position, Quaternion.identity);
            Destroy(instance, 8f);
            timeBtwTrail = startTimeBtwTrail;
        }
        timeBtwTrail -= Time.deltaTime;
        
        if(Mathf.Abs(playerTf.position.x - enemyTf.position.x) < 4 && timeBtwAttacking <= 0){
            SoundManager.PlaySound("slash");
            swordTrail.emitting = true;
            timeBtwAttacking = startTimeBtwAttacking;
        } 

        animSword.SetBool("isAttacking", timeBtwAttacking > 0);
        if(timeBtwAttacking < 0) swordTrail.emitting = false;
        timeBtwAttacking -= Time.deltaTime;
    }

    void moveEnemy(float side)
    {
        if(Mathf.Abs(playerTf.position.x - enemyTf.position.x) <= 4) side= 0;

        fakeFriction();
            
        enemyRb.AddForce((Vector2.right * moveForce * enemyRb.mass) * side);
        if(Mathf.Abs(enemyRb.velocity.x) > maxVelocity) enemyRb.velocity = new Vector2(maxVelocity * side, enemyRb.velocity.y);

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
		if(isGrounded) enemyRb.velocity = new Vector3(enemyRb.velocity.x * fakeFrictionValue, enemyRb.velocity.y);
	}

    void flip(float side){
        if(side < 0 && enemyFacingRight || side > 0 && !enemyFacingRight){
            enemyFacingRight = !enemyFacingRight;
            enemyTf.localScale = new Vector3(enemyTf.localScale.x * -1, enemyTf.localScale.y, enemyTf.localScale.z);
        }
    }

    void fallingTimeUpdate(){
        if(fallingTime > 2) die();

        if (!isGrounded) fallingTime += Time.deltaTime;
        else fallingTime = 0;
    }

    void die(){
        health = 0;
        fallingTime = 0;
        SoundManager.PlaySound("explosion");
        GameObject instance = Instantiate(explosionEffect, enemyTf.position, Quaternion.identity);
        Destroy(instance, 8f);
        CameraConfig.shake();
        Destroy(enemy);
	}

    void jumpEnemy()
    {
        if (!isGrounded || playerTf.position.y - enemyTf.position.y < 5) return;

        SoundManager.PlaySound("jump");
        anim.SetTrigger("takeOf");
        enemyRb.velocity = new Vector2(enemyRb.velocity.x, 0);
        enemyRb.AddForce(transform.up * (-Physics.gravity.y * jumpForce * enemyRb.mass), ForceMode2D.Impulse);
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
