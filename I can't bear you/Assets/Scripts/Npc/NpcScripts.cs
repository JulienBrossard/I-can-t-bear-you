using UnityEngine;

[RequireComponent(typeof(Panic), typeof(StatusEffects), typeof(NpcUI))]
public class NpcScripts : MonoBehaviour
{
    [Header("Scripts")] 
    public Panic panicData;
    public StatusEffects statusEffects;
    public NpcUI npcUI;
}
