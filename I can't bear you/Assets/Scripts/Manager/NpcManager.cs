using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class NpcManager : MonoBehaviour
{
    public static NpcManager instance;
    public int npcCountRemaining;
    public int npcCountkilled;
    public int npcCountfleed;
    public List<GameObject> npc = new List<GameObject>();
    public Dictionary<GameObject, Panic> panicDict = new Dictionary<GameObject, Panic>();
    public Dictionary<GameObject, Npc> npcScriptDict = new Dictionary<GameObject, Npc>();

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this);
        }
        instance = this;
    }

    [ContextMenu("Spawn Npc")]
    public Npc SpawnNpc(String name)
    {
        GameObject currentNpc = Pooler.instance.Pop(name);
        npc.Add(currentNpc);
        npc[^1].transform.position = LevelManager.instance.GetRandomNpcSpawn();
        npcCountRemaining++;
        if (npcCountRemaining == LevelManager.instance.level.npcCount)
        {
        }
        UiManager.instance.UpdateRemainingNpcText();
        panicDict.Add(npc[^1], npc[^1].GetComponent<Panic>());
        npcScriptDict.Add(npc[^1], npc[^1].GetComponent<Npc>());
        return npcScriptDict[npc[^1]];
    }

    [ContextMenu("Npc")]
    public void UnSpawnNpc(String name, GameObject npc)
    {
        panicDict.Remove(npc);
        npcScriptDict.Remove(npc);
        this.npc.Remove(npc);
        Pooler.instance.DePop(name, npc);
        npcCountRemaining--;
        npcCountfleed++;
        CheckForLvlEnd();
        UiManager.instance.UpdateRemainingNpcText();
    }
    
    public void CheckForLvlEnd()
    {
        if (npcCountRemaining == 0)
        {
            LevelManager.instance.EndLevel(false);
        }
    }

    public void SetDispersePoint(Vector3 center, float radius, Disperse.DisperseType type)
    {
        foreach (var npc in npcScriptDict.Values)
        {
            npc.pathfinding.dispersePoints.Add(center, radius);
        }
    }
    
    public void RemoveDispersePoint(Vector3 center, Disperse.DisperseType type)
    {
        foreach (var npc in npcScriptDict.Values)
        {
            npc.pathfinding.dispersePoints.Remove(center);
        }
    }
}
