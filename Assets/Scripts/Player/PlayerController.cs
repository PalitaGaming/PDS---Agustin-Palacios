using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // --- PROPIEDADES PÚBLICAS Y COMPONENTES ---
    public float Speed = 2f;
    public float jumpForce = 3f;
    public SpriteRenderer spriteRenderer;
    public Animator animator;
    public WeaponPoolManager poolManager;

    public Rigidbody2D rb { get; private set; }

    public bool IsGrounded => CheckGround.isGrounded;

    private IState currentState;
    public IState CurrentState => currentState;

    public PlayerIdleState IdleState { get; private set; }
    public PlayerRunState RunState { get; private set; }
    public PlayerJumpState JumpState { get; private set; }
    public PlayerAttackState AttackState { get; private set; }


    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        IdleState = new PlayerIdleState(this);
        RunState = new PlayerRunState(this);
        JumpState = new PlayerJumpState(this);
        AttackState = new PlayerAttackState(this);
    }

    void Start()
    {
        if (IsGrounded)
        {
            ChangeState(IdleState);
        }
        else
        {
            ChangeState(JumpState);
        }
    }

    void FixedUpdate()
    {
        currentState?.FixedUpdate();
    }

    public void ChangeState(IState newState)
    {
        currentState?.Exit();
        currentState = newState;
        currentState.Enter();
    }

    public void ResetStateForRespawn()
    {
        rb.velocity = Vector2.zero;
        rb.angularVelocity = 0f;

        ChangeState(IdleState);

        animator.Play("IdlePlayer", 0, 0f);
    }
}
