using System;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(StatusEffects))]
public class StatusEffectsEditor : Editor
{
#if UNITY_EDITOR
    public override void OnInspectorGUI()
    {
        UpdateStatusEffects(target as StatusEffects);
        base.OnInspectorGUI();
    }
    
    void UpdateStatusEffects(StatusEffects statusEffects)
    {
        if (statusEffects.statusEffectsData != null)
        {
            if (statusEffects.currentStatusEffectValue < statusEffects.statusEffectsData.littleDrunkData.minimumValue)
            {
                statusEffects.status = StatusEffects.Status.NORMAL;
            }
            else if (statusEffects.currentStatusEffectValue >= statusEffects.statusEffectsData.littleDrunkData.minimumValue && statusEffects.currentStatusEffectValue < statusEffects.statusEffectsData.drunkData.minimumValue)
            {
                statusEffects.status = StatusEffects.Status.LITTLE_DRUNK;
            }
            else if (statusEffects.currentStatusEffectValue >= statusEffects.statusEffectsData.drunkData.minimumValue && statusEffects.currentStatusEffectValue < statusEffects.statusEffectsData.veryDrunkData.minimumValue)
            {
                statusEffects.status = StatusEffects.Status.DRUNK;
            }
            else
            {
                statusEffects.status = StatusEffects.Status.VERY_DRUNK;
            }
        }
    }

    private void OnEnable()
    {
        StatusEffects statusEffects = target as StatusEffects;
        statusEffects.panicData = statusEffects.gameObject.GetComponent<Panic>();
        Debug.Log("ok");
    }
    
#endif
}
