using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class StealthState : PlayerState
{
    [SerializeField] private PlayerState bearserkerState;
    public override void OnStateEnter()
    {
        PlayerAnimatorManager.instance.SetAnimatorBool("Bearserker", false);
    }
    public override void Behave()
    {
        if (InputManager.instance.input.Actions.Interact.triggered)
        {
            if (heldObject?.GetComponent<IInteractable>() != default)
            {
                heldObject.GetComponent<IInteractable>().Interact();
                return;
            }
            interestPointsManager.GetInteractable()?.Interact();
        }
        if (InputManager.instance.input.Actions.Smash.triggered)
        {
            interestPointsManager.GetSmashable()?.Smash();
        }
        if (InputManager.instance.input.Actions.Grab.triggered)
        {
            if (heldObject == default)
            {
                if(interestPointsManager.GetGrabbable() == null) return;
                heldObject = interestPointsManager.GetGrabbable().Grab(handTransform).gameObject;
            }
            else
            {
                heldObject.GetComponent<IGrabbable>().Drop();
                heldObject = null;
            }
        }
        if (InputManager.instance.input.Actions.Roar.triggered)
        {
            Debug.Log("Switching to Bearserker");
            heldObject?.GetComponent<IGrabbable>().Drop();
            playerStateManager.SwitchState(bearserkerState);
        }
    }

    public override void FixedBehave()
    {
        Move();
        PlayerAnimatorManager.instance.SetAnimatorFloat("Speed", rb.velocity.magnitude);
        LookForInterestPoints(playerStats.detectionAngle,playerStats.detectionRange,playerStats.detectionStep);
        PlayerAnimatorManager.instance.SetAnimatorFloat("Speed", rb.velocity.magnitude);
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
