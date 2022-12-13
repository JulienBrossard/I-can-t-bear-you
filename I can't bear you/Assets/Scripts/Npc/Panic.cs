using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class Panic : MonoBehaviour
{
    public enum PanicState
    {
        Calm,
        Tense,
        Panic,
    }
    
    [Header("Data")]
    [Range(0,1)]
    public float currentPanic;
    public PanicState panicState = PanicState.Tense;
    public PanicData panicData;
    
    [Header("Npc Script")]
    [SerializeField] private Npc npc;

    [Header("Visual Feedback")] [SerializeField]
    private Image suspiciousImage;
    [SerializeField] private Image panicImage;

    public void UpdatePanic(float panic)
    {
        currentPanic += panic;
        if (currentPanic < 0.5f)
        {
            npc.UpdateSpeed(npc.npcData.speed);
            panicState = PanicState.Calm;
            StopCoroutine(UpdateSuspicious());
            StartCoroutine(UpdateSuspicious());
            return;
        }
        if(currentPanic >= 0.5f && currentPanic <1f)
        {
            npc.UpdateSpeed(npc.npcData.speed);
            panicState = PanicState.Tense;
            StopCoroutine(UpdatePanic());
            StartCoroutine(UpdatePanic());
        }
        else if(currentPanic >= 1f)
        {
            npc.UpdateSpeed(npc.npcData.runSpeed);
            panicState = PanicState.Panic;
            currentPanic = 1f;
            if (!NpcManager.instance.npcScriptDict[gameObject].isDie)
            {
                PlayerStateManager.instance.SwitchState(PlayerStateManager.instance.bearserkerState);
                panicImage.transform.parent.gameObject.SetActive(true);
                panicImage.fillAmount = 1;
            }
        }
    }

    IEnumerator UpdateSuspicious()
    {
        suspiciousImage.transform.parent.gameObject.SetActive(true);
        suspiciousImage.fillAmount = currentPanic*2;
        yield return new WaitForSeconds(3f);
        suspiciousImage.transform.parent.gameObject.SetActive(false);
    }
    
    IEnumerator UpdatePanic()
    {
        panicImage.transform.parent.gameObject.SetActive(true);
        panicImage.fillAmount = (currentPanic-0.5f)*2;
        yield return new WaitForSeconds(3f);
        panicImage.transform.parent.gameObject.SetActive(false);
    }
}
