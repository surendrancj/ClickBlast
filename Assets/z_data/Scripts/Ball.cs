using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Ball : MonoBehaviour
{
    public CircleCollider2D circleCollider;
    public Rigidbody2D rbd;
    [SerializeField] Sprite[] allSprites;
    [SerializeField] Material[] allMaterials;
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] Renderer sphereRenderer;
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

    public void SetId(int _id)
    {
        id = _id;
        spriteRenderer.sprite = allSprites[_id];
        sphereRenderer.material = allMaterials[_id];
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
        if (!markedForDeletion && otherBalls.Count > 0)
        {
            markedForDeletion = true;
            foreach (Ball ob in otherBalls)
            {
                if (ob != null)
                    ob.Destroy();
            }
            GameManager.Instance.allBallsOnStage.Remove(this);
            Destroy(gameObject, 0.1f);
        }
        GameManager.Instance.CheckForLevelEndState();
    }

    int GetNeighbourBallCountOfSameId()
    {
        int rCount = 0;
        foreach (Ball bb in connectedBalls)
        {
            if (bb != null && bb.id == id)
                rCount++;
        }
        return rCount;
    }
}
