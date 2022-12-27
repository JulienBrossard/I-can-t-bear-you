using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestoyableObject : Item, ISmashable
{
  public void Smash()
    {
        Debug.Log("Obj has been Destroyed");
        DeleteItem();
    }
}
