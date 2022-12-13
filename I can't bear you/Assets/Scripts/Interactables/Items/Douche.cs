using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Douche : Item,IInteractable,ISmashable
{
    public void Interact(Vector3 sourcePos)
    {
        Debug.Log("Interacting douche");
        CreatePuddle();
    }

    public void Smash()
    {
        Debug.Log("Breaking the douche");
        CreatePuddle();
    }
}
