using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BookCase : Item, ISmashable, IInteractable
{
    public void Interact(Vector3 sourcePos)
    {
        Debug.Log("Interacting BookCase");
        Fall(sourcePos);
    }
    public void Smash()
    {
        Debug.Log("Breaking the BookCase");
        Destroy(this);
    }
}
