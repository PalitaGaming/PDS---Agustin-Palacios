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
            Debug.LogError("ControladorRespawn requiere un componente PlayerController en el mismo GameObject.");
            enabled = false;
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
        Vector3 posicionGuardada = GameManager.GMSharedInstance.LoadPositionData();

        transform.position = posicionGuardada;

        playerController.ResetStateForRespawn();

        Debug.Log("¡Jugador reaparecido y estado de FSM reiniciado!");
    }
}

