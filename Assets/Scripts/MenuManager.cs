using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    private Canvas principalCanvas;
    private Canvas optionsCanvas;
    private Canvas creditsCanvas;
    private Text playTxt;
    private Text optionsTxt;
    private Text creditsTxt;
    private Text backTxt;
    private Text back2Txt;
    private Text exitTxt;
    private Text languajeTxt;
    private Text creditsTo;
    private int lastLvlPlayed;
    public bool english;

    private void Awake()
    {
       
    }

    public void GoToLvl(int lvlIndex)
    {
        if (lastLvlPlayed == 0)
        { SceneManager.LoadScene(1); }
        else
        { SceneManager.LoadScene(lvlIndex); }
    }

    public void optionsButton()
    {
        principalCanvas.enabled = false;
        optionsCanvas.enabled = true;
        creditsCanvas.enabled = false;
    }

    public void creditsButton()
    {
        principalCanvas.enabled = false;
        optionsCanvas.enabled = false;
        creditsCanvas.enabled = true;
    }

    public void ExitButton()
    { Application.Quit(); }

    public void backButton()
    {
        principalCanvas.enabled = true;
        optionsCanvas.enabled = false;
        creditsCanvas.enabled = false;
    }

    public void languajeButton()
    {
        if (english)
        {
            english = false;
            playTxt.text = "Play";
            optionsTxt.text = "Options";
            creditsTxt.text = "Credits";
            backTxt.text = "Main Menu";
            back2Txt.text = "Main Menu";
            exitTxt.text = "Exit";
            languajeTxt.text = "Languaje";
        }
        else
        {
            english = true;
            playTxt.text = "Jugar";
            optionsTxt.text = "Opciones";
            creditsTxt.text = "Creditos";
            backTxt.text = "Menú";
            back2Txt.text = "Menú";
            exitTxt.text = "Salir";
            languajeTxt.text = "Lenguaje";
        }
    }
}
