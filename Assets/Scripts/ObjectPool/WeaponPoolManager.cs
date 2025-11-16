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

                    // LA ROTACIÓN CORRECTA:
                    bullet.transform.rotation = firePoint.rotation;

                    bullet.SetActive(true);
                    fireCooldown = fireRate;
                    return;
                }
            }
        }
 }

