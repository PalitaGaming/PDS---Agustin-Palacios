using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour
{
    [Header("Referencias")]
    public AudioSource musicSource;

    [Header("Clips de Audio")]
    [Tooltip("El Elemento 0 es Nivel 0, etc.")]
    [SerializeField] AudioClip[] clips;
    [SerializeField] AudioClip deathClip;

    [Header("Timer Logic (Opcional)")]
    private float clip0Duration;

    private void Awake()
    {
        if (musicSource == null) musicSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        EventByCronoMusic();
    }

    public void InitializeMusic()
    {
        int sceneIndex = SceneManager.GetActiveScene().buildIndex;

        if (sceneIndex < clips.Length && clips[sceneIndex] != null)
        {
            musicSource.clip = clips[sceneIndex];
            musicSource.loop = true;
            musicSource.Play();

            if (sceneIndex == 0) clip0Duration = clips[0].length;
        }
    }

    public void PlayDeathSound()
    {
        musicSource.Stop();
        if (deathClip != null)
        {
            musicSource.PlayOneShot(deathClip);
        }
    }

    public void ModifyVolume(float amount)
    {
        if (musicSource != null)
        {
            musicSource.volume += amount;
            musicSource.volume = Mathf.Clamp01(musicSource.volume);
        }
    }

    void EventByCronoMusic()
    {
        if (musicSource.clip == clips[0])
        {
            clip0Duration -= Time.deltaTime;
            if (clip0Duration <= 0)
            {
                Debug.Log("Ejecuta evento de música");
                clip0Duration = clips[0].length;
            }
        }
    }
}
