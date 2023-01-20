using System;
using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

public class SightManager : MonoBehaviour
{
    [SerializeField] private InterestPointsManager interestPointsManager;
    [SerializeField] private SphereCollider collider;
    private PlayerState currentState;
    [Dracau.ReadOnly]public State state;

    public void UpdateState(PlayerState newPlayerState)
    {
        currentState = newPlayerState;
        if (currentState == transform.parent.GetComponent<BearserkerState>()) state = State.Bearserker;
        else state = State.Stealth;
        collider.radius = currentState.playerStats.detectionRange;
    }

    private float dot;
    private void OnTriggerStay(Collider other)
    {
        Vector3 hitPoint = other.ClosestPoint(transform.position);
        if(currentState.heldObject != default) return;
        if (state == State.Stealth)
        {
            if (other.GetComponent<IInteractable>() == default && other.GetComponent<ISmashable>() == default) return;
        }
        else
        {
            if (other.GetComponent<ISmashable>() == default) return;
        }
        dot = Mathf.InverseLerp(1,-1,Vector3.Dot((hitPoint - transform.position).normalized, transform.forward));
        
        if(dot > currentState.playerStats.detectionAngle/360f) return;
        interestPointsManager.AddInterestPoint(new InterestPoint(other.gameObject,
            Mathf.InverseLerp(0,
                currentState.playerStats.detectionRange*2,
                Vector3.Distance(new Vector3(transform.position.x,0,transform.position.z), new Vector3(hitPoint.x,0,hitPoint.z))),
            -dot,
            currentState.playerStats.detectionRangeCurve,
            currentState.playerStats.detectionAngleCurve));
        
        //Touch a npc
        if (other.gameObject.CompareTag("Npc") && Mathf.InverseLerp(0,
                currentState.playerStats.detectionRange*2,
                Vector3.Distance(new Vector3(transform.position.x,0,transform.position.z), new Vector3(hitPoint.x,0,hitPoint.z))) < 0.3f && NpcManager.instance.npcScriptDict.ContainsKey(other.gameObject))
        {
            NpcManager.instance.npcScriptDict[other.gameObject].npcScripts.panicData.UpdatePanic(0.005f);
        }
    }
    public enum State
    {
        Bearserker, Stealth
    }
}

