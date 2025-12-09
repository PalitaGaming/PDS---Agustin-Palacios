using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float Speed = 4f;
    public float jumpForce = 8f;
    public SpriteRenderer spriteRenderer;
    public Animator animator;
    public WeaponPoolManager poolManager;

    public float knockbackForce = 10f;
    public float blinkDuration = 1f;
    public float blinkInterval = 0.1f;
    public Color damageColor = Color.red;

    public int maxLives = 3;
    [SerializeField] private int currentLives;
    public int CurrentLives => currentLives;

    public bool IsInvulnerable { get; private set; } = false;
    private Coroutine blinkCoroutine;
    private Color originalColor;

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

        if (spriteRenderer == null)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        if (spriteRenderer != null)
        {
            originalColor = spriteRenderer.color;
        }

        IdleState = new PlayerIdleState(this);
        RunState = new PlayerRunState(this);
        JumpState = new PlayerJumpState(this);
        AttackState = new PlayerAttackState(this);
        currentLives = maxLives;
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

    public void TakeDamage(Transform enemyTransform)
    {
        if (IsInvulnerable || currentLives <= 0) return;

        currentLives--;
        Debug.Log("Vidas restantes: " + currentLives);

        if (currentLives > 0)
        {
            IsInvulnerable = true;

            animator.SetTrigger("TakeDamage");

            Vector2 knockbackDirection = (transform.position - enemyTransform.position).normalized;
            rb.velocity = Vector2.zero;
            rb.AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse);

            if (blinkCoroutine != null) StopCoroutine(blinkCoroutine);
            blinkCoroutine = StartCoroutine(BlinkEffect());
        }
        else
        {
            Die();
        }
    }

    public void FallDamage(Vector3 respawnPosition)
    {
        if (currentLives <= 0) return;

        currentLives--;
        Debug.Log("El jugador se cayó al vacío. Vidas: " + currentLives);

        if (currentLives > 0)
        {
            transform.position = respawnPosition;

            ResetStateForRespawn();


            IsInvulnerable = true;

            if (blinkCoroutine != null) StopCoroutine(blinkCoroutine);
            blinkCoroutine = StartCoroutine(BlinkEffect());
        }
        else
        {
            Die();
        }
    }

    private IEnumerator BlinkEffect()
    {
        float timer = 0f;
        while (timer < blinkDuration)
        {
            if (spriteRenderer != null)
            {
                spriteRenderer.enabled = !spriteRenderer.enabled;
            }

            yield return new WaitForSeconds(blinkInterval);
            timer += blinkInterval;
        }

        if (spriteRenderer != null)
        {
            spriteRenderer.enabled = true;
            spriteRenderer.color = originalColor;
        }

        IsInvulnerable = false;

        if (IsGrounded || rb.velocity.y < 0.1f)
        {
            ChangeState(IdleState);
        }
    }

    private void Die()
    {
        Debug.Log("Game Over. Vidas agotadas.");
        rb.velocity = Vector2.zero;

        if (GameManager.GMSharedInstance != null)
        {
            GameManager.GMSharedInstance.GameOver();
        }

        this.enabled = false;
    }

    public void ResetStateForRespawn()
    {
        rb.velocity = Vector2.zero;
        rb.angularVelocity = 0f;

        ChangeState(IdleState);

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