using UnityEngine;

[CreateAssetMenu(fileName = "Npc data", menuName = "Npc Data")]
public class NpcData : ScriptableObject
{
    public float maxHunger;
    public float maxThirst;
    public float maxBladder;
    public float speed = 10;
    public float acceleration = 10;
}
