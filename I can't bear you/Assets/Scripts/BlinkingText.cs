using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class BlinkingText : MonoBehaviour
{
    private TextMeshProUGUI textToBlink;
    
    void Start()
    {
        textToBlink = gameObject.GetComponent<TextMeshProUGUI>();
        textToBlink.DOFade(0, 1f).SetLoops(-1, LoopType.Yoyo);
    }

}
