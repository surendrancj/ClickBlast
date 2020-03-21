using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallCreator : MonoBehaviour
{
    [SerializeField] GameObject ballPrefab = null;
    [SerializeField] GameObject shinePrefab = null;
    [SerializeField] float ballCreatingInterval = 2f;

    public bool pauseBallCreation = false;

    int ballCreationCount = 0;

    public void CreateFirstBalls(int _count = 10)
    {
        StopAllCoroutines();
        GameManager.Instance.allBallsOnStage = new List<Ball>();
        ballCreationCount += _count;
        StartCoroutine(CreateFirstSetBalls(_count));
        StartCoroutine(StartCreateBalls());
    }

    IEnumerator StartCreateBalls()
    {
        yield return new WaitForSeconds(10f);
        while (true)
        {
            if (!pauseBallCreation)
            {
                yield return new WaitForSeconds(ballCreatingInterval);
                for (int i = 0; i < 40; i++)
                {
                    yield return new WaitForSeconds(0.2f);
                    ballCreationCount++;
                    CreateNewBall(ballCreationCount, GameManager.Instance.GetAllBallSprites());
                }
            }
        }

    }

    IEnumerator CreateFirstSetBalls(int _count)
    {
        // load all the sprites from the resources 
        while (_count > 0)
        {
            yield return new WaitForSeconds(0.05f);
            CreateNewBall(_count, GameManager.Instance.GetAllBallSprites());
            _count--;
        }
    }

    void CreateNewBall(int _count, List<Sprite> _allBallSprites)
    {
        Ball newBall = Instantiate(ballPrefab, transform.position, Quaternion.identity).GetComponent<Ball>();
        newBall.gameObject.name = "ball_" + _count;
        newBall.transform.parent = GameManager.Instance.dynamicObjects;
        newBall.transform.localPosition = Vector3.one * Random.insideUnitCircle * 0.5f;

        int randomIndex = Random.Range(0, GameManager.Instance.currentTheme.allBallColors.Length);
        newBall.SetId(randomIndex, GameManager.Instance.allBallSprites[GameManager.Instance.ballSpriteIndex]);
        GameManager.Instance.allBallsOnStage.Add(newBall);

        newBall.shineTr = Instantiate(shinePrefab, transform.position, Quaternion.identity).gameObject.transform;
        newBall.shineTr.transform.parent = GameManager.Instance.dynamicObjects.transform;

        // set the ball color 
        newBall.SetColor(GameManager.Instance.currentTheme.allBallColors[randomIndex]);
    }
}
