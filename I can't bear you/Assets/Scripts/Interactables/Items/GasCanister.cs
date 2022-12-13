using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GasCanister : Item, ISmashable, IAffectable
{
    public void Smash()
    {
        Debug.Log("Breaking the Gas Canister");
        CreateGas();
        Destroy(this);
    }
}
