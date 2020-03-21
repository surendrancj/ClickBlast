using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance
    {
        get { return _instance; }
        private set { _instance = value; }
    }
    void Awake()
    {
        if (Instance == null)
        {
            DontDestroyOnLoad(this.gameObject);
            Instance = this;
        }
        else
        {
            Destroy(this.gameObject);
            return;
        }
    }

    public bool splashDone = false;
    [SerializeField] AudioSource fxAudioSource;
    [SerializeField] AudioSource bgAudioSource;

    bool _pauseGame = false;
    public bool PauseGame
    {
        get
        {
            return _pauseGame;
        }
        set
        {
            _pauseGame = value;
            if (_pauseGame)
                Time.timeScale = 0f;
            else
                Time.timeScale = 1f;
        }
    }

    // Start is called before the first frame update
    void Start()
    {

    }


    public void PlayFxAudio(AudioClip _clip)
    {
        if (_clip != null)
        {
            fxAudioSource.PlayOneShot(_clip);
        }
        else
        {
            print("audio file not found " + _clip.name);
        }
    }


    public void LoadLevel(int _levelId)
    {
        SceneManager.LoadScene("splash");
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}

