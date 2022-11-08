using UnityEngine;

[CreateAssetMenu(fileName = "Outline Data", menuName = "Outline Data")]
public class OutlineData : ScriptableObject
{
    public Material outlineMaterial;
    public float outlineScaleFactor;
    public Color outlineColor;
}
