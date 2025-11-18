using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour

{
    public static GameManager GMSharedInstance;

    AudioSource audioSource;

    [SerializeField] AudioClip[] clips;

    private void Awake()

    {

        GMSharedInstance = this;

        DontDestroyOnLoad(gameObject);

    }

    private void Start()

    {
        audioSource = GetComponent<AudioSource>();

        MusicLogic();
    }

    public void SaveData(int score, float posX, float posY, float posZ)

    {
        PlayerPrefs.SetInt("score", score);

        PlayerPrefs.SetFloat("posX", posX);

        PlayerPrefs.SetFloat("posY", posY);

        PlayerPrefs.SetFloat("posZ", posZ);

    }

    public void LoadData()

    {

        Debug.Log("Score=" + PlayerPrefs.GetInt("Score"));

        Debug.Log("X=" + PlayerPrefs.GetFloat("posX"));

        Debug.Log("Y=" + PlayerPrefs.GetFloat("posY"));

        Debug.Log("Z=" + PlayerPrefs.GetFloat("posZ"));

    }

    public Vector3 LoadPositionData()
    {
        float posX = PlayerPrefs.GetFloat("posX");
        float posY = PlayerPrefs.GetFloat("posY");
        float posZ = PlayerPrefs.GetFloat("posZ");

        return new Vector3(posX, posY, posZ);
    }


    //---------SFX------------



    public void AddSoundVolume()

    {

        audioSource.volume += 0.1f;

    }



    public void SubstractSoundVolume()

    {

        audioSource.volume -= 0.1f;

    }

    public void MusicLogic()

    {
        audioSource.Stop();

        audioSource.clip = clips[SceneManager.GetActiveScene().buildIndex];

        audioSource.Play();
    }

    public void DangerMomentSFX()

    { audioSource.PlayOneShot(clips[3]); }

}
