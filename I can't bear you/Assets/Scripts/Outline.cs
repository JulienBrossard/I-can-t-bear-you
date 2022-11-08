using System;
using UnityEngine;

public class Outline : MonoBehaviour
{
    [Header("Outline Data")]
    [SerializeField] private Material outlineMaterial;
    [SerializeField] private float outlineScaleFactor;
    [SerializeField] private Color outlineColor;
    private Renderer outlineRenderer;
    [Header("Outline Object")]
    [SerializeField] private GameObject outlineObject;
    private void Start()
    {
        CreateOutline(outlineMaterial, outlineScaleFactor, outlineColor);
    }

    void CreateOutline(Material outlineMat, float scaleFactor, Color color)
    {
        Renderer rend = outlineObject.GetComponent<Renderer>();
        
        rend.material = outlineMat;
        rend.material.SetColor("OutelineColor", color);
        rend.material.SetFloat("Scale", scaleFactor);
        rend.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;

        rend.enabled = true;
    }
    
    [ContextMenu("Enabled Outline")]
    void EnabledOutline()
    {
        outlineObject.SetActive(true);
    }
    
    [ContextMenu("Disabled Outline")]
    void DisabledOutline()
    {
        outlineObject.SetActive(false);
    }
}
