using System;
using UnityEngine;

public class Disperse : Awareness
{
    public enum DisperseType
    {
        RANDOM
    }
    
    [SerializeField] private DisperseType disperseType;
    
    private void Update()
    {
        DisperseNpc();
    }

    void DisperseNpc()
    {
        if (visibleTargets.Count > 0)
        {
            foreach (Transform target in visibleTargets)
            {
                if (NpcManager.instance.npcScriptDict.ContainsKey(target.gameObject))
                {
                    if (NpcManager.instance.npcScriptDict[target.gameObject].state != Npc.STATE.PARTY && NpcManager.instance.npcScriptDict[target.gameObject].state != Npc.STATE.ATTRACTED)
                    {
                        continue;
                    }
                }
                Vector3 direction = transform.position - target.position;
                direction.Normalize();
                NpcManager.instance.npcScriptDict[target.gameObject].Disperse(transform.position, direction, viewRadius);
            }
        }
    }

    private void OnEnable()
    {
        NpcManager.instance.SetDispersePoint(transform.position, viewRadius, disperseType);
    }

    private void OnDisable()
    {
        NpcManager.instance.RemoveDispersePoint(transform.position, disperseType);
    }
}
