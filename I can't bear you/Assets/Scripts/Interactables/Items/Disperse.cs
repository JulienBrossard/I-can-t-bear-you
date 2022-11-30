using UnityEngine;

public class Disperse : Awareness
{
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
                    if (NpcManager.instance.npcScriptDict[target.gameObject].state != Npc.STATE.DANCING && NpcManager.instance.npcScriptDict[target.gameObject].state != Npc.STATE.ATTRACTED)
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
}
