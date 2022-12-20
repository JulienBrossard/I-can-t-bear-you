using System;
using Unity.VisualScripting;
using UnityEditor;

[CustomEditor(typeof(Panic))]
public class PanicEditor : Editor
{
#if UNITY_EDITOR
    public override void OnInspectorGUI()
    {
        UpdatePanicState(target as Panic);
        base.OnInspectorGUI();
    }

    void UpdatePanicState(Panic panic)
    {
        if (panic.panicData != null)
        {
            if (panic.currentPanic < panic.panicData.suspiciousValue)
            {
                panic.panicState = Panic.PanicState.Calm;
            }
            else if (panic.currentPanic >= panic.panicData.suspiciousValue && panic.currentPanic < 1)
            {
                panic.panicState = Panic.PanicState.Tense;
            }
            else
            {
                panic.panicState = Panic.PanicState.Panic;
            }
        }
    }

    private void OnEnable()
    {
        Panic panic = target as Panic;
        panic.npc = panic.gameObject.GetComponent<Npc>();
    }

#endif
}
