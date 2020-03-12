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
        // don't fucking move this check 
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

    public BallCreator ballCreator;
    public Transform dynamicObjects;
    public int ballMatchCount = 2;
    public string themeIndex = "1";
    public ThemeData currentTheme;
    public List<Sprite> allBallSprites;
    [SerializeField] int ballCreateCount = 4;
    [SerializeField] AudioSource fxAudioSource;
    [SerializeField] AudioSource bgAudioSource;
    [HideInInspector] public List<Ball> allBallsOnStage;
    [HideInInspector] public int uniqueBallCount = 6;


    // Start is called before the first frame update
    void Start()
    {
        // load current theme 
        currentTheme = Resources.Load<ThemeData>("theme_" + themeIndex + "/theme_data");
        print("asdf " + currentTheme.allBallColors.Length);
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

    public List<Sprite> GetAllBallSprites()
    {
        if (allBallSprites.Count <= 0)
        {
            allBallSprites = new List<Sprite>();
            for (int i = 0; i < uniqueBallCount; i++)
            {
                Sprite ballSprite = Resources.Load<Sprite>("ball_" + i);
                allBallSprites.Add(ballSprite);
            }
        }
        return allBallSprites;
    }

}

