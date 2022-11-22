using UnityEngine;
using UnityEditor;

[CustomEditor (typeof (Awareness))]
public class AwarenessEditor : Editor
{
    void OnSceneGUI() {
        Display();
    }

    public virtual void Display()
    {
        var awareness = (Awareness)target;
        Handles.color = Color.white;
        Handles.DrawWireArc (awareness.transform.position, Vector3.up, Vector3.forward, 360, awareness.viewRadius);
        Vector3 viewAngleA = awareness.DirFromAngle (-awareness.viewAngle / 2, false);
        Vector3 viewAngleB = awareness.DirFromAngle (awareness.viewAngle / 2, false);

        Handles.DrawLine (awareness.transform.position, awareness.transform.position + viewAngleA * awareness.viewRadius);
        Handles.DrawLine (awareness.transform.position, awareness.transform.position + viewAngleB * awareness.viewRadius);

        Handles.color = Color.red;
        if (awareness.visibleTargets.Count > 0)
        {
            for (int i = 0; i < awareness.visibleTargets.Count; i++)
            {
                Handles.DrawLine (awareness.transform.position, awareness.visibleTargets[i].position);
            }
        }
    }
}

[CustomEditor (typeof(AwarenessNpc))]
public class AwarenessNpcEditor : AwarenessEditor
{
    void OnSceneGUI()
    {
        base.Display();
    }
}
