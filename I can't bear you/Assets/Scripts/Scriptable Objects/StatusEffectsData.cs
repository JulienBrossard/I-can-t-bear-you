using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Status Effects Data", menuName = "ScriptableObjects/Status Effects Data")]
public class StatusEffectsData : ScriptableObject
{
    public Data normalData;
    public Data littleDrunkData;
    public Data drunkData;
    public Data veryDrunkData;
}

[Serializable]
public class Data
{
    [Range(0, 1)] public float minimumValue;
    public float speedRatio;
    public float panicRatio;
    public float awarenessRatio;
    public float damageRatio;
}
