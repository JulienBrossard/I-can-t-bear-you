using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExampleItem : Item, IInteractable
{
    public void Interact(Vector3 sourcePos)
    {
        DeleteItem();
    }

    public override void Electrocute()
    {
        Explode();
    }
}
