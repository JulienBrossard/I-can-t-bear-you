using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ponch : Item,IInteractable,ISmashable
{
    [SerializeField] private GameObject psBubblePoisoned;
    public void Interact(Vector3 sourcePos)
    {
        // Spice up the Ponch
        Debug.Log("Interacting Ponch");
    }

    public void Smash()
    {
        Debug.Log("Breaking the ponch");
        CreatePuddle();
        DeleteItem();
    }
}
