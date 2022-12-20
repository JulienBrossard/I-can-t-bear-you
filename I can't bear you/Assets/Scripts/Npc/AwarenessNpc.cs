using UnityEngine;

[RequireComponent(typeof(Panic), typeof(StatusEffects))]
public class AwarenessNpc : Awareness
{
    [Header("Scripts")] 
    public Panic panicData;
    public StatusEffects statusEffects;
    
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

    /// <summary>
    /// Check Bear State
    /// </summary>
    private void CheckForBear()
    {
        if (visibleTargets.Count > 0)
        {
            if (visibleTargets.Contains(LevelManager.instance.GetPlayer()))
            {
                // If Player Suspicious
                if (PlayerStateManager.instance.currentState.currentSusState == PlayerState.SUSSTATE.SUSPICIOUS)
                {
                    panicData.UpdatePanic(panicData.panicData.suspicious);
                }

                // If Player Alerted
                if (PlayerStateManager.instance.currentState.currentSusState == PlayerState.SUSSTATE.FREIGHTNED)
                {
                    panicData.UpdatePanic(1);
                }
            }
        }
    }

    
    /// <summary>
    /// Check Npc State
    /// </summary>
    private void CheckForNpcPanic()
    {
        if (visibleTargets.Count > 0)
        {
            foreach (var npc in visibleTargets)
            {
                if (NpcManager.instance.npcScriptDict.ContainsKey(npc.gameObject))
                {
                    // If npc is died
                    if (NpcManager.instance.npcScriptDict[npc.gameObject].isDie)
                    {
                        panicData.UpdatePanic(1);
                    }
                }
                if (NpcManager.instance.npcScriptDict.ContainsKey(npc.gameObject))
                {
                    if(NpcManager.instance.npcScriptDict[npc.gameObject].npcScripts.panicData.panicState == Panic.PanicState.Panic)
                    {
                        // If npc is panicked
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
