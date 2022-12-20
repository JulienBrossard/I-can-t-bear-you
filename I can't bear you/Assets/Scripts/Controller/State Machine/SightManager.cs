using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SightManager : MonoBehaviour
{
    [SerializeField] private InterestPointsManager interestPointsManager;
    [SerializeField] private SphereCollider collider;
    [SerializeField] private PlayerStats playerStats;

    public void UpdateStats(PlayerStats newPlayerStats)
    {
        playerStats = newPlayerStats;
        collider.radius = newPlayerStats.detectionRange;
    }

    private float dot;
    private void OnTriggerStay(Collider other)
    {
        if (other.GetComponent<IInteractable>() == default && other.GetComponent<ISmashable>() == default) return;
        dot = Mathf.InverseLerp(1,-1,Vector3.Dot((other.transform.position - transform.position).normalized, transform.forward));
        
        if(dot > playerStats.detectionAngle/360f) return;
        interestPointsManager.AddInterestPoint(new InterestPoint(other.gameObject, Mathf.InverseLerp(0,playerStats.detectionRange,Vector3.Distance(transform.position,other.transform.position)),dot,playerStats.detectionRangeCurve,playerStats.detectionAngleCurve));
        Debug.Log("Found " + other.name);
    }
}
