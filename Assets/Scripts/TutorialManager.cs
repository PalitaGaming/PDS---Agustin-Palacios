using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialManager : MonoBehaviour
{
    [Header("Referencias UI")]
    public GameObject panelTutorial;
    public Text textoTutorial;

    [Header("Configuración")]
    [TextArea(5, 10)]
    public string mensajeTutorial;
    public float velocidadEscritura = 0.05f;

    private bool escribiendo = false;

    void Start()
    {
        Time.timeScale = 0f;

        if (string.IsNullOrEmpty(mensajeTutorial))
        {
            mensajeTutorial = "OBJETIVO: Llega al final con vida.\n\nCONTROLES:\n[A/D]: Moverse\n[ESPACIO]: Saltar\n[K]: Atacar\n\nPresiona ESPACIO para comenzar.";
        }

        panelTutorial.SetActive(true);
        textoTutorial.text = "";

        StartCoroutine(EscribirTexto());
    }

    void Update()
    {
        if (!escribiendo && Input.GetKeyDown(KeyCode.Space))
        {
            CerrarTutorial();
        }
        else if (escribiendo && Input.GetKeyDown(KeyCode.Space))
        {
            StopAllCoroutines();
            textoTutorial.text = mensajeTutorial + "\n\n<size=20>Presiona [ESPACIO] para jugar</size>";
            escribiendo = false;
        }
    }

    IEnumerator EscribirTexto()
    {
        escribiendo = true;
        foreach (char letra in mensajeTutorial.ToCharArray())
        {
            textoTutorial.text += letra;
            yield return new WaitForSecondsRealtime(velocidadEscritura);
        }

        textoTutorial.text += "\n\n<size=20>Presiona [ESPACIO] para jugar</size>";
        escribiendo = false;
    }

    public void CerrarTutorial()
    {
        panelTutorial.SetActive(false);
        Time.timeScale = 1f;
    }
}