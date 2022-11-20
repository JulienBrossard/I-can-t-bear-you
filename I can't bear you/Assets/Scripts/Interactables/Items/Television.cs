using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Television : Item, ISmashable, IInteractable
{
    public bool functioning = false;
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

    private void Switch()
    {
        functioning = !functioning;
    }
}
