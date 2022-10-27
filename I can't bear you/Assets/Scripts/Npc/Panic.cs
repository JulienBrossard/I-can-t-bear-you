using System;
using UnityEngine;

[Serializable]
public class Panic : MonoBehaviour
{
    [Range(0,1)]
    [SerializeField] float currentPanic;
    public PanicData panicData;
    [SerializeField] private Npc npc;
    
    public enum PanicState
    {
        Calm,
        Tense,
        Panic,
    }
    
    public PanicState panicState = PanicState.Tense;

    public void UpdatePanic(float panic)
    {
        currentPanic += panic;
        if (currentPanic < 0.5f)
        {
            npc.currentSpeed = npc.npcData.speed;
            panicState = PanicState.Calm;
        }
        else if(currentPanic >= 0.5f && currentPanic <1f)
        {
            npc.currentSpeed = npc.npcData.speed;
            panicState = PanicState.Tense;
        }
        else if(currentPanic >= 1f)
        {
            npc.currentSpeed = npc.npcData.runSpeed;
            panicState = PanicState.Panic;
            currentPanic = 1f;
        }
    }
}
