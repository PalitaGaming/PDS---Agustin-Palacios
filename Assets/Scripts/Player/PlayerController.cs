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

    // --- PROPIEDADES DE DAÑO Y KNOCKBACK (NUEVAS) ---
    public float knockbackForce = 5f;       // Fuerza con la que el jugador es empujado
    public float blinkDuration = 1f;        // Duración total de la invulnerabilidad/parpadeo
    public float blinkInterval = 0.1f;      // Intervalo de tiempo entre cada parpadeo
    public Color damageColor = Color.red;   // Color opcional para el parpadeo

    // --- ESTADO INTERNO DE DAÑO (NUEVAS) ---
    public bool IsInvulnerable { get; private set; } = false;
    private Coroutine blinkCoroutine;
    private Color originalColor;

    // --- COMPONENTES PRIVADOS ACCESIBLES ---
    public Rigidbody2D rb { get; private set; }

    public bool IsGrounded => CheckGround.isGrounded;

    // --- FSM PROPIEDADES ---
    private IState currentState;
    public IState CurrentState => currentState;

    // Instancias de estados (reutilizables)
    public PlayerIdleState IdleState { get; private set; }
    public PlayerRunState RunState { get; private set; }
    public PlayerJumpState JumpState { get; private set; }
    public PlayerAttackState AttackState { get; private set; }


    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        // Asegurarse de obtener el SpriteRenderer si no fue asignado en el Inspector
        if (spriteRenderer == null)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        if (spriteRenderer != null)
        {
            originalColor = spriteRenderer.color; // Guardar el color original
        }

        // Inicializar todas las instancias de estado
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

    // --- FUNCIÓN DE DAÑO (NUEVA) ---
    /// <summary>
    /// Aplica retroceso, animación de daño e inicia el efecto de invulnerabilidad.
    /// </summary>
    /// <param name="enemyTransform">La posición del enemigo que causó el daño.</param>
    public void TakeDamage(Transform enemyTransform)
    {
        if (IsInvulnerable) return; // Si es invulnerable, ignorar el daño

        IsInvulnerable = true;

        // 1. Reproducir animación de daño
        animator.SetTrigger("TakeDamage"); // **Asegúrate de que este nombre sea exacto**

        // 2. Aplicar retroceso (Knockback)
        // Calcula la dirección opuesta al enemigo
        Vector2 knockbackDirection = (transform.position - enemyTransform.position).normalized;
        rb.velocity = Vector2.zero; // Limpiar velocidad actual
        rb.AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse);

        // 3. Iniciar el parpadeo
        if (blinkCoroutine != null)
        {
            StopCoroutine(blinkCoroutine);
        }
        blinkCoroutine = StartCoroutine(BlinkEffect());
    }

    /// <summary>
    /// Corutina para el efecto de parpadeo y gestión de invulnerabilidad.
    /// </summary>
    private IEnumerator BlinkEffect()
    {
        float timer = 0f;
        while (timer < blinkDuration)
        {
            // Alternar visibilidad (parpadeo)
            if (spriteRenderer != null)
            {
                spriteRenderer.enabled = !spriteRenderer.enabled;
            }

            yield return new WaitForSeconds(blinkInterval);
            timer += blinkInterval;
        }

        // Asegurarse de que el sprite sea visible y con el color original al finalizar
        if (spriteRenderer != null)
        {
            spriteRenderer.enabled = true;
            spriteRenderer.color = originalColor;
        }

        IsInvulnerable = false; // Desactivar invulnerabilidad

        // Regresar inmediatamente al estado Idle si no está en el suelo.
        if (IsGrounded || rb.velocity.y < 0.1f)
        {
            ChangeState(IdleState);
        }
    }

    // --- FUNCIÓN DE RESPAWN (Existente, con ajustes de invulnerabilidad) ---
    public void ResetStateForRespawn()
    {
        rb.velocity = Vector2.zero;
        rb.angularVelocity = 0f;

        ChangeState(IdleState);

        // Asegurarse de que el estado de daño/parpadeo se detiene al reaparecer
        if (blinkCoroutine != null)
        {
            StopCoroutine(blinkCoroutine);
        }
        IsInvulnerable = false;
        if (spriteRenderer != null)
        {
            spriteRenderer.enabled = true;
            spriteRenderer.color = originalColor;
        }

        animator.Play("IdlePlayer", 0, 0f);
    }
}