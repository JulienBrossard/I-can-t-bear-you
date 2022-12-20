using UnityEngine;

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
    [Header("Roar")]
    public float roarRange;
    public float roarCD;
    public float roarFreezeChance;
    public float roarFreezeDuration;
    public float roarDuration;
}
