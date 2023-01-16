using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(Panic))]
public class StatusEffects : MonoBehaviour
{
    #region Enums

    public enum Status
    {
        NORMAL,
        LITTLE_DRUNK,
        DRUNK,
        VERY_DRUNK
    }

    [SerializeField] Tools.FIELD field = Tools.FIELD.HIDDEN;

    #endregion
    
    
    [Header("Data")]
    public Panic panicData;
    public StatusEffectsData statusEffectsData;
    [Range(0,1)]
    public float currentStatusEffectValue;
    public CurrentData currentData;

    [ConditionalEnumHide("field", 0)] public Status status;

    private void Awake()
    {
        currentData = new CurrentData();
        UpdateStatusEffects(0);
    }

    /// <summary>
    /// Update Status Effects
    /// </summary>
    /// <param name="effectValue"> Value to add </param>
    public void UpdateStatusEffects(float effectValue)
    {
        currentStatusEffectValue += effectValue;

        if (currentStatusEffectValue < statusEffectsData.littleDrunkData.minimumValue)
        {
            SwitchStatus(Status.NORMAL);
        }
        else if (currentStatusEffectValue >= statusEffectsData.littleDrunkData.minimumValue && currentStatusEffectValue < statusEffectsData.drunkData.minimumValue)
        {
            SwitchStatus(Status.LITTLE_DRUNK);
        }
        else if (currentStatusEffectValue >= statusEffectsData.drunkData.minimumValue && currentStatusEffectValue < statusEffectsData.veryDrunkData.minimumValue)
        {
            SwitchStatus(Status.DRUNK);
        }
        else
        {
            SwitchStatus(Status.VERY_DRUNK);
        }
        
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
    
    /// <summary>
    /// Update Panic when switch status
    /// </summary>
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
    
    
    /// <summary>
    /// Switch status
    /// </summary>
    /// <param name="newStatus"> New Status </param>
    void SwitchStatus(Status newStatus)
    {
        if (status != newStatus)
        {
            status = newStatus;
            UpdatePanics();
            Debug.Log("New Status : " + status);
        }
    }
}

public class CurrentData
{
    public float currentAwarenessRatio;
    public float currentSpeedRatio;
    public float currentDamageRatio;
}
