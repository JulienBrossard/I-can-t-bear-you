using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BearserkerState : PlayerState
{
    [SerializeField] private PlayerState stealthState;
    public override void Behave()
    {
        if (inputManager.interactDown)
        {
            //Tapotage très fort sur le front
        }
        if (inputManager.roarDown)
        {
            Debug.Log("Switching to Stealth");
            playerStateManager.SwitchState(stealthState);
        }
    }

    public override void FixedBehave()
    {
        Move();
    }
}