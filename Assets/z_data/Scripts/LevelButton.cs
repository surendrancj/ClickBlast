using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelButton : MonoBehaviour
{
    [SerializeField] int levelId = 0;
    [SerializeField] int levelCost = 500;
    [SerializeField] GameObject lockModeGroup;
    [SerializeField] GameObject playModeGroup;
    [SerializeField] GameObject notEnoughStarsGroup;
    [SerializeField] Text notEnoughStarsText;

    // Start is called before the first frame update
    void Start()
    {
        RefreshButton();
    }

    void RefreshButton()
    {
        HideAllGroups();
        if (GameManager.Instance.IsLevelLocked(levelId))
            lockModeGroup.SetActive(true);
        else
            playModeGroup.SetActive(true);

        if (levelId == 1)
        {
            HideAllGroups();
            playModeGroup.SetActive(true);
        }
    }

    void HideAllGroups()
    {
        lockModeGroup.SetActive(false);
        playModeGroup.SetActive(false);
        notEnoughStarsGroup.SetActive(false);
    }

    public void OnClicked()
    {
        // check if we can unlock the level 
        if (GameManager.Instance.TotalStars >= levelCost)
        {
            // we have more stars we can unlock this level 
            GameManager.Instance.TotalStars = GameManager.Instance.TotalStars - levelCost;
            GameManager.Instance.UnlockLevel(levelId);
        }

        if (!GameManager.Instance.IsLevelLocked(levelId))
        {
            GameManager.Instance.themeIndex = levelId.ToString();
            print("loading level " + levelId);
            GameManager.Instance.LoadLevel(levelId);
        }
        else
        {
            print("level locked.");
            notEnoughStarsGroup.SetActive(true);
            notEnoughStarsText.text = (levelCost - GameManager.Instance.TotalStars) + " stars needed";
            CancelInvoke();
            Invoke("HideNotEnoughStarsAlert", 3f);
        }
    }

    void HideNotEnoughStarsAlert()
    {
        notEnoughStarsGroup.SetActive(false);
    }
}
