using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// WeaponPoolManager.cs


public class WeaponPoolManager : MonoBehaviour
{
    public GameObject Prefab;
    public List<GameObject> ObjectsInPool;
    public float weaponRate = 0.5f;
    [SerializeField] int poolSize = 15;

    float shootTimer;
    [SerializeField] Transform firePoint;

    void Start()
    {
        CreateThings(poolSize); // Llama al Factory al inicio
    }

    private void Update()
    {
        shootTimer -= Time.deltaTime;
    }

    // Patrón FACTORY
    void CreateThings(int amount)
    {
        ObjectsInPool = new List<GameObject>();
        for (int i = 0; i < amount; i++)
        {
            GameObject s = Instantiate(Prefab);
            ObjectsInPool.Add(s);
            s.SetActive(false);
            s.transform.SetParent(this.transform);
        }
    }

    // Patrón OBJECT POOLING
    public void ShootFromPool()
    {
        if (shootTimer > 0) return;

        for (int i = 0; i < ObjectsInPool.Count; i++)
        {
            if (!ObjectsInPool[i].activeSelf)
            {
                GameObject bullet = ObjectsInPool[i];

                // Posición y Rotación: Usamos el FirePoint para la dirección
                bullet.transform.position = firePoint.position;
                bullet.transform.rotation = firePoint.rotation;

                // Asegura que el vector de velocidad del Rigidbody2D apunte correctamente
                // (Esto es manejado en FixedUpdate de Bullet2D, solo activamos)

                bullet.SetActive(true);
                shootTimer = weaponRate;

                break;
            }
        }
    }
}
