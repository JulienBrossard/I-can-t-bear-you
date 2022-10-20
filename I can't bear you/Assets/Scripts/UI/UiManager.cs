using System;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{
    public static UiManager instance;
    [SerializeField] private Image bearserkerGauge;
    [SerializeField] private TextMeshProUGUI remainingNpcText;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this);
        }
        instance = this;
    }


    public void UpdateBearserkerGauge(float value)
    {
        bearserkerGauge.DOFillAmount(bearserkerGauge.fillAmount + value, 0.2f);
    }
    
    public void UpdateRemainingNpcText()
    {
        remainingNpcText.text = NpcManager.instance.npcCountRemaining + "/" + NpcManager.instance.npcCountMax;
    }
}
