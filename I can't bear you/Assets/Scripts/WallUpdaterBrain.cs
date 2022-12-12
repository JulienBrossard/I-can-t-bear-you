using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallUpdaterBrain : MonoBehaviour
{
    [SerializeField] private WallUpdater[] wallUpdatersToShow;
    [SerializeField] private WallUpdater[] wallUpdatersToHide;
    
    public void UpdateWallsOnEnter()
    {
        foreach (var wallUpdater in wallUpdatersToShow)
        {
            wallUpdater.ShowWalls();
        }
        
        foreach (var wallUpdater in wallUpdatersToHide)
        {
            wallUpdater.HideWalls();
        }
    }
    public void UpdateWallsOnExit()
    {
        foreach (var wallUpdater in wallUpdatersToShow)
        {
            wallUpdater.HideWalls();
        }
        
        foreach (var wallUpdater in wallUpdatersToHide)
        {
            wallUpdater.ShowWalls();
        }
    }
}
