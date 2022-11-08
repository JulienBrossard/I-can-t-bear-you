using UnityEngine;

public class Outline : MonoBehaviour
{
    [Header("Outline Data")] [SerializeField]
    private OutlineData outlineData;
    private Renderer outlineRenderer;
    [Header("Outline Object")]
    [SerializeField] private GameObject outlineObject;
    private void Start()
    {
        CreateOutline(outlineData.outlineMaterial, outlineData.outlineScaleFactor, outlineData.outlineColor);
    }

    void CreateOutline(Material outlineMat, float scaleFactor, Color color)
    {
        Renderer rend = outlineObject.GetComponent<Renderer>();
        
        rend.material = outlineMat;
        rend.material.SetColor("OutlineColor", color);
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
