using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{
    public static UiManager instance;
    [SerializeField] private Image bearserkerGauge;
    [SerializeField] private Image currentItem;
    [SerializeField] private TextMeshProUGUI remainingNpcText;
    private int maxNpc;

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
        maxNpc = LevelManager.instance.levels[LevelManager.instance.currentLevel-1].npcCount;
        UpdateRemainingNpcText();
    }


    public void UpdateBearserkerGauge(float value, float time)
    {
        bearserkerGauge.DOFillAmount(bearserkerGauge.fillAmount + value, time);
    }
    
    public void UpdateCurrentItem(Sprite sprite)
    {
        currentItem.sprite = sprite;
    }
    
    public void UpdateRemainingNpcText()
    {
        remainingNpcText.text = NpcManager.instance.npcCountRemaining + "/" + maxNpc;
    }
}
