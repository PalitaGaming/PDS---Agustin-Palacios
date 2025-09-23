using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrabMovement : MonoBehaviour
{
    public float radio;
    public LayerMask LayerPlayer;
    public Transform transformPlayer;
    public float moveSpeed;
    public float maxDistance;
    public Vector3 initialPoint;
    public bool turnRight;
    public Rigidbody2D rb;
    public Animator animator;

    public StatesMovement actualState;

    public enum StatesMovement
    {
        Waiting,
        Following,
        Returning,
    }

    private void Start()
    {
        initialPoint = transform.position;
    }

    private void Update()
    {
        switch (actualState)
        {
            case StatesMovement.Waiting:
                WaitingState();
                break;
            case StatesMovement.Following:
                FollowingState();
                break;
            case StatesMovement.Returning:
                ReturningState();
                break;
        }
           
    }

    void WaitingState()
    {
        Collider2D playerCollider = Physics2D.OverlapCircle(transform.position, radio, LayerPlayer);

        if (playerCollider)
        {
            transformPlayer = playerCollider.transform;
            actualState = StatesMovement.Following;
        }
    }
    void FollowingState()
    {
        animator.SetBool("CrabMovement", true);
        if (transformPlayer == null)
        {
            actualState = StatesMovement.Returning;
            return;
        }

        if(transformPlayer.position.x < transformPlayer.position.x)
        {
            rb.velocity = new Vector2(moveSpeed, rb.velocity.y);
        }
        else
        {
            rb.velocity = new Vector2(-moveSpeed, rb.velocity.y);
        }

        TurnObjective(transformPlayer.position);

        if (Vector2.Distance(transform.position, initialPoint) > maxDistance || Vector2.Distance(transform.position, transformPlayer.position) > maxDistance)
        {
            actualState = StatesMovement.Returning;
            transformPlayer = null;
        }
    }

    void ReturningState()
    {
        if (transformPlayer.position.x < initialPoint.x)
        {
            rb.velocity = new Vector2(moveSpeed, rb.velocity.y);
        }
        else
        {
            rb.velocity = new Vector2(-moveSpeed, rb.velocity.y);
        }

        TurnObjective(initialPoint);

        if(Vector2.Distance(transform.position, initialPoint) < 0.1f)
        {
            rb.velocity = Vector2.zero;
            animator.SetBool("CrabMovement", false);
            actualState = StatesMovement.Waiting;
        }
    }

    private void TurnObjective(Vector3 objective)
    {
        if(objective.x > transformPlayer.position.x && !turnRight)
        {
            Turn();
        }
        else if(objective.x < transformPlayer.position.x && turnRight)
        {
            Turn();
        }
    }

    private void Turn()
    {
        turnRight = !turnRight;
        transformPlayer.eulerAngles = new Vector3(0, transformPlayer.eulerAngles.y + 100, 0);
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radio);
        Gizmos.DrawWireSphere(initialPoint, maxDistance);
    }
}
