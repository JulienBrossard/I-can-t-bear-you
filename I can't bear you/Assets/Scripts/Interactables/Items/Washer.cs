using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Washer : Item, ISmashable
{
    public void Smash()
    {
        Debug.Log("Breaking the Wasger");
        Electrocute();
    }
}
