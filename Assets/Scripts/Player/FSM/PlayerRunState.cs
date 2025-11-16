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

        // 1. Lógica de Movimiento
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
            // 2. Transición a Quieto (si el input horizontal se detiene)
            player.ChangeState(player.IdleState);
            return;
        }

        // 3. Transición a Saltar
        if (inputJump && player.IsGrounded)
        {
            player.ChangeState(player.JumpState);
        }
        // 4. Transición a Caer (si se cae de una plataforma)
        else if (!player.IsGrounded && player.rb.velocity.y <= 0)
        {
            player.ChangeState(player.JumpState); // Usamos JumpState para la lógica de aire/caída
        }
    }

    public void Exit() { /* Limpieza */ }
}
