using System;
using UnityEngine;

public class AwarenessNpc : Awareness
{
    [Header("Scripts")] 
    [SerializeField] private Panic panicData;
    [SerializeField] private StatusEffects statusEffects;
    
    void Start()
    {
        StartCoroutine ("FindTargetsWithDelay", .2f);
        viewRadius = viewRadius + (panicData.currentPanic * viewRadius) -
                     ((1-statusEffects.currentData.currentAwarenessRatio) * viewRadius);
    }

    private void FixedUpdate()
    {
        CheckForBear();
        CheckForNpcPanic();
    }

    private void CheckForBear()
    {
        if (visibleTargets.Count > 0)
        {
            if (visibleTargets.Contains(LevelManager.instance.GetPlayer()))
            {
                if (PlayerStateManager.instance.currentState.currentSusState == PlayerState.SUSSTATE.SUSPICIOUS)
                {
                    panicData.UpdatePanic(panicData.panicData.suspicious);
                }

                if (PlayerStateManager.instance.currentState.currentSusState == PlayerState.SUSSTATE.FREIGHTNED)
                {
                    panicData.UpdatePanic(1);
                }
            }
        }
    }

    private void CheckForNpcPanic()
    {
        if (visibleTargets.Count > 0)
        {
            foreach (var npc in visibleTargets)
            {
                if (NpcManager.instance.panicDict.ContainsKey(npc.gameObject))
                {
                    if (NpcManager.instance.panicDict[npc.gameObject].panicState == Panic.PanicState.Tense)
                    {
                        panicData.UpdatePanic(panicData.panicData.suspicious);
                    }
                    else if(NpcManager.instance.panicDict[npc.gameObject].panicState == Panic.PanicState.Panic)
                    {
                        if (panicData.currentPanic < 0.5f)
                        {
                            panicData.UpdatePanic(panicData.currentPanic + 0.5f);
                        }
                        panicData.UpdatePanic(panicData.panicData.suspicious);
                    }
                }
            }
 
               
            
        }
    }
     
}
