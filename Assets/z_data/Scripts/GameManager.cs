using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


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
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    public BallCreator ballCreator;
    public Transform dynamicObjects;
    public bool testMode = false;
    public int ballMatchCount = 2;
    public string currentTheme = "theme_1";
    [SerializeField] int ballCreateCount = 4;
    [SerializeField] AudioSource fxAudioSource;
    [SerializeField] AudioSource bgAudioSource;
    [HideInInspector] public List<Ball> allBallsOnStage;

    // Start is called before the first frame update
    void Start()
    {
        ballCreator.CreateBalls(ballCreateCount);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            testMode = true;
        }
        if (Input.GetKeyUp(KeyCode.T))
        {
            testMode = false;
        }
    }

    public void CheckForLevelEndState()
    {
        if (!IsThereAnyMatchFound())
        {
            // print("level ended");
            ballCreator.CreateBalls(ballCreateCount);
        }
    }

    public bool IsThereAnyMatchFound()
    {
        foreach (Ball bb in allBallsOnStage)
        {
            if (bb.GetCorrectNeighbourBalls().Count >= ballMatchCount)
                return true;
        }
        return false;
    }

    public void PlayFxAudio(string _clipName)
    {
        string clipPath = GameManager.Instance.currentTheme + "/" + _clipName;
        AudioClip clip = Resources.Load<AudioClip>(clipPath);

        if (clip != null)
        {
            fxAudioSource.PlayOneShot(clip);
        }
        else
        {
            print("audio file not found " + clipPath);
        }
    }

}

