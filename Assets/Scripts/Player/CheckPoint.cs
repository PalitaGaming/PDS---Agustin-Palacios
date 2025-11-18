using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.GMSharedInstance.SaveData(10, other.transform.position.x, other.transform.position.y, other.transform.position.z);

            Debug.Log("Checkpoint alcanzado. Nueva posición guardada: " + other.transform.position); 
        }
    }
}

