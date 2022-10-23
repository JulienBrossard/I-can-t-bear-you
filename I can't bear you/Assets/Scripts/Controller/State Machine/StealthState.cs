using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class StealthState : PlayerState
{
    [SerializeField] private PlayerState bearserkerState;
    public override void Behave()
    {
        if (InputManager.instance.input.Actions.Interact.triggered)
        {
            Debug.Log("Interacting with " + interestPointsManager.interactables[0].go);
        }
        if (InputManager.instance.input.Actions.Smash.triggered)
        {
            //Tapotage sur le front
        }
        if (InputManager.instance.input.Actions.Roar.triggered)
        {
            Debug.Log("Switching to Bearserker");
            playerStateManager.SwitchState(bearserkerState);
        }
    }

    public override void FixedBehave()
    {
        Move();
        LookForInterestPoints(playerStats.detectionAngle,playerStats.detectionRange,playerStats.detectionStep, InterestType.INTERACTABLE);
        //LookForInterestPoints(playerStats.detectionAngle,playerStats.detectionRange,playerStats.detectionStep, InterestType.SMASHABLE);
    }
}
