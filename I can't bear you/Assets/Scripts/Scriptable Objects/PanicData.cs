using UnityEngine;

[CreateAssetMenu(fileName = "Panic Data", menuName = "Panic Data")]
public class PanicData : ScriptableObject
{
    [Header("Panic rate")]
    public float suspicious;
    public float investigateRadius;
    
    [Header("Suspicious Value")]
    public float suspiciousValue = 0.5f;
}
