using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerJumpState : IState
{
    private PlayerController player;
    private const float JumpCutMultiplier = 0.5f; // Factor para cortar el salto

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
    }

    public void FixedUpdate()
    {
        bool inputMoveLeft = Input.GetKey("a") || Input.GetKey("left");
        bool inputMoveRight = Input.GetKey("d") || Input.GetKey("right");
        bool inputJump = Input.GetKey("space"); // Necesitas verificar la tecla de salto aquí

        // 1. Control Horizontal en el Aire (¡MODIFICADO!)
        float horizontalInput = 0;
        if (inputMoveRight) horizontalInput = 1;
        else if (inputMoveLeft) horizontalInput = -1;

        if (horizontalInput != 0)
        {
            // Aplica la nueva velocidad horizontal y voltea el sprite
            player.rb.velocity = new Vector2(horizontalInput * player.Speed, player.rb.velocity.y);
            player.spriteRenderer.flipX = horizontalInput < 0;
        }
        // *******************************************************************
        // CAMBIO CLAVE: Cuando horizontalInput es 0, no hacemos nada con rb.velocity.x.
        // El Rigidbody2D mantiene la velocidad horizontal existente (inercia) 
        // y el Drag (fricción) en el Rigidbody lo desacelerará lentamente, 
        // lo que simula la capacidad de "quedarse quieto" o mantener la inercia en el aire.
        // *******************************************************************

        // 2. Control de Altura del Salto (SALTO CORTO / Caída Controlada)
        // Condición: Si el jugador suelta la tecla 'space' Y el personaje aún está subiendo (rb.velocity.y > 0)
        if (!inputJump && player.rb.velocity.y > 0)
        {
            // Reduce la velocidad vertical bruscamente para cortar el salto.
            player.rb.velocity = new Vector2(player.rb.velocity.x, player.rb.velocity.y * JumpCutMultiplier);
        }

        // 3. Transición a Aterrizaje
        if (player.IsGrounded && player.rb.velocity.y <= 0)
        {
            // Nota: Aquí ya no usamos 'horizontalInput' para la transición, 
            // sino la velocidad horizontal real del cuerpo para decidir si ir a Run o Idle.
            if (Mathf.Abs(player.rb.velocity.x) > 0.05f) // Si aún hay movimiento horizontal significativo
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
