using System;
using System.Collections.Generic;
using UnityEngine;
using Debug = UnityEngine.Debug;
using Random = UnityEngine.Random;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;
    [SerializeField] private Pooler pooler;
    public LevelData level;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(instance);
        }
        instance = this;
    }

    private void Start()
    {
        //Init Level (Spawn NPC)
        for (int i = 0; i < level.partyData.Length; i++)
        {
            for (int j = 0; j < level.partyData[i].npc.Length; j++)
            {
                for (int k = 0; k < level.partyData[i].npc[j].count; k++)
                {
                    NpcManager.instance.SpawnNpc(level.partyData[i].npc[j].npc.name).InitPartyData(level.partyData[i], NpcManager.instance.npcCountRemaining); //Spawn NPC + loaded party data
                }
            }
        }
    }

    /// <summary>
    /// Spawn Npc to Random point
    /// </summary>
    /// <returns></returns>
    public Vector3 GetRandomNpcSpawn()
    {
        return level.npcSpawnPositions[Random.Range(0, level.npcSpawnPositions.Length)].position;
    }

    /// <summary>
    /// Get Player
    /// </summary>
    /// <returns></returns>
    public Transform GetPlayer()
    {
        return level.player;
    }
    
    /// <summary>
    /// Remove Exit point from npc list
    /// </summary>
    /// <param name="exitPoint"> Exit point to remove</param>
    public void RemoveExitPoint(Transform exitPoint)
    {
        foreach (var npc in NpcManager.instance.npc)
        {
            Debug.Log(npc);
            NpcManager.instance.npcScriptDict[npc].RemoveExitPoint(exitPoint);
        }
        Debug.Log(exitPoint.name + " removed");
    }

    /// <summary>
    /// Apply modifications to inspector
    /// </summary>
    public void ApplyModifications()
    {
        if (level.partyData != null)
        {
            pooler.poolKeys = new List<Pooler.PoolKey>();
            bool found;
            for (int j = 0; j < level.partyData.Length; j++)
            {
                if (level.partyData[j].npc == null)
                {
                    continue;
                }
                for (int i = 0; i < level.partyData[j].npc.Length; i++)
                {
                    if (level.partyData[j].npc[i].npc != null)
                    {
                        found = false;
                        
                        //Check if pool key already exists
                        for (int k = 0; k < pooler.poolKeys.Count; k++)
                        {
                            if (pooler.poolKeys[k].key == level.partyData[j].npc[i].npc.name)
                            {
                                pooler.poolKeys[k].pool.baseCount += level.partyData[j].npc[i].count;
                                found = true;
                                break;
                            }
                        }

                        // Create new pool key if not already exists
                        if (!found)
                        {
                            pooler.poolKeys.Add(new Pooler.PoolKey()
                            {
                                key = level.partyData[j].npc[i].npc.name,
                                pool = new Pooler.Pool(){ prefab = level.partyData[j].npc[i].npc,baseCount = level.partyData[j].npc[i].count,baseRefreshSpeed = 5,refreshSpeed = 5}
                            });
                        }
                    }
                }
            }
            Debug.Log("Apply modifications successful");
        }
        else
        {
            Debug.LogWarning("Party Data List is empty");
        }
    }

    /// <summary>
    /// End Level
    /// </summary>
    /// <param name="bySleeping"> ? </param>
    public void EndLevel(bool bySleeping)
    {
        if (NpcManager.instance.npcCountkilled < level.requiredNpcKillCount)
        {
            UiManager.instance.LaunchEndLevelScreen(true);
        }
        else
        {
            UiManager.instance.LaunchEndLevelScreen(false);
        }
    }
}

[Serializable]
public class LevelData
{
    [Header("Party Data")]
    public PartyData[] partyData;
    [HideInInspector] public int npcCount;
    public int requiredNpcKillCount;
    
    [Header("Waypoint Settings")]
    public Transform[] npcSpawnPositions;
    public Transform[] notExitPoints;
    public Transform[] exitPoints;
    public Transform[] hungerPoints;
    public Transform[] thirstPoints;
    public Transform[] bladderPoints;
    
    [Header("Player")]
    public Transform player;
}

[Serializable]
public class PartyData
{
    public enum Shape
    {
        CIRCLE,
        RECTANGLE
    }
    public Shape shape = Shape.CIRCLE;
    public Transform partyPosition;
    [ConditionalEnumHide("shape", 0)] public float radius;
    [ConditionalEnumHide("shape", 1)] public float width;
    [ConditionalEnumHide("shape", 1)] public float length;
    public SpawnNpc[] npc;
}

[Serializable]
public class SpawnNpc
{
    public GameObject npc;
    public int count;
}
