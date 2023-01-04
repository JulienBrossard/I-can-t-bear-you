using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sink : Item, ISmashable
{
    public void Smash()
    {
        Debug.Log("Breaking the Sink");
        CreatePuddle();
    }
}
