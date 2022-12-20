using UnityEditor;

[CustomEditor(typeof(AwarenessNpc))]
public class AwarenessNpcEditor : Editor
{
#if UNITY_EDITOR
    private void OnEnable()
    {
        AwarenessNpc awarenessNpc = (AwarenessNpc)target;
        awarenessNpc.panicData = awarenessNpc.gameObject.GetComponent<Panic>();
        awarenessNpc.statusEffects = awarenessNpc.gameObject.GetComponent<StatusEffects>();
    }
    
#endif
}
