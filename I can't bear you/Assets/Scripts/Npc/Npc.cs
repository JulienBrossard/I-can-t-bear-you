using System;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

public class Npc : MonoBehaviour
{
    [Header("Stats")] 
    public Stats stats;
    
    [SerializeField] NavMeshAgent agent;

    private Vector3 currentDestination;
    
    [Header("Waypoint Settings")]
    [SerializeField] Transform[] runAwayPoints;
    [SerializeField] Transform[] hungerPoints;
    [SerializeField] Transform[] thirstPoints;
    [SerializeField] Transform[] bladderPoints;

    enum STATE {
        RUNAWAY,
        THIRST,
        HUNGER,
        BLADDER,
        NOTHING
    }

    private STATE state = STATE.NOTHING;
    

    private void Start()
    {
        agent.speed = stats.speed;
    }

    private void Update()
    {
        switch (state)
        {
            case STATE.RUNAWAY :
                agent.SetDestination(currentDestination);
                if (Mathf.Abs(transform.position.x - agent.destination.x) <= 0.1f &&
                    Mathf.Abs(transform.position.z - agent.destination.z) <= 0.1f)
                {
                    state = STATE.NOTHING;
                }
                break;
            case STATE.HUNGER :
                agent.SetDestination(currentDestination);
                if (Mathf.Abs(transform.position.x - agent.destination.x) <= 0.1f &&
                    Mathf.Abs(transform.position.z - agent.destination.z) <= 0.1f)
                {
                    state = STATE.NOTHING;
                    stats.hunger = 10;
                }
                break;
            case STATE.THIRST :
                agent.SetDestination(currentDestination);
                if(Mathf.Abs(transform.position.x - agent.destination.x) <= 1f && Mathf.Abs(transform.position.z - agent.destination.z) <= 1f)
                {
                    state = STATE.NOTHING;
                    stats.thirst = 10;
                }
                break;
            case STATE.BLADDER :
                agent.SetDestination(currentDestination);
                if(Mathf.Abs(transform.position.x - agent.destination.x) <= 0.1f && Mathf.Abs(transform.position.z - agent.destination.z) <= 0.1f)
                {
                    state = STATE.NOTHING;
                    stats.bladder = 10;
                }
                break;
        }
        
        stats.hunger -= Time.deltaTime;
        stats.thirst -= Time.deltaTime;
        stats.bladder -= Time.deltaTime;

        if (state == STATE.NOTHING)
        {
            if (stats.hunger <= 0)
            {
                state = STATE.HUNGER;
                currentDestination = FindCloseDestination(hungerPoints);
            }
            else if (stats.thirst <= 0)
            {
                state = STATE.THIRST;
                currentDestination = FindCloseDestination(thirstPoints);
            }
            else if (stats.bladder <= 0)
            {
                state = STATE.BLADDER;
                currentDestination = FindCloseDestination(bladderPoints);
            }
        }
    }
    
    public Vector3 FindCloseDestination(Transform[] wayPoints)
    {
        agent.SetDestination(wayPoints[0].position);
        float minDistance = agent.remainingDistance;
        Vector3 closestPoint = wayPoints[0].position;
        float pathLenght;

        for (int i = 1; i < wayPoints.Length; i++)
        {
            agent.CalculatePath(wayPoints[i].position, agent.path);
            pathLenght = GetPathLength();
            if (pathLenght < minDistance)
            {
                minDistance = pathLenght;
            }
        }

        return closestPoint;
    }
    
    float GetPathLength()
    {
        float lng = 0.0f;
       
        if (( agent.pathStatus != NavMeshPathStatus.PathInvalid ))
        {
            for ( int i = 1; i < agent.path.corners.Length; ++i )
            {
                lng += Vector3.Distance( agent.path.corners[i-1], agent.path.corners[i] );
            }
        }
       
        return lng;
    }
    
}

[Serializable]
public class Stats
{
    public float hunger = 100;
    public float thirst = 100;
    public float bladder = 100;
    public float speed = 10;
}
