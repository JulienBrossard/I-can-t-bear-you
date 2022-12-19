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
        
        /*GUI.enabled = false;
        var scriptProp = so.FindProperty("m_Script");
        EditorGUILayout.PropertyField(scriptProp);
        GUI.enabled = true;*/
 
        // Draw subclass props
        base.OnInspectorGUI();
    }
    
    void ApplyModifications(LevelManager levelManager)
    {
        levelManager.ApplyModifications();
    }

    void UpdateNpcCount(LevelManager levelManager)
    {
        if (levelManager.level.partyData != null)
        {
            int count = 0;

            for (int i = 0; i < levelManager.level.partyData.Length; i++)
            {
                if (levelManager.level.partyData[i] != null && levelManager.level.partyData[i].npc != null)
                {
                    for (int j = 0; j < levelManager.level.partyData[i].npc.Length; j++)
                    {
                        count += levelManager.level.partyData[i].npc[j].count;
                    }
                }
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
        if (levelManager.level.partyData != null)
        {
            List<GameObject> npc = new List<GameObject>();
            for (int i = 0; i < levelManager.level.partyData.Length; i++)
            {
                if (levelManager.level.partyData[i] != null && levelManager.level.partyData[i].npc != null)
                {
                    for (int j = 0; j < levelManager.level.partyData[i].npc.Length; j++)
                    {
                        npc.Add(levelManager.level.partyData[i].npc[j].npc);
                    }
                }
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
        if (levelManager.level.partyData != null)
        {
            for (int i = 0; i < levelManager.level.partyData.Length; i++)
            {
                if (levelManager.level.partyData[i] != null)
                {
                    if (levelManager.level.partyData[i].partyPosition != null )
                    {
                        Handles.DrawWireArc (levelManager.level.partyData[i].partyPosition.position, Vector3.up, Vector3.forward, 360, levelManager.level.partyData[i].radius);
                    }
                }
            }
        }
    }
#endif
}
