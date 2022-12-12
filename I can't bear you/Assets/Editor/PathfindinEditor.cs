using UnityEditor;
using UnityEngine;

[CustomEditor (typeof (Npc))]
public class PathfindinEditor : Editor
{
#if UNITY_EDITOR
    void OnSceneGUI() {
        Npc npc = (Npc)target;
        Handles.color = Color.blue;
        for (int i = 0; i < npc.agent.path.corners.Length-1; i++)
        {
            Handles.DrawLine(npc.agent.path.corners[i], npc.agent.path.corners[i+1]);
        }
    }
#endif
}
