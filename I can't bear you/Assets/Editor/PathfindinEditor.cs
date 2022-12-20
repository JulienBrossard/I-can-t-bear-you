using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

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
    
    private void OnEnable()
    {
        Npc npc = (Npc)target;
        npc.animator = npc.gameObject.GetComponent<Animator>();
        npc.agent = npc.gameObject.GetComponent<NavMeshAgent>();
        npc.npcScripts = npc.gameObject.GetComponent<NpcScripts>();
    }
#endif
}
