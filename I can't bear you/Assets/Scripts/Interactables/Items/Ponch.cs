using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ponch : Item,IInteractable,ISmashable
{


    public void Interact()
    {
        Debug.Log("Drinking ponch");
    }

    public void Smash()
    {
        Debug.Log("Breaking the battery");
        CreatePuddle();
        DeleteItem();
    }
}
