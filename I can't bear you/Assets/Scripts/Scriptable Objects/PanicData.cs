using UnityEngine;

[CreateAssetMenu(fileName = "Panic Data", menuName = "Panic Data")]
public class PanicData : ScriptableObject
{
    [Header("Panic rate")]
    public float murder;
    public float suspicious;
    public float investigateRadius;
}
