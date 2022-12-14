using System;
using UnityEditor;
using UnityEngine;

[CustomEditor (typeof (AttractiveItem), true)]
public class TelevisionEditor : ItemEditor
{
#if UNITY_EDITOR
    private void OnSceneGUI()
    {
        var television = (AttractiveItem)target;
        Handles.color = Color.blue;
        Handles.DrawWireArc (television.transform.position, Vector3.up, Vector3.forward, 360, television.attractedDistance);
        Vector3 viewAngleA = television.DirFromAngle (-television.angle / 2, false);
        Vector3 viewAngleB = television.DirFromAngle (television.angle / 2, false);
        
        int axisDir = 1;

        if (television.invertZAxis)
        {
            axisDir = -1;
        }

        Handles.DrawLine (television.transform.position, television.transform.position + viewAngleA * television.attractedDistance * axisDir);
        Handles.DrawLine (television.transform.position, television.transform.position + viewAngleB * television.attractedDistance * axisDir);
        OnSceneDraw();
    }
#endif
}
