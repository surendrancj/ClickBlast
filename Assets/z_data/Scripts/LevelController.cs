using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    private static LevelController _instance;
    public static LevelController Instance
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
            Destroy(this.gameObject);
            return;
        }
    }

    public int ballMatchCount = 1;
    public BallCreator ballCreator;
    public Transform dynamicObjects;
    public List<Sprite> allBallSprites;
    public ThemeData currentTheme;
    public int ballSpriteIndex = 0;

    [SerializeField] int ballCreateCount = 4;
    [HideInInspector] public List<Ball> allBallsOnStage;
    [HideInInspector] public int uniqueBallCount = 6;

    // Start is called before the first frame update
    void Start()
    {
        // load current theme 
        int themeIndex = UnityEngine.Random.Range(0, 16);
        currentTheme = Resources.Load<ThemeData>("theme_data_" + themeIndex);

        // create bg 
        GameObject newBg = Instantiate(currentTheme.bgPrefab, transform);
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
