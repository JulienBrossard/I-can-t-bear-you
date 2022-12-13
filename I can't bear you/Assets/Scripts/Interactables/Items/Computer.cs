using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Computer : Item, ISmashable
{
    public void Smash()
    {
        Debug.Log("Breaking the Computer");
        Electrocute();
    }
}
