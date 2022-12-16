using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.Serialization;
using UnityEngine;
using UnityEngine.Rendering;
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
        for (int i = 0; i < level.partyData.Length; i++)
        {
            for (int j = 0; j < level.partyData[i].npc.Length; j++)
            {
                for (int k = 0; k < level.partyData[i].npc[j].count; k++)
                {
                    NpcManager.instance.SpawnNpc(level.partyData[i].npc[j].npc.name).InitPartyData(level.partyData[i]);
                }
            }
        }
    }

    public Vector3 GetRandomNpcSpawn()
    {
        return level.npcSpawnPositions[Random.Range(0, level.npcSpawnPositions.Length)].position;
    }

    public Transform GetPlayer()
    {
        return level.player;
    }
    
    public void RemoveExitPoint(Transform exitPoint)
    {
        foreach (var npc in NpcManager.instance.npc)
        {
            Debug.Log(npc);
            NpcManager.instance.npcScriptDict[npc].RemoveExitPoint(exitPoint);
        }
    }

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
                        for (int k = 0; k < pooler.poolKeys.Count; k++)
                        {
                            if (pooler.poolKeys[k].key == level.partyData[j].npc[i].npc.name)
                            {
                                pooler.poolKeys[k].pool.baseCount += level.partyData[j].npc[i].count;
                                found = true;
                                break;
                            }
                        }

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
    enum Shape
    {
        CIRCLE,
        RECTANGLE
    }
    [SerializeField] private Shape shape = Shape.CIRCLE;
    public bool test;
    public Transform partyPosition;
    [ConditionalEnumHide("shape", 0)] public float radius;
    public SpawnNpc[] npc;
}

[Serializable]
public class SpawnNpc
{
    public GameObject npc;
    public int count;
}
