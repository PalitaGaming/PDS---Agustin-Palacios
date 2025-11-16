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

    // --- COMPONENTES PRIVADOS ACCESIBLES ---
    public Rigidbody2D rb { get; private set; }

    // Propiedad Helper para el suelo (asumiendo que 'CheckGround' es un script estático o accesible)
    public bool IsGrounded => CheckGround.isGrounded;

    // --- FSM PROPIEDADES ---
    private IState currentState;
    public IState CurrentState => currentState;

    // Instancias de estados (reutilizables)
    public PlayerIdleState IdleState { get; private set; }
    public PlayerRunState RunState { get; private set; }
    public PlayerJumpState JumpState { get; private set; }

    // Asumiendo que CheckGround.isGrounded existe y funciona para 2D.

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        // Inicializar todas las instancias de estado
        IdleState = new PlayerIdleState(this);
        RunState = new PlayerRunState(this);
        JumpState = new PlayerJumpState(this);
    }

    void Start()
    {
        // 1. Verificar la condición inicial del suelo
        if (IsGrounded)
        {
            // Si está en el suelo, establece el estado inicial como IDLE.
            ChangeState(IdleState);
        }
        else
        {
            // Si NO está en el suelo (cae desde arriba), establece el estado inicial como JUMP.
            // Esto asegura que la animación de salto/caída se active inmediatamente.
            // Nota: Usamos JumpState para la lógica de caída en el aire.
            ChangeState(JumpState);
        }
    }

    void FixedUpdate()
    {
        // Delegamos la lógica al estado actual
        currentState?.FixedUpdate();
    }

    // Método clave para la transición
    public void ChangeState(IState newState)
    {
        currentState?.Exit();
        currentState = newState;
        currentState.Enter();
    }
}
