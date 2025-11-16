using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// PlayerAttackState.cs


public class PlayerAttackState : IState
{
    private PlayerController player;
    // Ya no necesitas 'attackDuration' ni 'timer' si el loop lo controla el Bool.

    public PlayerAttackState(PlayerController player)
    {
        this.player = player;
    }

    public void Enter()
    {
        // 1. Ejecutar Disparo
        player.poolManager.ShootFromPool();

        // 2. ACTIVAR EL BOOL para que la animación se quede en loop
        player.animator.SetBool("Attack", true);

        // El Trigger "Attack" ya no se usa aquí.
    }

    public void FixedUpdate()
    {
        bool inputMoveLeft = Input.GetKey("a") || Input.GetKey("left");
        bool inputMoveRight = Input.GetKey("d") || Input.GetKey("right");
        bool inputJump = Input.GetKey("space");
        bool inputAttack = Input.GetKey(KeyCode.K); // La tecla que mantiene el ataque
        float horizontalInput = 0;

        if (inputMoveRight) horizontalInput = 1;
        else if (inputMoveLeft) horizontalInput = -1;

        // 1. DISPARO CONTINUO: Siempre intentamos disparar mientras estamos en este estado.
        if (inputAttack)
        {
            player.poolManager.ShootFromPool();
        }

        if (player.IsGrounded && horizontalInput == 0)
        {
            // Forzar la detención y la animación de quieto
            player.rb.velocity = new Vector2(0f, player.rb.velocity.y);
            player.animator.SetBool("Movement", false);
        }
        else if (horizontalInput != 0)
        {
            // Control de movimiento
            player.rb.velocity = new Vector2(horizontalInput * player.Speed, player.rb.velocity.y);
            player.spriteRenderer.flipX = horizontalInput < 0;
            player.animator.SetBool("Movement", true);
        }

        // 3. Transición de Salida (CLAVE)
        // Solo salimos si la tecla de ataque NO está pulsada.
        if (!inputAttack)
        {
            // DESACTIVAR EL BOOL al salir para que la animación termine.
            player.animator.SetBool("Attack", false);

            // ... (Tu lógica de ChangeState a RunState/IdleState/JumpState)
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
        // Aseguramos que el BOOL se apague por si hay salidas inesperadas
        player.animator.SetBool("Attack", false);
    }
}
