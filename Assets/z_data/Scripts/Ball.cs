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
        print("on mouse down " + gameObject.name);
        Destroy();
    }

    void CreateBurstParticleEffect()
    {
        GameObject eff = Instantiate(LevelController.Instance.currentTheme.burstEffectPrefab, transform.position, Quaternion.identity);
        eff.transform.position = new Vector3(eff.transform.position.x, eff.transform.position.y, -1.3f);
        Destroy(eff, 2f);
    }

    public void Destroy(float _ddelay = 0.1f)
    {
        List<Ball> otherBalls = GetCorrectNeighbourBalls();
        if (!markedForDeletion && otherBalls.Count >= LevelController.Instance.ballMatchCount)
        {
            markedForDeletion = true;
            for (int i = 0; i < otherBalls.Count; i++)
            {
                if (otherBalls[i] != null)
                    otherBalls[i].Destroy();
            }
            LevelController.Instance.allBallsOnStage.Remove(this);

            CreateBurstParticleEffect();
            GameManager.Instance.PlayFxAudio(LevelController.Instance.currentTheme.burstAudioClip);

            Destroy(shineTr.gameObject);
            Destroy(gameObject);
        }
    }
}
