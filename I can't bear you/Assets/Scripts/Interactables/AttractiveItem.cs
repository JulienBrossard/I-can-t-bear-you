using UnityEngine;

public class AttractiveItem : Item, IInteractable, ISmashable
{
    [Header("Television Attraction")] 
    [SerializeField] public Awareness awareness;
    public bool functioning = false;
    public float attractedDistance = 5f;
    public int npcCount;
    [Range(0,180)]
    [SerializeField] public float angle;
    [SerializeField] public bool invertZAxis;
    
    private void Update()
    {
        if (functioning)
        {
            Attracted();
        }
    }
    
    [ContextMenu("Switch")]
    public virtual void Switch()
    {
        functioning = !functioning;
        if (!functioning)
        {
            StopAttracted();
        }
    }
    
    public virtual void Attracted()
    {
        if (npcCount != awareness.visibleTargets.Count)
        {
            for (int i = 0; i < awareness.visibleTargets.Count; i++)
            {
                Npc npc = awareness.visibleTargets[i].GetComponent<Npc>();
                if (npc.state == Npc.STATE.PARTY)
                {
                    if (!invertZAxis)
                    {
                        npc.Attracted(attractedDistance * - Mathf.Sign(Vector3.Dot(transform.forward, Vector3.forward)), transform.position, angle);
                    }
                    else
                    {
                        npc.Attracted(-attractedDistance * - Mathf.Sign(Vector3.Dot(transform.forward, Vector3.forward)), transform.position, angle);
                    }
                }
            }

            npcCount = awareness.visibleTargets.Count;
        }
    }
    
    public virtual void StopAttracted()
    {
        for (int i = 0; i < awareness.visibleTargets.Count; i++)
        {
            awareness.visibleTargets[i].GetComponent<Npc>().StopAttracted();
        }

        npcCount = 0;
    }
    
    public Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal) {
        if (!angleIsGlobal) {
            angleInDegrees += transform.eulerAngles.y;
        }
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad),0,Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }

    public void Smash()
    {
        if (charged) return;
        Electrocute();
    }

    public void Interact(Vector3 sourcePos)
    {
        Debug.Log("Interacting with " + gameObject.name);
        Switch();
    }
}
