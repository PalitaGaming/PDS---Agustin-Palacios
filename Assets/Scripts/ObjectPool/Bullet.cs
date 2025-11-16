using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Bullet2D.cs

public class Bullet : MonoBehaviour
{
    [SerializeField] float speed = 10f;
    Rigidbody2D rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        // Usa transform.right (la dirección local) para el movimiento 2D.
        rb.velocity = transform.right * speed;
    }

    // Usa OnTriggerEnter2D para detectar colisiones 2D
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Desactivar y devolver al pool
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        // Desactivar la bala automáticamente después de 3 segundos (para que no se pierdan)
        Invoke("DisableBullet", 3f);
    }

    private void DisableBullet()
    {
        if (gameObject.activeSelf)
        {
            gameObject.SetActive(false);
        }
    }

    private void OnDisable()
    {
        CancelInvoke("DisableBullet");
    }
}
