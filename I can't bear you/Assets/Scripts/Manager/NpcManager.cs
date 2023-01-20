using System;
using System.Collections.Generic;
using UnityEngine;

public class NpcManager : MonoBehaviour
{
    public Tools.FIELD field = Tools.FIELD.HIDDEN;
    [ConditionalEnumHide("field", 0)] public static NpcManager instance;
    [ConditionalEnumHide("field", 0)] public int npcCountRemaining;
    [ConditionalEnumHide("field", 0)] public int npcCountkilled;
    [ConditionalEnumHide("field", 0)] public int npcCountfleed;
    [ConditionalEnumHide("field", 0)] public List<GameObject> npc = new List<GameObject>();
    public Dictionary<GameObject, Npc> npcScriptDict = new Dictionary<GameObject, Npc>();
    [SerializeField] private GameObject npcDisapearEffect;


    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this);
        }
        instance = this;
    }

    /// <summary>
    /// Spawn Npc
    /// </summary>
    /// <param name="name"> Npc name </param>
    /// <returns></returns>
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
        npcScriptDict.Add(npc[^1], npc[^1].GetComponent<Npc>());
        return npcScriptDict[npc[^1]];
    }

    /// <summary>
    /// Unspawn Npc
    /// </summary>
    /// <param name="name"> Npc name (Don't forget to replace (Clone) by String.Empty </param>
    /// <param name="npc"> Npc GameObject </param>
    [ContextMenu("Npc")]
    public void UnSpawnNpc(String name, GameObject npc)
    {
        Instantiate(npcDisapearEffect, npc.transform.position, Quaternion.identity);
        npcScriptDict.Remove(npc);
        this.npc.Remove(npc);
        Pooler.instance.DePop(name, npc);
        npcCountRemaining--;
        npcCountfleed++;
        CheckForLvlEnd();
        UiManager.instance.UpdateRemainingNpcText();
    }
    
    /// <summary>
    /// Check if the level is finished
    /// </summary>
    public void CheckForLvlEnd()
    {
        if (npcCountRemaining == 0)
        {
            LevelManager.instance.EndLevel(false);
        }
    }

    /// <summary>
    /// Added disperse point to all npc
    /// </summary>
    /// <param name="center"> Center of the disperse point </param>
    /// <param name="radius"> Radius of the disperse point </param>
    /// <param name="type">Type of the disperse point </param>
    public void SetDispersePoint(Vector3 center, float radius, Disperse.DisperseType type)
    {
        foreach (var npc in npcScriptDict.Values)
        {
            npc.pathfinding.dispersePoints.Add(center, radius);
        }
    }
    
    /// <summary>
    /// Remove disperse point to all npc
    /// </summary>
    /// <param name="center"> Center of the disperse point </param>
    /// <param name="type"> Type of the disperse point </param>
    public void RemoveDispersePoint(Vector3 center, Disperse.DisperseType type)
    {
        foreach (var npc in npcScriptDict.Values)
        {
            npc.pathfinding.dispersePoints.Remove(center);
        }
    }
}
