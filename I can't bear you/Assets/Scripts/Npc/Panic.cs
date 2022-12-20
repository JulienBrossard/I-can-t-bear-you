using System;
using System.Collections;
using UnityEngine;

[Serializable] [RequireComponent(typeof(Npc), typeof(CapsuleCollider))]
public class Panic : MonoBehaviour
{
    #region Enums
    public enum PanicState
    {
        Calm,
        Tense,
        Panic,
    }

    [SerializeField] Tools.FIELD field = Tools.FIELD.HIDDEN;

    #endregion
    
    [Header("Data")]
    [Range(0,1)]
    public float currentPanic;
    [ConditionalEnumHide("field", 0)] public PanicState panicState = PanicState.Tense;
    public PanicData panicData;
    
    [Header("Npc Script")]
    public Npc npc;

    /// <summary>
    /// Update Panic of npc
    /// </summary>
    /// <param name="panic"> Value to added </param>
    public void UpdatePanic(float panic)
    {
        currentPanic += panic;
        if (currentPanic < panicData.suspiciousValue)
        {
            npc.UpdateSpeed(npc.npcData.speed);
            SwitchPanicState(PanicState.Calm);
            StopCoroutine(UpdateSuspicious());
            StartCoroutine(UpdateSuspicious());
            return;
        }
        if(currentPanic >= panicData.suspiciousValue && currentPanic <1f)
        {
            npc.UpdateSpeed(npc.npcData.speed);
            SwitchPanicState(PanicState.Tense);
            StopCoroutine(UpdatePanic());
            StartCoroutine(UpdatePanic());
        }
        else if(currentPanic >= 1f)
        {
            npc.UpdateSpeed(npc.npcData.runSpeed);
            SwitchPanicState(PanicState.Panic);
            currentPanic = 1f;
            if (!NpcManager.instance.npcScriptDict[gameObject].isDie)
            {
                PlayerStateManager.instance.SwitchState(PlayerStateManager.instance.bearserkerState);
                npc.npcScripts.npcUI.panicImage.transform.parent.gameObject.SetActive(true);
                npc.npcScripts.npcUI.panicImage.fillAmount = 1;
            }
        }
    }

    /// <summary>
    /// Update Suspicious Image
    /// </summary>
    /// <returns></returns>
    IEnumerator UpdateSuspicious()
    {
        npc.npcScripts.npcUI.suspiciousImage.transform.parent.gameObject.SetActive(true);
        npc.npcScripts.npcUI.suspiciousImage.fillAmount = currentPanic*1/panicData.suspiciousValue;
        yield return new WaitForSeconds(3f);
        npc.npcScripts.npcUI.suspiciousImage.transform.parent.gameObject.SetActive(false);
    }
    
    /// <summary>
    /// Update Panic Image
    /// </summary>
    /// <returns></returns>
    IEnumerator UpdatePanic()
    {
        npc.npcScripts.npcUI.panicImage.transform.parent.gameObject.SetActive(true);
        npc.npcScripts.npcUI.panicImage.fillAmount = (currentPanic-panicData.suspiciousValue)*1/panicData.suspiciousValue;
        yield return new WaitForSeconds(3f);
        npc.npcScripts.npcUI.panicImage.transform.parent.gameObject.SetActive(false);
    }

    /// <summary>
    /// Switch Panic State
    /// </summary>
    /// <param name="panicState"> New Panic State </param>
    void SwitchPanicState(PanicState panicState)
    {
        if (this.panicState != panicState)
        {
            this.panicState = panicState;
            NpcManager.instance.npcScriptDict[gameObject].currentDestination = Vector3.zero;
            Debug.Log("New Panic State : " + panicState);
        }
    }
}
