using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallCreator : MonoBehaviour
{
    [SerializeField] GameObject ballPrefab = null;

    public void CreateBalls(int _count = 10)
    {
        StopAllCoroutines();
        GameManager.Instance.allBallsOnStage = new List<Ball>();
        StartCoroutine(StartCreatingBalls(_count));
    }

    IEnumerator StartCreatingBalls(int _count)
    {
        while (_count > 0)
        {
            yield return new WaitForSeconds(0.1f);
            Ball newBall = Instantiate(ballPrefab, transform.position, Quaternion.identity).GetComponent<Ball>();
            newBall.gameObject.name = "ball_" + _count;
            newBall.transform.parent = GameManager.Instance.dynamicObjects;
            newBall.transform.localPosition = Random.insideUnitCircle * 0.5f;
            newBall.SetId(Random.Range(0, GameManager.Instance.ballIdLimit));
            GameManager.Instance.allBallsOnStage.Add(newBall);
            _count--;
        }
    }
}
