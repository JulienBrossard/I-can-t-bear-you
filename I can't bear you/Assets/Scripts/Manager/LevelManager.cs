using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
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
        for (int i = 0; i < level.npc.Length; i++)
        {
            for (int j = 0; j < level.npc[i].count; j++)
            {
                NpcManager.instance.SpawnNpc(level.npc[i].npc.name);
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
            npc.GetComponent<Npc>().RemoveExitPoint(exitPoint);
        }
    }

    public void ApplyModifications()
    {
        if (level.npc != null)
        {
            pooler.poolKeys = new List<Pooler.PoolKey>();
            for (int i = 0; i < level.npc.Length; i++)
            {
                pooler.poolKeys.Add(new Pooler.PoolKey()
                {
                    key = level.npc[i].npc.name,
                    pool = new Pooler.Pool(){ prefab = level.npc[i].npc,baseCount = level.npc[i].count,baseRefreshSpeed = 5,refreshSpeed = 5}
                });
            }
            Debug.Log("Apply modifications successful");
        }
        else
        {
            Debug.LogWarning("Npc List is empty");
        }
    }
}

[Serializable]
public class LevelData
{
    public SpawnNpc[] npc;
    [HideInInspector] public int npcCount;
    [Header("Waypoint Settings")]
    public Transform[] npcSpawnPositions;
    public Transform[] notExitPoints;
    public Transform[] exitPoints;
    public Transform[] hungerPoints;
    public Transform[] thirstPoints;
    public Transform[] bladderPoints;
    
    [Header("Player")]
    public Transform player;

    [Header("Party Data")]
    public PartyData partyData;
}

[Serializable]
public class PartyData
{
    public Transform partyPosition;
    public float radius;
}

[Serializable]
public class SpawnNpc
{
    public GameObject npc;
    public int count;
}
