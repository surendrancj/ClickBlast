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
    public List<Sprite> allBallSprites;
    public ThemeData currentTheme;

    [SerializeField] int ballCreateCount = 4;
    [SerializeField] AudioSource fxAudioSource;
    [SerializeField] AudioSource bgAudioSource;
    [SerializeField] GameObject levelData;
    [HideInInspector] public List<Ball> allBallsOnStage;
    [HideInInspector] public int uniqueBallCount = 6;

    public int ballSpriteIndex = 0;

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
        // load current theme 
        int themeIndex = UnityEngine.Random.Range(0, 16);
        currentTheme = Resources.Load<ThemeData>("theme_data_" + themeIndex);

        // create bg 
        GameObject newBg = Instantiate(currentTheme.bgPrefab, levelData.transform);
        ballSpriteIndex = UnityEngine.Random.Range(0, allBallSprites.Count);
        ballCreator.CreateFirstBalls(60);
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
        AudioClip clip = Resources.Load<AudioClip>(_clipName);

        if (clip != null)
        {
            fxAudioSource.PlayOneShot(clip);
        }
        else
        {
            print("audio file not found " + _clipName);
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

    public bool IsLevelLocked(int _levelIndex)
    {
        if (_levelIndex == 1)
            return false;
        else
            return !PlayerPrefs.HasKey(Constants.LEVEL_LOCK_KEY + _levelIndex);
    }

    public void UnlockLevel(int _levelIndex)
    {
        PlayerPrefs.SetInt(Constants.LEVEL_LOCK_KEY + _levelIndex, 1);
    }

    public void LockLevel(int _levelIndex)
    {
        PlayerPrefs.DeleteKey(Constants.LEVEL_LOCK_KEY + _levelIndex);
    }

    public void LoadLevel(int _levelId)
    {
        SceneManager.LoadScene("game");
    }

}

