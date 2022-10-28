using System;
using UnityEngine;

public class StatusEffects : MonoBehaviour
{
    [Header("Data")]
    [SerializeField] private Panic panicData;
    [SerializeField] private StatusEffectsData statusEffectsData;
    [Range(0,1)]
    [SerializeField] private float currentStatueEffectValue;
    public CurrentData currentData;
    
    
    public enum Status
    {
        NORMAL,
        LITTLE_DRUNK,
        DRUNK,
        VERY_DRUNK
    }

    public Status status;

    private void Awake()
    {
        currentData = new CurrentData();
        UpdateStatusEffects(0);
    }

    void UpdateStatusEffects(float effectValue)
    {
        currentStatueEffectValue += effectValue;
        switch (status)
        {
            case Status.NORMAL :
                currentData.currentAwarenessRatio = statusEffectsData.normalData.awarenessRatio;
                currentData.currentSpeedRatio = statusEffectsData.normalData.speedRatio;
                currentData.currentDamageRatio = statusEffectsData.normalData.damageRatio;
                break;
            case Status.LITTLE_DRUNK :
                currentData.currentAwarenessRatio = statusEffectsData.littleDrunkData.awarenessRatio;
                currentData.currentSpeedRatio = statusEffectsData.littleDrunkData.speedRatio;
                currentData.currentDamageRatio = statusEffectsData.littleDrunkData.damageRatio;
                break;
            case Status.DRUNK :
                currentData.currentAwarenessRatio = statusEffectsData.drunkData.awarenessRatio;
                currentData.currentSpeedRatio = statusEffectsData.drunkData.speedRatio;
                currentData.currentDamageRatio = statusEffectsData.drunkData.damageRatio;
                break;
            case Status.VERY_DRUNK :
                currentData.currentAwarenessRatio = statusEffectsData.veryDrunkData.awarenessRatio;
                currentData.currentSpeedRatio = statusEffectsData.veryDrunkData.speedRatio;
                currentData.currentDamageRatio = statusEffectsData.veryDrunkData.damageRatio;
                break;
        }
    }
    
    void UpdatePanics()
    {
        switch (status)
        {
            case Status.LITTLE_DRUNK:
                panicData.UpdatePanic(-statusEffectsData.littleDrunkData.panicRatio);
                break;
            case Status.DRUNK :
                panicData.UpdatePanic(-statusEffectsData.drunkData.panicRatio);
                break;
            case Status.VERY_DRUNK :
                panicData.UpdatePanic(-statusEffectsData.veryDrunkData.panicRatio);
                break;
        }
    }
}

public class CurrentData
{
    public float currentAwarenessRatio;
    public float currentSpeedRatio;
    public float currentDamageRatio;
}
