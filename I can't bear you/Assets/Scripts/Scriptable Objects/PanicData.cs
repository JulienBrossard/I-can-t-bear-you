using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "Panic Data", menuName = "ScriptableObjects/Panic Data")]
public class PanicData : ScriptableObject
{
    [Header("Panic rate")]
    public float suspicious;
    public float investigateRadius;
    
    [Header("Suspicious Value")]
    public float tenseValue = 0.5f;
}
