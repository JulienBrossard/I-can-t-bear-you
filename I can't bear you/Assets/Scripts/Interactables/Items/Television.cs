using UnityEngine;

public class Television : Item, ISmashable, IInteractable
{
    [Header("Television Attraction")] 
    [SerializeField] private Awareness awareness;
    public bool functioning = false;
    private Material[] tempMatList;
    [SerializeField] private MeshRenderer tvScreenMR;
    [SerializeField] private Material tvOff;
    [SerializeField] private Material tvOn;
    public float attractedDistance = 5f;
    private int npcCount;
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

    public void Smash()
    {
        if (charged) return;
        Electrocute();
    }

    public override void Electrocute()
    {
        base.Electrocute();
        CreateZone();
        Debug.Log("Creating Zone");
    }

    public void Interact()
    {
        Debug.Log("Interacting with Television");
        Switch();
    }

    [ContextMenu("Switch")]
    private void Switch()
    {
        functioning = !functioning;
        tempMatList = tvScreenMR.materials;
        if (!functioning)
        {
            tempMatList[1] = tvOff;
            StopAttracted();
        }
        else
        { 
            tempMatList[1] = tvOn;
        }
        tvScreenMR.materials = tempMatList;
    }

    private void Attracted()
    {
        if (npcCount != awareness.visibleTargets.Count)
        {
            for (int i = 0; i < awareness.visibleTargets.Count; i++)
            {
                Npc npc = awareness.visibleTargets[i].GetComponent<Npc>();
                if (npc.state == Npc.STATE.DANCING)
                {
                    if (!invertZAxis)
                    {
                        npc.Attracted(attractedDistance, transform.position, angle);
                    }
                    else
                    {
                        npc.Attracted(-attractedDistance, transform.position, angle);
                    }
                }
            }

            npcCount = awareness.visibleTargets.Count;
        }
    }
    private void StopAttracted()
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
}
