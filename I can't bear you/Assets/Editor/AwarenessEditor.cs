using UnityEngine;
using UnityEditor;

[CustomEditor (typeof (Awareness))]
public class AwarenessEditor : Editor
{
    void OnSceneGUI() {
        Awareness awareness = (Awareness)target;
        Handles.color = Color.white;
        Handles.DrawWireArc (awareness.transform.position, Vector3.up, Vector3.forward, 360, awareness.viewRadius);
        Vector3 viewAngleA = awareness.DirFromAngle (-awareness.viewAngle / 2, false);
        Vector3 viewAngleB = awareness.DirFromAngle (awareness.viewAngle / 2, false);

        Handles.DrawLine (awareness.transform.position, awareness.transform.position + viewAngleA * awareness.viewRadius);
        Handles.DrawLine (awareness.transform.position, awareness.transform.position + viewAngleB * awareness.viewRadius);

        Handles.color = Color.red;
        if (awareness.visibleTarget != null)
        {
            Handles.DrawLine (awareness.transform.position, awareness.visibleTarget.position);
        }
    }
}
