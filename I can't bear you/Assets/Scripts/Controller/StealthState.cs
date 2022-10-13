using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StealthState : PlayerState
{
    [SerializeField] private PlayerState bearserkerState;
    public override void Behave()
    {
        if (inputManager.interactDown)
        {
            //Sabotage
        }
        if (inputManager.roarDown)
        {
            Debug.Log("Switching to Bearserker");
            playerStateManager.SwitchState(bearserkerState);
        }
    }

    public override void FixedBehave()
    {
        Move();
    }
}
