using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class WeaponPoolManager : MonoBehaviour
{
    [Header("References")]
    public Transform firePoint;
    public GameObject bulletPrefab;
    public SpriteRenderer playerSpriteRenderer;

    [Header("Pooling")]
    public int poolSize = 10;

    [Header("Weapon Settings")]
    public float fireRate = 0.2f;
    private float fireCooldown = 0f;

    private List<GameObject> pool;

    private void Awake()
    {
        pool = new List<GameObject>();

        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(bulletPrefab);
            obj.SetActive(false);
            pool.Add(obj);
        }
    }

    private void Update()
    {
        fireCooldown -= Time.deltaTime;
    }

    public void ShootFromPool()
    {
        if (fireCooldown > 0f) return;

        foreach (GameObject bullet in pool)
        {
            if (!bullet.activeSelf)
            {
                bullet.transform.position = firePoint.position;


                Vector2 playerDirection = firePoint.right;

                if (playerSpriteRenderer.flipX)
                {
                    playerDirection = Vector2.right;
                }
                else
                {
                    playerDirection = Vector2.left;
                }

                Vector2 oppositeDirection = -playerDirection; // <-- Usamos la dirección opuesta

                float angle = Mathf.Atan2(oppositeDirection.y, oppositeDirection.x) * Mathf.Rad2Deg;
                bullet.transform.rotation = Quaternion.Euler(0f, 0f, angle);

                Bullet bulletScript = bullet.GetComponent<Bullet>();
                bulletScript.SetDirection(oppositeDirection);

                bullet.SetActive(true);
                fireCooldown = fireRate;
                return;
            }
        }
    }
}

