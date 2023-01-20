using System;
using System.Collections;
using DG.Tweening;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{
    public static UiManager instance;
    [SerializeField] private Image bearserkerGauge;
    [SerializeField] private Image currentItem;
    [SerializeField] private TextMeshProUGUI remainingNpcText;
    [SerializeField] private GameObject endLvlMenu;
    [SerializeField] private GameObject uiInGame;
    [SerializeField] private GameObject npcIcon;
    [SerializeField] private GameObject npcIconParent;
    [SerializeField] private Image fadeImage;
    [SerializeField] private GameObject winScreen;
    [SerializeField] private GameObject looseScreen;
    [SerializeField] private TextMeshProUGUI textEndScreen;
    private int maxNpc;
    private bool endScreenLaunched;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this);
        }
        instance = this;
    }

    private void Start()
    {
        maxNpc = LevelManager.instance.level.npcCount;
        UpdateRemainingNpcText();
    }


    public void UpdateBearserkerGauge(float value)
    {
        bearserkerGauge.fillAmount = value;
    }
    
    public void UpdateCurrentItem(Sprite sprite)
    {
        currentItem.sprite = sprite;
    }
    
    public void UpdateRemainingNpcText()
    {
        remainingNpcText.text = NpcManager.instance.npcCountRemaining + "/" + maxNpc;
    }

    public void LaunchEndLevelScreen(bool win)
    {
        if (endScreenLaunched)
        {
            return;
        }
        else
        {
            StartCoroutine(WaitForEndStateAnim(win));
            if (win)
            {
                PlayerAnimatorManager.instance.SetAnimatorTrigger("Win");
                Debug.Log("win");
            }
            else
            {
                PlayerAnimatorManager.instance.SetAnimatorTrigger("Sleep");
                Debug.Log("Lose");
            }
            endScreenLaunched = true;
        }
            
      
    }

    IEnumerator WaitForEndStateAnim(bool win)
    {
        fadeImage.gameObject.SetActive(true);
        yield return new WaitForSeconds(1f);
        fadeImage.DOFade(1f, 1f);
        yield return new WaitForSeconds(1f);
        textEndScreen.text = win ? "The party is stopped" : "The party goes on...";
        fadeImage.DOFade(0f, 1f).OnComplete(()=> { fadeImage.gameObject.SetActive(false); });
        uiInGame.SetActive(false);
        endLvlMenu.SetActive(true);
        Camera.main.gameObject.SetActive(false);
            if (win)
                winScreen.SetActive(true);
            else
                looseScreen.SetActive(true);
            

            
    }
}
