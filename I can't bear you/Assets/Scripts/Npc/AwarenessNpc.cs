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
}
