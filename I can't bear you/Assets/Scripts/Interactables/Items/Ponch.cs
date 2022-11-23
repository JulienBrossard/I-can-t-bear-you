using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ponch : Item,IInteractable,ISmashable
{
    public void Interact()
    {
        Debug.Log("Interacting Ponch");
    }

    public void Smash()
    {
        Debug.Log("Breaking the ponch");
        CreatePuddle();
        DeleteItem();
    }
}
