using UnityEditor;

[CustomEditor(typeof(NpcScripts))]
public class NpcScriptsEditor : Editor
{
#if UNITY_EDITOR
    
    private void OnEnable()
    {
        NpcScripts npcScripts = (NpcScripts)target;
        npcScripts.panicData = npcScripts.GetComponent<Panic>();
        npcScripts.statusEffects = npcScripts.GetComponent<StatusEffects>();
        npcScripts.npcUI = npcScripts.GetComponent<NpcUI>();
    }
    
#endif
}
