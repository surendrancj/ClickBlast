using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Ball : MonoBehaviour
{
    public CircleCollider2D circleCollider;
    public Rigidbody2D rbd;
    public Sprite[] allSprites;
    public Transform shineTr;
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] float scanRadius = 3f;

    List<Ball> connectedBalls = new List<Ball>();
    bool markedForDeletion = false;
    [HideInInspector] public int id;

    // Start is called before the first frame update
    void Start()
    {
        connectedBalls = new List<Ball>();
        markedForDeletion = false;
    }

    void Update()
    {
        shineTr.position = gameObject.transform.position;
    }

    public void SetId(int _id)
    {
        id = _id;
        spriteRenderer.sprite = allSprites[_id];
    }

    public List<Ball> GetCorrectNeighbourBalls()
    {
        List<Ball> rBalls = new List<Ball>();
        Collider2D[] otherColliders = Physics2D.OverlapCircleAll(transform.position, scanRadius);
        foreach (Collider2D otc in otherColliders)
        {
            Ball otherBall = otc.GetComponent<Ball>();
            if (otherBall != null && otherBall != this && !rBalls.Contains(otherBall) && otherBall.id == id)
            {
                rBalls.Add(otherBall);
            }
        }
        return rBalls;
    }

    void OnMouseDown()
    {
        if (GameManager.Instance.testMode)
            PrintAllConnectedBalls();
        else
            Destroy();
    }

    void PrintAllConnectedBalls()
    {
        print("clicked on " + gameObject.name + " id " + id);
        List<Ball> otherBalls = GetCorrectNeighbourBalls();
        print("other ball count " + otherBalls.Count);
        foreach (Ball bb in otherBalls)
        {
            if (bb != null && bb.id == id)
                print(bb.gameObject.name);
        }
    }

    public void Destroy()
    {
        List<Ball> otherBalls = GetCorrectNeighbourBalls();
        if (!markedForDeletion && otherBalls.Count >= GameManager.Instance.ballMatchCount)
        {
            markedForDeletion = true;
            foreach (Ball ob in otherBalls)
            {
                if (ob != null)
                    ob.Destroy();
            }
            GameManager.Instance.allBallsOnStage.Remove(this);
            Destroy(shineTr.gameObject, 0.1f);
            Destroy(gameObject, 0.1f);
        }
        GameManager.Instance.CheckForLevelEndState();
    }
}
