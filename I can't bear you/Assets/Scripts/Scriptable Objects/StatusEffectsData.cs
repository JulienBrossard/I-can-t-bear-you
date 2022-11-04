using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Status Effects Data", menuName = "Status Effects Data")]
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
    public float speedRatio;
    public float panicRatio;
    public float awarenessRatio;
    public float damageRatio;
}