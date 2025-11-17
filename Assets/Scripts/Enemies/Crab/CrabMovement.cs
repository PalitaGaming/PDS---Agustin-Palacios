using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPatrolEdge : MonoBehaviour
{
    [Header("Movimiento")]
    public float speed = 2f;
    public bool movingRight = true;

    [Header("Detección de bordes")]
    public Transform groundCheck;
    public float groundCheckDistance = 1f;
    public LayerMask groundLayer;

    [Header("Componentes")]
    public SpriteRenderer spriteRenderer;
    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        Patrol();
        EdgeDetection();
    }

    void Patrol()
    {
        // Movimiento horizontal
        float dir = movingRight ? 1f : -1f;
        rb.velocity = new Vector2(dir * speed, rb.velocity.y);

        // Rotación visual del sprite
        spriteRenderer.flipX = !movingRight;
    }

    void EdgeDetection()
    {
        // Raycast hacia abajo desde groundCheck
        RaycastHit2D hit = Physics2D.Raycast(
            groundCheck.position,
            Vector2.down,
            groundCheckDistance,
            groundLayer
        );

        // SI NO HAY PISO = doy la vuelta
        if (hit.collider == null)
        {
            Flip();
        }

        // Raycast frontal para detectar paredes
        RaycastHit2D wallHit = Physics2D.Raycast(
            groundCheck.position,
            movingRight ? Vector2.right : Vector2.left,
            0.2f,
            groundLayer
        );

        if (wallHit.collider != null)
        {
            Flip();
        }
    }

    void Flip()
    {
        movingRight = !movingRight;
    }

    private void OnDrawGizmosSelected()
    {
        if (groundCheck == null) return;

        // Gizmo raycast piso
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(groundCheck.position,
                        groundCheck.position + Vector3.down * groundCheckDistance);

        // Gizmo raycast pared
        Gizmos.color = Color.red;
        Vector3 dir = movingRight ? Vector3.right : Vector3.left;
        Gizmos.DrawLine(groundCheck.position, groundCheck.position + dir * 0.2f);
    }
}


