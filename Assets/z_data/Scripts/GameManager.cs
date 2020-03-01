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
    public int ballIdLimit = 6;
    public bool testMode = false;
    [SerializeField] int ballCreateCount = 4;
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
            if (bb.GetCorrectNeighbourBalls().Count > 0)
                return true;
        }
        return false;
    }

}

