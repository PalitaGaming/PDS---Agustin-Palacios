using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerAttackState : IState
{
    private PlayerController player;

    public PlayerAttackState(PlayerController player)
    {
        this.player = player;
    }

    public void Enter()
    {
        player.poolManager.ShootFromPool();

        player.animator.SetBool("Attack", true);

    }

    public void FixedUpdate()
    {
        bool inputMoveLeft = Input.GetKey("a") || Input.GetKey("left");
        bool inputMoveRight = Input.GetKey("d") || Input.GetKey("right");
        bool inputJump = Input.GetKey("space");
        bool inputAttack = Input.GetKey(KeyCode.K);
        float horizontalInput = 0;

        if (inputMoveRight) horizontalInput = 1;
        else if (inputMoveLeft) horizontalInput = -1;

        if (inputAttack)
        {
            player.poolManager.ShootFromPool();
        }

        if (player.IsGrounded && horizontalInput == 0)
        {
            player.rb.velocity = new Vector2(0f, player.rb.velocity.y);
            player.animator.SetBool("Movement", false);
        }
        else if (horizontalInput != 0)
        {
            player.rb.velocity = new Vector2(horizontalInput * player.Speed, player.rb.velocity.y);
            player.spriteRenderer.flipX = horizontalInput < 0;
            player.animator.SetBool("Movement", true);
        }

      
        if (!inputAttack)
        {
            player.animator.SetBool("Attack", false);

            if (player.IsGrounded)
            {
                if (inputMoveLeft || inputMoveRight)
                    player.ChangeState(player.RunState);
                else
                    player.ChangeState(player.IdleState);
            }
            else
            {
                player.ChangeState(player.JumpState);
            }
        }
    }

    public void Exit()
    {
        player.animator.SetBool("Attack", false);
    }
}
