using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ponch : Item,IInteractable,ISmashable
{
    public void Interact()
    {
        if(poisoned) return;
        Debug.Log("Poisoning Ponch");
        Poison();
    }

    public void Smash()
    {
        Debug.Log("Breaking the ponch");
        CreatePuddle();
        DeleteItem();
    }
}
