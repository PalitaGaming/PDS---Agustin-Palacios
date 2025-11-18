using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamage : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // 1. Comprobar si el objeto colisionado tiene el Tag "Player"
        if (collision.gameObject.CompareTag("Player"))
        {
            // 2. Intentar obtener el componente PlayerController del objeto colisionado
            PlayerController player = collision.gameObject.GetComponent<PlayerController>();

            // 3. Si se encuentra el componente, llamar a la función de daño.
            if (player != null)
            {
                player.TakeDamage(transform);
            }
        }
    }
}
