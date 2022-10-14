using UnityEngine;

public class NpcManager : MonoBehaviour
{
    public static NpcManager instance;
    public int npcCount;
    
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this);
        }
        instance = this;
    }

    [ContextMenu("Spawn Npc")]
    public void SpawnNpc()
    {
        Pooler.instance.Pop("Npc").transform.position = LevelManager.instance.GetRandomNpcSpawn();
        npcCount++;
    }
    
    [ContextMenu("UnSpawn Npc")]
    public void UnSpawnNpc(GameObject npc)
    {
        Pooler.instance.DePop("Npc", npc);
        npcCount--;
    }
}
