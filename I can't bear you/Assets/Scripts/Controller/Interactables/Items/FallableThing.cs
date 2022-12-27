using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FallableThing : Item, IInteractable
{
    public void Interact(Vector3 sourcePos)
    {
        Fall(sourcePos);
    }
}
