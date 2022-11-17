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

    public void Interact()
    {
        Debug.Log("Interacting with Television");
        Switch();
    }

    private void Switch()
    {
        functioning = !functioning;
    }

    private void OnTriggerEnter(Collider other)
    {
        //Attirer les PNJ vers la télévision
    }
}
