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
        player.rb.velocity = new Vector2(0f, player.rb.velocity.y);
        player.animator.SetBool("Movement", false);
        player.animator.SetBool("Jump", !player.IsGrounded);
    }

    public void FixedUpdate()
    {
        bool inputMoveLeft = Input.GetKey("a") || Input.GetKey("left");
        bool inputMoveRight = Input.GetKey("d") || Input.GetKey("right");
        bool inputJump = Input.GetKey("space");

        if (player.IsGrounded && !inputMoveLeft && !inputMoveRight)
        {
            player.rb.velocity = new Vector2(0f, player.rb.velocity.y);
        }

        if (inputMoveLeft || inputMoveRight)
        {
            player.ChangeState(player.RunState);
        }

        else if (inputJump && player.IsGrounded)
        {
            player.ChangeState(player.JumpState);
        }
        if (Input.GetKey(KeyCode.K))
        {
            player.ChangeState(player.AttackState);
        }
    }

    public void Exit() {}
}