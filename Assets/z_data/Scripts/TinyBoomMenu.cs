using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class TinyBoomMenu : MonoBehaviour
{

    [SerializeField] RectTransform mainPanelRect;
    [SerializeField] float panelCloseX = -1593f;

    bool isClosed = true;
    // Start is called before the first frame update
    void Start()
    {

    }

    public void ToggleMenu()
    {
        if (isClosed)
        {
            isClosed = false;
            GameManager.Instance.PauseGame = true;
            OpenPanel();
        }
        else
        {
            isClosed = true;
            GameManager.Instance.PauseGame = false;
            ClosePanel();
        }
    }

    void OpenPanel()
    {
        DOTween.KillAll();
        mainPanelRect.DOAnchorPosX(0f, 1f).SetEase(Ease.OutQuint).SetUpdate(true);
    }
    void ClosePanel()
    {
        DOTween.KillAll();
        mainPanelRect.DOAnchorPosX(panelCloseX, 1f).SetEase(Ease.OutQuint);
    }


    public void RestartButtonPressed()
    {
        GameManager.Instance.RestartGame();
    }
}
