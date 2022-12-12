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
    [SerializeField] private TextMeshProUGUI winLoseText;
    [SerializeField] private GameObject endLvlMenu;
    [SerializeField] private GameObject uiInGame;
    [SerializeField] private GameObject npcIcon;
    [SerializeField] private GameObject npcIconParent;
    [SerializeField] private float speed;
    private int iconTokill;
    private int iconWhoFlee;
    private int noIconToSwitch;
    private int currentIcon;
    private float timeToWait;
    [SerializeField] private AnimationCurve timeBetweenIconKill;
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
        if (!endScreenLaunched)
        {
            iconWhoFlee = NpcManager.instance.npcCountfleed;
            iconTokill = NpcManager.instance.npcCountkilled;
            noIconToSwitch = iconTokill + iconWhoFlee;
            winLoseText.text = win ? "You Win" : "Looser";
            uiInGame.SetActive(false);
            endLvlMenu.SetActive(true);
            for (int x = 0; x < LevelManager.instance.level.npcCount; x++)
            {
                Instantiate(npcIcon, npcIconParent.transform);
            }

            StartCoroutine(ShowDeadNpc());
            endScreenLaunched = true;
        }
  
    }

    IEnumerator ShowDeadNpc()
    {
        if (iconTokill >= 0)
        {
            npcIconParent.transform.GetChild(currentIcon).GetComponent<Image>().DOColor(Color.red, 0.1f);
            timeToWait = timeBetweenIconKill.Evaluate((float)currentIcon / noIconToSwitch) * speed;
            yield return new WaitForSeconds(timeToWait);
            iconTokill--;
            currentIcon++;
            StartCoroutine(ShowDeadNpc());
        }
        else
        {
            StartCoroutine(ShowFleeNpc());

        }
    }

    IEnumerator ShowFleeNpc()
    {
        if (iconWhoFlee >= 0)
        {
            npcIconParent.transform.GetChild(currentIcon).GetComponent<Image>().DOColor(Color.gray, 0.1f);
            timeToWait = timeBetweenIconKill.Evaluate((float)currentIcon / noIconToSwitch) * speed;
            yield return new WaitForSeconds(timeToWait);
            iconWhoFlee--;
            currentIcon++;
            StartCoroutine(ShowFleeNpc());

        }
   
    
    }
    
}
