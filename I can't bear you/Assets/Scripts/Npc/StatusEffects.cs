using UnityEngine;

public class StatusEffects : MonoBehaviour
{
    [SerializeField] private Panic panicData;
    [HideInInspector] public CurrentData currentData;
    
    
    public enum Status
    {
        NORMAL,
        LITTLE_DRUNK,
        DRUNK,
        VERY_DRUNK
    }

    public Status status;

    void UpdateStatusEffects(float effectValue)
    {
        
    }
    
    void UpdatePanics()
    {
        switch (status)
        {
            case Status.LITTLE_DRUNK:
                panicData.UpdatePanic(-0.5f);
                break;
            case Status.DRUNK :
                panicData.UpdatePanic(-1f);
                break;
            case Status.VERY_DRUNK :
                panicData.UpdatePanic(-1f);
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
