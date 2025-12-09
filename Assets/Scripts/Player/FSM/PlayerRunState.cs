using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRunState : IState
{
    private PlayerController player;

    public PlayerRunState(PlayerController player)
    {
        this.player = player;
    }

    public void Enter()
    {
        player.animator.SetBool("Movement", true);
        player.animator.SetBool("Jump", false);
    }

    public void FixedUpdate()
    {
        bool inputMoveLeft = Input.GetKey("a") || Input.GetKey("left");
        bool inputMoveRight = Input.GetKey("d") || Input.GetKey("right");
        bool inputJump = Input.GetKey("space");

        if (inputMoveRight)
        {
            player.rb.velocity = new Vector2(player.Speed, player.rb.velocity.y);
            player.spriteRenderer.flipX = false;
        }
        else if (inputMoveLeft)
        {
            player.rb.velocity = new Vector2(-player.Speed, player.rb.velocity.y);
            player.spriteRenderer.flipX = true;
        }
        else
        {
            player.ChangeState(player.IdleState);
            return;
        }

        if (inputJump && player.IsGrounded)
        {
            player.ChangeState(player.JumpState);
        }

        else if (!player.IsGrounded && player.rb.velocity.y <= 0)
        {
            player.ChangeState(player.JumpState);
        }

        if (Input.GetKey(KeyCode.K))
        {
            player.ChangeState(player.AttackState);
        }
    }

    public void Exit() { }
}
