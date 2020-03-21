using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Ball : MonoBehaviour
{
    public CircleCollider2D circleCollider;
    public Rigidbody2D rbd;
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

    public void SetColor(Color _color)
    {
        spriteRenderer.color = _color;
    }

    public void SetId(int _id, Sprite _sprite)
    {
        id = _id;
        spriteRenderer.sprite = _sprite;
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
        Destroy();
    }

    void CreateBurstParticleEffect()
    {
        GameObject effect = Resources.Load<GameObject>(GameManager.Instance.currentTheme + "\\burst_effect");
        if (effect != null)
        {
            GameObject eff = Instantiate(effect, transform.position, Quaternion.identity);
            eff.transform.position = new Vector3(eff.transform.position.x, eff.transform.position.y, -1.3f);
            Destroy(eff, 2f);
        }
    }

    public void Destroy(float _ddelay = 0.1f)
    {
        List<Ball> otherBalls = GetCorrectNeighbourBalls();
        if (!markedForDeletion && otherBalls.Count >= GameManager.Instance.ballMatchCount)
        {
            markedForDeletion = true;
            for (int i = 0; i < otherBalls.Count; i++)
            {
                if (otherBalls[i] != null)
                    otherBalls[i].Destroy(i * .3f);
            }
            GameManager.Instance.allBallsOnStage.Remove(this);

            CreateBurstParticleEffect();
            GameManager.Instance.PlayFxAudio(GameManager.Instance.currentTheme.burstAudioClip.name);

            Destroy(shineTr.gameObject, _ddelay);
            Destroy(gameObject, _ddelay);
        }
        // GameManager.Instance.CheckForLevelEndState();
    }
}
