using System.Collections;
using DG.Tweening;
using UnityEngine;

public class WallUpdater : MonoBehaviour
{
    private Renderer objRenderer;
    private Material[] transparentMaterials;
    [SerializeField] private bool wallInRoomYouStart = false;
    [SerializeField]private Material[] opaqueMaterials;
    [SerializeField] private float timeToFade = 1f;
    private void Start()
    {
        objRenderer = gameObject.GetComponent<Renderer>();
        transparentMaterials = objRenderer.materials;
        if (wallInRoomYouStart)
        {
            HideWalls();
        }
        else
        {
            SwitchToOpaqueMaterials();
        }
    }
    public void ShowWalls()
    {
        foreach (var mat in transparentMaterials)
        {
            mat.DOFade(0, 0f);
            mat.DOFade(1, timeToFade);
        }

        StartCoroutine(WaitBeforeSwitchingToOpaqueMaterials());
    }
    
    public void HideWalls()
    {
        Debug.Log("HideWalls");
        SwitchToTransparentMaterials();
        foreach (var mat in transparentMaterials)
        {
//            mat.DOFade(1, 0f);
            mat.DOFade(0, timeToFade);
        }
    }
    
    
    private void SwitchToTransparentMaterials()
    {
        objRenderer.materials = transparentMaterials;
    }
    
    
    private void SwitchToOpaqueMaterials()
    {
        objRenderer.materials = opaqueMaterials;
    }

    IEnumerator WaitBeforeSwitchingToOpaqueMaterials()
    {
        yield return new WaitForSeconds(timeToFade);
        SwitchToOpaqueMaterials();
    }
}
