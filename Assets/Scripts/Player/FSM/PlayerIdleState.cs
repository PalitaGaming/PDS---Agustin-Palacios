// PlayerIdleState.cs (Modificado)
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
        // En Enter, aseguras que inicie quieto, pero el FixedUpdate debe mantenerlo quieto.
        player.rb.velocity = new Vector2(0f, player.rb.velocity.y);
        player.animator.SetBool("Movement", false);
        player.animator.SetBool("Jump", !player.IsGrounded);
    }

    public void FixedUpdate()
    {
        // Obtener input para verificar transiciones
        bool inputMoveLeft = Input.GetKey("a") || Input.GetKey("left");
        bool inputMoveRight = Input.GetKey("d") || Input.GetKey("right");
        bool inputJump = Input.GetKey("space");

        // --- MODIFICACIÓN CLAVE ---
        // Si no hay input de movimiento Y está en el suelo, forzar rb.velocity.x a 0.
        // Esto previene el deslizamiento en rampas.
        if (player.IsGrounded && !inputMoveLeft && !inputMoveRight)
        {
            player.rb.velocity = new Vector2(0f, player.rb.velocity.y);
        }
        // --------------------------

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
        if (Input.GetKey(KeyCode.K)) // Presionando K para atacar
        {
            player.ChangeState(player.AttackState);
        }
    }

    public void Exit() { /* Limpieza */ }
}