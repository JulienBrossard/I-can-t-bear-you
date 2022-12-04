using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Rendering;

public class WallUpdater : MonoBehaviour
{
    [SerializeField] private Renderer[] wallToShow;
    [SerializeField] private Renderer[] wallToHide;



    public void UpdateWallOnEnter()
    {
        foreach (var wall in wallToHide)
        {
            wall.shadowCastingMode = ShadowCastingMode.ShadowsOnly;
            
        }
        foreach (var wall in wallToShow)
        {
         wall.shadowCastingMode = ShadowCastingMode.On;
        }
    }

    public void UpdateWallOnExit()
    {
        foreach (var wall in wallToHide)
        {
            wall.shadowCastingMode = ShadowCastingMode.On;

        }
        foreach (var wall in wallToShow)
        {
            wall.shadowCastingMode = ShadowCastingMode.ShadowsOnly;
        }
    }
    
    
}
