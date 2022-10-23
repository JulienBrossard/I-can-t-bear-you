using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "Player Stats", menuName = "ScriptableObjects/PlayerStats")]
public class PlayerStats : ScriptableObject
{
    public AnimationCurve accelerationCurve,slowdownCurve;
    public float accelerationStep,slowdownStep;
    public float maxSpeed;
}
