using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SightManager : MonoBehaviour
{
    [SerializeField] private InterestPointsManager interestPointsManager;
    [SerializeField] private SphereCollider collider;
    private PlayerState currentState;

    public void UpdateState(PlayerState newPlayerState)
    {
        currentState = newPlayerState;
        collider.radius = currentState.playerStats.detectionRange;
    }

    private float dot;
    private void OnTriggerStay(Collider other)
    {
        if(currentState.heldObject != default) return;
        if (other.GetComponent<IInteractable>() == default && other.GetComponent<ISmashable>() == default) return;
        dot = Mathf.InverseLerp(1,-1,Vector3.Dot((other.transform.position - transform.position).normalized, transform.forward));
        
        if(dot > currentState.playerStats.detectionAngle/360f) return;
        interestPointsManager.AddInterestPoint(new InterestPoint(other.gameObject, Mathf.InverseLerp(0,currentState.playerStats.detectionRange,Vector3.Distance(transform.position,other.transform.position)),dot,currentState.playerStats.detectionRangeCurve,currentState.playerStats.detectionAngleCurve));
    }
}
