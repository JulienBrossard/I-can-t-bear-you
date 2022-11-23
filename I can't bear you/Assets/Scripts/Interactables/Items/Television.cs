using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Television : Item, ISmashable, IInteractable
{
    [SerializeField] private Awareness awareness;
    public bool functioning = false;
    [SerializeField] float attractedDistance = 5f;
    [SerializeField] private Material tvOn;
    [SerializeField] private Material tvOff;
    [SerializeField] private MeshRenderer tvScreenMR;
    private int npcCount;

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
        if (!functioning)
        {
            StopAttracted();
        }
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
                    npc.Attracted(attractedDistance, transform.position);
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
    
}
