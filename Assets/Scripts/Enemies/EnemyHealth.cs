using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int maxHealth = 10;
    private int currentHealth;


    void Start()
    {
        currentHealth = maxHealth;
    }

    public void TakeDamage(int damageAmount)
    {
        if (currentHealth <= 0) return;

        currentHealth -= damageAmount;
        Debug.Log(gameObject.name + " Vida restante del enemigo: " + currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log(gameObject.name + " Enemigo derrotado y destruido.");

        CrabMovement crabMovement = GetComponent<CrabMovement>();
        if (crabMovement != null)
        {
            crabMovement.enabled = false;
        }

        EnemyDamage enemyDamage = GetComponent<EnemyDamage>();
        if (enemyDamage != null)
        {
            enemyDamage.enabled = false;
        }

        Collider2D col = GetComponent<Collider2D>();
        if (col != null) col.enabled = false;

        Destroy(gameObject);
    }
}