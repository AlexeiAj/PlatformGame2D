using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerConfig : MonoBehaviour
{
    public GameObject player;
    public GameObject groundCheck;

    public bool isGrounded = false;

    void FixedUpdate()
    {
        verifyGrounded();
        moveTorso();
        jumpPlayer();
    }
    
    void moveTorso()
    {
        if (!isGrounded) return;

        float moveForce = 30;

        if (Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D))
        {
            if (player.GetComponent<Rigidbody2D>().velocity.x < 0.5f)
            {
                player.GetComponent<Rigidbody2D>().velocity = new Vector2(10, player.GetComponent<Rigidbody2D>().velocity.y);
            }
            player.GetComponent<Rigidbody2D>().AddForce(Vector2.right * moveForce);
        }
        else if (Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A))
        {
            if (player.GetComponent<Rigidbody2D>().velocity.x > -0.5f)
            {
                player.GetComponent<Rigidbody2D>().velocity = new Vector2(-10, player.GetComponent<Rigidbody2D>().velocity.y);
            }
            player.GetComponent<Rigidbody2D>().AddForce(Vector2.left * moveForce);
        }
        else
        {
            player.GetComponent<Rigidbody2D>().velocity = new Vector2(0, player.GetComponent<Rigidbody2D>().velocity.y);
        }
    }

    void jumpPlayer()
    {
        if (!isGrounded || !Input.GetKey(KeyCode.Space)) return;

        float jumpForce = 0.25f;
        
        player.GetComponent<Rigidbody2D>().velocity = new Vector2(player.GetComponent<Rigidbody2D>().velocity.x, 0);
        player.GetComponent<Rigidbody2D>().AddForce(transform.up * (-Physics.gravity.y * jumpForce), ForceMode2D.Impulse);
    }

    void verifyGrounded()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.transform.position, 0.45f, LayerMask.GetMask("Ground"));
    }
}
