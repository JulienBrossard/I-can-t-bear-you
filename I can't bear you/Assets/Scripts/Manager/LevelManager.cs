using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;
    public int currentLevel;

    public LevelData[] levels;

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
        for (int i = 0; i < levels[currentLevel-1].npcCount; i++)
        {
            NpcManager.instance.SpawnNpc();
        }
    }

    public Vector3 GetRandomNpcSpawn()
    {
        return levels[currentLevel-1].npcSpawnPositions[Random.Range(0, levels[currentLevel-1].npcSpawnPositions.Length)].position;
    }

    public LevelData GetCurrentLevel()
    {
        return levels[currentLevel-1];
    }

    public Transform GetPlayer()
    {
        return levels[currentLevel-1].player;
    }
}

[Serializable]
public class LevelData
{
    public string name;
    public int level;
    public int npcCount;
    public Transform[] npcSpawnPositions;
    [Header("Waypoint Settings")]
    public Transform[] runAwayPoints;
    public Transform[] hungerPoints;
    public Transform[] thirstPoints;
    public Transform[] bladderPoints;
    
    [Header("Player")]
    public Transform player;

    public PartyData partyData;
}

[Serializable]
public class PartyData
{
    public Transform partyPosition;
    public float radius;
}
