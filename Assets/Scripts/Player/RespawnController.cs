using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnController : MonoBehaviour
{
    public float limiteCaidaY = -10f;

    private PlayerController playerController;

    void Start()
    {
        playerController = GetComponent<PlayerController>();

        if (playerController == null)
        {
            enabled = false;
            return;
        }

        if (GameManager.GMSharedInstance != null)
        {
            Vector3 inicio = transform.position;
            GameManager.GMSharedInstance.SaveData(0, inicio.x, inicio.y, inicio.z);
        }
    }

    void FixedUpdate()
    {
        if (transform.position.y < limiteCaidaY)
        {
            PlatformRespawn();
        }
    }

    private void PlatformRespawn()
    {
        Vector3 posicionGuardada = Vector3.zero;

        if (GameManager.GMSharedInstance != null)
        {
            posicionGuardada = GameManager.GMSharedInstance.LoadPositionData();
        }
        else
        {
            posicionGuardada = new Vector3(0, 0, 0);
        }

        playerController.FallDamage(posicionGuardada);

        Debug.Log("Caída detectada por límite");
    }
}