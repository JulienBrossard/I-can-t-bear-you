using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(LevelManager))]
public class LevelManagerCustomInspector : Editor
{
#if UNITY_EDITOR
    private int currentNpcCount;
    List<GameObject> npcList = new List<GameObject>();

    public override void OnInspectorGUI()
    {
        LevelManager levelManager = (LevelManager)target;

        UpdateNpcCount(levelManager);
        
        UpdateNpcGameObject(levelManager);
        
        base.OnInspectorGUI();
    }
    
    void ApplyModifications(LevelManager levelManager)
    {
        levelManager.ApplyModifications();
    }

    void UpdateNpcCount(LevelManager levelManager)
    {
        if (levelManager.level.npc != null)
        {
            int count = 0;

            for (int i = 0; i < levelManager.level.npc.Length; i++)
            {
                count += levelManager.level.npc[i].count;
            }

            if (currentNpcCount != count)
            {
                currentNpcCount = count;
                levelManager.level.npcCount = currentNpcCount;
                ApplyModifications(levelManager);
            }
        }
        else if(currentNpcCount != 0)
        {
            currentNpcCount = 0;
            levelManager.level.npcCount = currentNpcCount;
            ApplyModifications(levelManager);
        }
        EditorGUILayout.LabelField("NPC Count: " + currentNpcCount);
        serializedObject.ApplyModifiedProperties();
    }

    void UpdateNpcGameObject(LevelManager levelManager)
    {
        if (levelManager.level.npc != null)
        {
            List<GameObject> npc = new List<GameObject>();
            for (int i = 0; i < levelManager.level.npc.Length; i++)
            {
                npc.Add(levelManager.level.npc[i].npc);
            }

            if (npc.Count != npcList.Count)
            {
                npcList = npc;
                ApplyModifications(levelManager);
            }
            else
            {
                for (int i = 0; i < npc.Count; i++)
                {
                    if (npc[i] != npcList[i])
                    {
                        npcList = npc;
                        ApplyModifications(levelManager);
                        break;
                    }
                }
            }
        }
    }

    private void OnSceneGUI()
    {
        LevelManager levelManager = (LevelManager)target;
        Handles.color = Color.white;
        Handles.DrawWireArc (levelManager.level.partyData.partyPosition.position, Vector3.up, Vector3.forward, 360, levelManager.level.partyData.radius);
    }
#endif
}
