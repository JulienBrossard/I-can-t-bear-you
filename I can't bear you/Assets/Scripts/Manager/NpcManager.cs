using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class NpcManager : MonoBehaviour
{
    public static NpcManager instance;
    public int npcCountRemaining;
    public int npcCountMax;
    
    public List<GameObject> npc = new List<GameObject>();
    
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this);
        }
        instance = this;
    }

    [ContextMenu("Spawn Npc")]
    public void SpawnNpc(String name)
    {
        npc.Add(Pooler.instance.Pop(name));
        npc[^1].transform.position = LevelManager.instance.GetRandomNpcSpawn();
        npcCountRemaining++;
        if (npcCountRemaining == LevelManager.instance.level.npcCount)
        {
            UiManager.instance.UpdateRemainingNpcText();
        }
    }
    
    [ContextMenu("UnSpawn Npc")]
    public void UnSpawnNpc(String name, GameObject npc)
    {
        this.npc.Remove(npc);
        Pooler.instance.DePop(name, npc);
        npcCountRemaining--;
        UiManager.instance.UpdateRemainingNpcText();
    }
}
