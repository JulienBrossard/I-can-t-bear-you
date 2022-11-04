using System;
using UnityEngine;
using UnityEngine.Serialization;

public class NpcManager : MonoBehaviour
{
    public static NpcManager instance;
    public int npcCountRemaining;
    public int npcCountMax;
    
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
        Pooler.instance.Pop(name).transform.position = LevelManager.instance.GetRandomNpcSpawn();
        npcCountRemaining++;
        if (npcCountRemaining == LevelManager.instance.level.npcCount)
        {
            UiManager.instance.UpdateRemainingNpcText();
        }
    }
    
    [ContextMenu("UnSpawn Npc")]
    public void UnSpawnNpc(String name, GameObject npc)
    {
        Pooler.instance.DePop(name, npc);
        npcCountRemaining--;
        UiManager.instance.UpdateRemainingNpcText();
    }
}
