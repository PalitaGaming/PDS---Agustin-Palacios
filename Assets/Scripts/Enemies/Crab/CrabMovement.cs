using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CrabMovement : MonoBehaviour
{

    public float speed = 2f;
    public float waitTime = 1f;
    public Transform[] patrolPoints;


    private int currentPointIndex;
    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        if (patrolPoints.Length == 0)
        {
            Debug.LogError("El enemigo no tiene puntos de patrullaje asignados. ¡Deteniendo patrulla!");
            enabled = false;
            return;
        }

        StartCoroutine(PatrolRoutine());
    }


    private IEnumerator PatrolRoutine()
    {
        while (true)
        {
            Vector2 targetPosition = patrolPoints[currentPointIndex].position;
            animator.SetBool("IsWalking", true);

            while (Vector2.Distance(transform.position, targetPosition) > 0.1f)
            {

                Vector2 moveDirection = (targetPosition - (Vector2)transform.position).normalized;

                transform.Translate(moveDirection * speed * Time.deltaTime, Space.World);
                
                FlipSprite(moveDirection.x);
                yield return null;
            }

            if (rb != null) rb.velocity = Vector2.zero;
            transform.position = targetPosition;
            animator.SetBool("IsWalking", false);

            yield return new WaitForSeconds(waitTime);

            currentPointIndex = (currentPointIndex + 1) % patrolPoints.Length;
        }
    }
    private void FlipSprite(float directionX)
    {
        if (spriteRenderer == null) return; 

        if (directionX < 0.01f)
        {
            
            spriteRenderer.flipX = false;
        }
        else 
        {
            spriteRenderer.flipX = true;
        }
        
    }
}


