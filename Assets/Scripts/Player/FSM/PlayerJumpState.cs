using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpState : IState
{
    private PlayerController player;
    // float horizontalInput; // Opcional: Puedes usar GetKey/GetAxisRaw directamente

    public PlayerJumpState(PlayerController player)
    {
        this.player = player;
    }

    public void Enter()
    {
        player.animator.SetBool("Jump", true);
        player.animator.SetBool("Movement", false);

        // Solo aplicar fuerza si está en el suelo al entrar (para saltar)
        if (player.IsGrounded)
        {
            player.rb.velocity = new Vector2(player.rb.velocity.x, player.jumpForce);
        }
        // Si entra porque se cayó de una plataforma, simplemente maneja el movimiento en el aire
    }

    public void FixedUpdate()
    {
        bool inputMoveLeft = Input.GetKey("a") || Input.GetKey("left");
        bool inputMoveRight = Input.GetKey("d") || Input.GetKey("right");

        // 1. Movimiento Horizontal en el Aire (Air Control)
        float horizontalInput = 0;
        if (inputMoveRight) horizontalInput = 1;
        else if (inputMoveLeft) horizontalInput = -1;

        if (horizontalInput != 0)
        {
            player.rb.velocity = new Vector2(horizontalInput * player.Speed, player.rb.velocity.y);
            player.spriteRenderer.flipX = horizontalInput < 0;
        }
        else
        {
            // Si no hay input horizontal, mantener la velocidad X actual, pero sin forzar a 0
            // (el drag de Rigidbody2D se encargará de desacelerar naturalmente).
        }

        // 2. Transición a Aterrizaje
        if (player.IsGrounded && player.rb.velocity.y <= 0)
        {
            if (horizontalInput != 0)
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
