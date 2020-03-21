using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
using UnityEngine.UI;

public class SplashScreen : MonoBehaviour
{
    [SerializeField] Image logoImage;

    // Start is called before the first frame update
    void Start()
    {
        if (!GameManager.Instance.splashDone)
        {
            GameManager.Instance.splashDone = true;
            logoImage.color = new Color(1f, 1f, 1f, 0f);
            logoImage.DOFade(1f, 3f).SetDelay(1f);
            logoImage.DOFade(0f, 3f).SetDelay(3f).OnComplete(LoadGameScene);
        }
        else
        {
            LoadGameScene();
        }
    }

    void LoadGameScene()
    {
        SceneManager.LoadScene("game");
    }
}

