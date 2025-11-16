using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerIdleState : IState
{
    private PlayerController player;

    public PlayerIdleState(PlayerController player)
    {
        this.player = player;
    }

    public void Enter()
    {
        // Lógica al entrar: Detener movimiento horizontal
        player.rb.velocity = new Vector2(0f, player.rb.velocity.y);
        player.animator.SetBool("Movement", false);
        player.animator.SetBool("Jump", !player.IsGrounded); // Podría entrar aquí si aterriza sin input
    }

    public void FixedUpdate()
    {
        // Obtener input para verificar transiciones
        bool inputMoveLeft = Input.GetKey("a") || Input.GetKey("left");
        bool inputMoveRight = Input.GetKey("d") || Input.GetKey("right");
        bool inputJump = Input.GetKey("space");

        // Transición a Correr
        if (inputMoveLeft || inputMoveRight)
        {
            player.ChangeState(player.RunState);
        }
        // Transición a Saltar
        else if (inputJump && player.IsGrounded)
        {
            player.ChangeState(player.JumpState);
        }
        // Transición a Caer (implícito si ya no está en el suelo)
        else if (!player.IsGrounded)
        {
            // Opcional: Podrías cambiar a un estado 'FallingState' si necesitas lógica específica para la caída.
            // Por ahora, asumimos que se gestiona la lógica de caer dentro del JumpState.
        }
    }

    public void Exit() { /* Limpieza */ }
}
