using DG.Tweening;
using UnityEngine;

public class WallUpdater : MonoBehaviour
{
    private Renderer objRenderer;
    [SerializeField] private Material[] transparentMaterials;
    [SerializeField] private bool wallInRoomYouStart = false;
    private Material[] opaqueMaterials;

    private void Start()
    {
        objRenderer = gameObject.GetComponent<Renderer>();
        opaqueMaterials = objRenderer.materials;
        if (wallInRoomYouStart)
        {
            HideWalls();
        }
    }
    public void ShowWalls()
    {
        for (int x = 0; x < opaqueMaterials.Length; x++)
        {
            transparentMaterials[x].DOFade(1, 0.1f);
        }
    }
    
    public void HideWalls()
    {
        SwitchToTransparentMaterials();
        foreach (var mat in transparentMaterials)
        {
            mat.DOFade(0, 0.1f).OnComplete (() => SwitchToOpaqueMaterials());
        }
    }
    
    
    private void SwitchToTransparentMaterials()
    {
        for (int i = 0; i < objRenderer.materials.Length; i++)
        {
            objRenderer.materials[i] = transparentMaterials[i];
        }
    }
    
    
    private void SwitchToOpaqueMaterials()
    {
        for (int i = 0; i < objRenderer.materials.Length; i++)
        {
            objRenderer.materials[i] = opaqueMaterials[i];
        }
    }

}
