using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Television : Item, ISmashable, IInteractable
{
    [SerializeField] private Awareness awareness;
    public bool functioning = false;
    [SerializeField] float attractedDistance = 5f;
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
        if (functioning)
        {
            for (int i = 0; i < awareness.visibleTargets.Count; i++)
            {
                awareness.visibleTargets[i].GetComponent<Npc>().Attracted(attractedDistance, transform.position);
            }
        }
        else
        {
            for (int i = 0; i < awareness.visibleTargets.Count; i++)
            {
                awareness.visibleTargets[i].GetComponent<Npc>().StopAttracted();
            }
        }
    }
}
