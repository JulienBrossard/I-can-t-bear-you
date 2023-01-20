using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WinNpc : MonoBehaviour
{
    public Transform[] waypoints;
    private int currentWaypoint;
    [SerializeField] private NavMeshAgent agent;
    
    public void LoadWaypoints(Transform[] waypoints)
    {
        this.waypoints = waypoints;
        agent.SetDestination(waypoints[currentWaypoint].position);   
    }
    
    private void Update()
    {
        if (waypoints != null)
        if (Vector3.Distance(transform.position, waypoints[currentWaypoint].position) < 0.5f)
        {
            currentWaypoint++;
            if (currentWaypoint >= waypoints.Length)
            {
                DestroyImmediate(gameObject);
                return;
            }
            agent.SetDestination(waypoints[currentWaypoint].position);
        }
    }
}
