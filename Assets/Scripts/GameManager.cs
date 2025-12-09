using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager GMSharedInstance;
    AudioSource audioSource;

    [Header("Audio Config")]
    [Tooltip("Arrastra aquí la música de fondo. El Elemento 0 es para el Nivel 0, etc.")]
    [SerializeField] AudioClip[] clips;
    public AudioClip deathClip;

    [Header("Interfaz de Usuario (UI)")]
    public GameObject gameOverPanel;
    public GameObject victoryPanel;

    public bool isGameOver = false;

    private void Awake()
    {
        if (GMSharedInstance == null)
        {
            GMSharedInstance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();

        Time.timeScale = 1f;
        isGameOver = false;

        if (gameOverPanel != null) gameOverPanel.SetActive(false);
        if (victoryPanel != null) victoryPanel.SetActive(false);

        MusicLogic();
    }

    public void SaveData(int score, float posX, float posY, float posZ)
    {
        PlayerPrefs.SetInt("score", score);
        PlayerPrefs.SetFloat("posX", posX);
        PlayerPrefs.SetFloat("posY", posY);
        PlayerPrefs.SetFloat("posZ", posZ);
    }

    public Vector3 LoadPositionData()
    {
        return new Vector3(PlayerPrefs.GetFloat("posX"), PlayerPrefs.GetFloat("posY"), PlayerPrefs.GetFloat("posZ"));
    }

    public void AddSoundVolume() { if (audioSource) audioSource.volume += 0.1f; }
    public void SubstractSoundVolume() { if (audioSource) audioSource.volume -= 0.1f; }

    public void MusicLogic()
    {
        if (audioSource == null) return;

        audioSource.Stop();

        if (SceneManager.GetActiveScene().buildIndex < clips.Length)
        {
            audioSource.clip = clips[SceneManager.GetActiveScene().buildIndex];
            audioSource.loop = true;
            audioSource.Play();
        }
    }

    public void GameOver()
    {
        if (isGameOver) return;
        isGameOver = true;

        Debug.Log("GAME OVER");

        if (audioSource != null) audioSource.Stop();

        if (deathClip != null && audioSource != null)
        {
            audioSource.PlayOneShot(deathClip);
        }

        Time.timeScale = 0f;

        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }
    }

    public void Victory()
    {
        if (isGameOver) return;
        isGameOver = true;
        Debug.Log("VICTORIA");
        Time.timeScale = 0f;
        if (victoryPanel != null) victoryPanel.SetActive(true);
    }

    public void RestartLevel()
    {
        Time.timeScale = 1f;
        isGameOver = false;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void BackToMenu()
    {
        Time.timeScale = 1f;
        isGameOver = false;
        SceneManager.LoadScene("MainMenu");
    }
}