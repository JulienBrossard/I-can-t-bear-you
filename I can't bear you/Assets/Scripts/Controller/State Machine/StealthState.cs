using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class StealthState : PlayerState
{
    [SerializeField] private PlayerState bearserkerState;
    protected override void OnStateEnter()
    {
        
    }

    public override void Behave()
    {
        if (InputManager.instance.input.Actions.Interact.triggered)
        {
            interestPointsManager.GetInteractable()?.Interact();
        }
        if (InputManager.instance.input.Actions.Smash.triggered)
        {
            interestPointsManager.GetSmashable()?.Smash();
        }
        if (InputManager.instance.input.Actions.Grab.triggered)
        {
            interestPointsManager.GetGrabbable()?.Grab();
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
        LookForInterestPoints(playerStats.detectionAngle,playerStats.detectionRange,playerStats.detectionStep);
    }

    protected override void SendRayCast(Vector3 origin, Vector3 dir, float length, float centerDistance)
    {
        RaycastHit hit;
        if (Physics.Raycast(origin, dir, out hit, length))
        {
            if (hit.collider.GetComponent<IInteractable>() != default || hit.collider.GetComponent<ISmashable>() != default || hit.collider.GetComponent<IGrabbable>() != default)
            {
                interestPointsManager.AddInterestPoint(new InterestPoint(hit.collider.gameObject, hit.distance,centerDistance));
                Debug.DrawRay(origin, dir * hit.distance, Color.blue);
                return;
            }
            Debug.DrawRay(origin, dir * length, Color.green);
            return;
        }
        Debug.DrawRay(origin, dir * length, Color.green);
        return;
    }
}
