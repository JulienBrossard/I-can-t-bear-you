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
    [Range(0.1f,Mathf.PI)]public float detectionAngle;
    [SerializeField] public AnimationCurve detectionAngleCurve;
    [Range(0.1f,5f)]public float detectionRange;
    [SerializeField] public AnimationCurve detectionRangeCurve;
    [Range(0.01f,0.5f)]public float detectionStep;
    [Header("Roar")]
    public float roarRange;
    public float roarCD;
    public float roarFreezeChance;
    public float roarFreezeDuration;
    public float roarDuration;
}
