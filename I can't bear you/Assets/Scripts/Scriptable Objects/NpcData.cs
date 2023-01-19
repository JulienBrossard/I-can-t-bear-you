using UnityEngine;

[CreateAssetMenu(fileName = "Npc Data", menuName = "ScriptableObjects/Npc Data")]
public class NpcData : ScriptableObject
{
    public float maxHunger;
    public float maxThirst;
    public float maxBladder;
    public float speed = 10;
    public float runSpeed;
    public float acceleration = 10;
    public float screamRadius = 5;
    public LayerMask screamLayerMask;
}
