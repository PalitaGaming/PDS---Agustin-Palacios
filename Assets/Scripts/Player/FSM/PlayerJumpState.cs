using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerJumpState : IState
{
    private PlayerController player;
    private const float JumpCutMultiplier = 0.5f;

    public PlayerJumpState(PlayerController player)
    {
        this.player = player;
    }

    public void Enter()
    {
        player.animator.SetBool("Jump", true);
        player.animator.SetBool("Movement", false);

        if (player.IsGrounded)
        {
            player.rb.velocity = new Vector2(player.rb.velocity.x, player.jumpForce);
        }
    }

    public void FixedUpdate()
    {
        bool inputMoveLeft = Input.GetKey("a") || Input.GetKey("left");
        bool inputMoveRight = Input.GetKey("d") || Input.GetKey("right");
        bool inputJump = Input.GetKey("space");

        float horizontalInput = 0;
        if (inputMoveRight) horizontalInput = 1;
        else if (inputMoveLeft) horizontalInput = -1;

        if (horizontalInput != 0)
        {
            player.rb.velocity = new Vector2(horizontalInput * player.Speed, player.rb.velocity.y);
            player.spriteRenderer.flipX = horizontalInput < 0;
        }


        if (!inputJump && player.rb.velocity.y > 0)
        {
            player.rb.velocity = new Vector2(player.rb.velocity.x, player.rb.velocity.y * JumpCutMultiplier);
        }

        if (player.IsGrounded && player.rb.velocity.y <= 0)
        {
            if (Mathf.Abs(player.rb.velocity.x) > 0.05f)
            {
                player.ChangeState(player.RunState);
            }
            else
            {
                player.ChangeState(player.IdleState);
            }
        }
    }

    public void Exit()
    {
        player.animator.SetBool("Jump", false);
    }
}
