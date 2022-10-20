using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BearserkerState : PlayerState
{
    [SerializeField] private PlayerState stealthState;
    public override void Behave()
    {
        if (InputManager.instance.input.Actions.Smash.triggered)
        {
            //Tapotage sur le front
        }
        if (InputManager.instance.input.Actions.Roar.triggered)
        {
            //Temporaire pour le debug
            Debug.Log("Switching to Bearserker");
            playerStateManager.SwitchState(stealthState);
        }
    }

    public override void FixedBehave()
    {
        Move();
    }
}