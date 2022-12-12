using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoneyPot : Item,IInteractable,ISmashable
{
    [SerializeField, Range(0f, 1f)] private float bearserkerToAdd;
    public void Interact(Vector3 sourcePos)
    {
        Debug.Log("Eating Honey Pot");
        BearserkerGaugeManager.instance.AddBearserker(bearserkerToAdd);
    }

    public void Smash()
    {
        Break();
    }

    public override void OnHitGround()
    {
        if (thrown) Break();
        base.OnHitGround();
    }

    private void Break()
    {
        Debug.Log("Breaking Honey Pot");
        CreatePuddle();
        DeleteItem();
    }
}
