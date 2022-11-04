using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Player Stats", menuName = "ScriptableObjects/PlayerStats")]
public class PlayerStats : ScriptableObject
{
    [Header("Movement")]
    public AnimationCurve accelerationCurve;
    public AnimationCurve slowdownCurve;
    public float accelerationStep,slowdownStep;
    public float maxSpeed;
    [Header("Sight")] 
    [Range(0.1f,Mathf.PI)]public float detectionAngle;
    [SerializeField] public AnimationCurve detectionAngleCurve;
    [Range(0.1f,5f)]public float detectionRange;
    [SerializeField] public AnimationCurve detectionRangeCurve;
    [Range(0.01f,0.5f)]public float detectionStep;
}
