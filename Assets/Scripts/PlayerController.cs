using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float Speed = 2;
    public float jumpForce = 3;
    public SpriteRenderer spriteRenderer;
    public Animator animator;

    Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void FixedUpdate()
    {
        if(Input.GetKey("d") || Input.GetKey("right"))
        {
            rb.velocity = new Vector2(Speed, rb.velocity.y);
            spriteRenderer.flipX = false;
            animator.SetBool("Movement", true);
        }
        else if (Input.GetKey("a") || Input.GetKey("left"))
        {
            rb.velocity = new Vector2(-Speed, rb.velocity.y);
            spriteRenderer.flipX = true;
            animator.SetBool("Movement", true);
        }
        else
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
            animator.SetBool("Movement", false);
        }
        if(Input.GetKey("space") && CheckGround.isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
        if (CheckGround.isGrounded == false)
        {
            animator.SetBool("Jump", true);
            animator.SetBool("Movement", false);
        }
        else
        {
            animator.SetBool("Jump", false) ;
        }
    }
}
