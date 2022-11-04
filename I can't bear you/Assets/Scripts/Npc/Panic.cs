using System;
using UnityEngine;

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


    public void UpdatePanic(float panic)
    {
        currentPanic += panic;
        if (currentPanic < 0.5f)
        {
            npc.UpdateSpeed(npc.npcData.speed);
            panicState = PanicState.Calm;
        }
        else if(currentPanic >= 0.5f && currentPanic <1f)
        {
            npc.UpdateSpeed(npc.npcData.speed);
            panicState = PanicState.Tense;
        }
        else if(currentPanic >= 1f)
        {
            npc.UpdateSpeed(npc.npcData.runSpeed);
            panicState = PanicState.Panic;
            currentPanic = 1f;
        }
    }
}
