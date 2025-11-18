using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Bullet : MonoBehaviour
{
    [SerializeField] float speed = 10f;
    Rigidbody2D rb;

    private Vector2 direction;

    public void SetDirection(Vector2 newDirection)
    {
        direction = newDirection;
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        rb.velocity = direction * speed;

        Invoke(nameof(DisableBullet), 3f); 
    }

    private void DisableBullet()
    {
        gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        CancelInvoke();
    }

    private void OnTriggerEnter2D(Collider2D collision) 
    {
        gameObject.SetActive(false);
    }
}

