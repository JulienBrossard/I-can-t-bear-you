using UnityEngine;
using UnityEngine.SocialPlatforms;

[CreateAssetMenu(fileName = "Player Stats", menuName = "ScriptableObjects/PlayerStats")]
public class PlayerStats : ScriptableObject
{
    [Header("Movement")]
    public AnimationCurve accelerationCurve;
    public AnimationCurve slowdownCurve;
    public float accelerationStep,slowdownStep;
    public float maxSpeed;
    public float turnTime;
    [Header("Sight")] 
    [Range(0f,360f)]public float detectionAngle;
    [SerializeField] public AnimationCurve detectionAngleCurve;
    [Range(0.1f,10f)]public float detectionRange;
    [SerializeField] public AnimationCurve detectionRangeCurve;
    [Header("Throw")] 
    [Range(0.01f, 1f),Tooltip("Max time throw can be held down before throw starts")] public float maxTimeThrowHeld;
    [Range(0f,1f),Tooltip("Minimal proportion of maxTimeThrowHeld for the throw to not be considered a drop")] public float mitigationRatioDropThrow;
    [Header("Damage")] 
    [Range(0f, 1f)] public float bearserkerReductionWhenElectrocuted;
    [Range(0f, 1f)] public float bearserkerReductionWhenExploded;
    [Header("Roar")]
    public float roarRange;
    public float roarCD;
    public float roarFreezeChance;
    public float roarFreezeDuration;
    public float roarDuration;
}
