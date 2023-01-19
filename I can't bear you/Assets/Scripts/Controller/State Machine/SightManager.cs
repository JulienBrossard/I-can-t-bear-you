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
        Debug.Log(Vector3.Distance(new Vector3(transform.position.x,0,transform.position.z),new Vector3(other.transform.position.x,0,other.transform.position.z)).ToString());
        interestPointsManager.AddInterestPoint(new InterestPoint(other.gameObject,
            Mathf.InverseLerp(0,currentState.playerStats.detectionRange*2,Vector3.Distance(new Vector3(transform.position.x,0,transform.position.z), new Vector3(other.transform.position.x,0,other.transform.position.z))),
            -dot,
            currentState.playerStats.detectionRangeCurve,
            currentState.playerStats.detectionAngleCurve));
    }
    /*#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        if(currentState == default) return;
        Handles.color = new Color(0,1,0,0.25f);
        //Draw solid arc for detection range with transform.forward as the center of the solid arc, not the beginning
        
        
        //Handles.DrawSolidArc(transform.position, Vector3.up, transform.forward + new Vector3((currentState.playerStats.detectionAngle*0.5f)/Mathf.PI,0,0), currentState.playerStats.detectionAngle, currentState.playerStats.detectionRange*2f);
    }
    #endif*/
}
