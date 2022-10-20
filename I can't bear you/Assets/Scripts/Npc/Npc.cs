using System;
using System.Numerics;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class Npc : MonoBehaviour
{
    [Header("Stats")] 
    public Stats stats;
    [SerializeField] NpcData npcData;

    [SerializeField] NavMeshAgent agent;
    
    [Header("Animator")]
    [SerializeField] Animator animator;

    private Vector3 currentDestination;
    
    [Header("Waypoint Settings")]
    Transform[] runAwayPoints;
    Transform[] hungerPoints;
    Transform[] thirstPoints;
    Transform[] bladderPoints;


    private Vector2 randomPosParty;
    

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
        agent.speed = npcData.speed;
        RandomStats();
        LoadWayPoints();
    }

    private void Update()
    {
        stats.currentHunger -= Time.deltaTime;
        stats.currentThirst -= Time.deltaTime;
        stats.currentBladder -= Time.deltaTime;

        if (state == STATE.NOTHING)
        {
            if (stats.currentHunger <= 0)
            {
                state = STATE.HUNGER;
                currentDestination = ChooseClosestTarget(hungerPoints);
                animator.SetBool("isDancing", false);
            }
            else if (stats.currentThirst <= 0)
            {
                state = STATE.THIRST;
                currentDestination = ChooseClosestTarget(thirstPoints);
                animator.SetBool("isDancing", false);
            }
            else if (stats.currentBladder <= 0)
            {
                state = STATE.BLADDER;
                currentDestination = ChooseClosestTarget(bladderPoints);
                animator.SetBool("isDancing", false);
            }
            else
            {
                CalculateRandomPosParty();
                agent.SetDestination(LevelManager.instance.GetCurrentLevel().partyData.partyPosition.position 
                                     + new Vector3(randomPosParty.x, 
                                         runAwayPoints[0].position.y, 
                                         randomPosParty.y));
                if (!agent.isStopped)
                {
                    animator.SetBool("isDancing", true);
                }
            }
        }
        else
        {
            agent.isStopped = false;
            switch (state)
            {
                case STATE.RUNAWAY :
                    agent.SetDestination(currentDestination);
                    if (Mathf.Abs(transform.position.x - agent.destination.x) <= 0.1f &&
                        Mathf.Abs(transform.position.z - agent.destination.z) <= 0.1f)
                    {
                        state = STATE.NOTHING;
                        NpcManager.instance.UnSpawnNpc(gameObject);
                    }
                    break;
                case STATE.HUNGER :
                    agent.SetDestination(currentDestination);
                    if (Mathf.Abs(transform.position.x - agent.destination.x) <= 0.1f &&
                        Mathf.Abs(transform.position.z - agent.destination.z) <= 0.1f)
                    {
                        state = STATE.NOTHING;
                        stats.currentHunger = npcData.maxHunger;
                    }
                    break;
                case STATE.THIRST :
                    agent.SetDestination(currentDestination);
                    if(Mathf.Abs(transform.position.x - agent.destination.x) <= 1f && Mathf.Abs(transform.position.z - agent.destination.z) <= 1f)
                    {
                        state = STATE.NOTHING;
                        stats.currentThirst = npcData.maxThirst;
                    }
                    break;
                case STATE.BLADDER :
                    agent.SetDestination(currentDestination);
                    if(Mathf.Abs(transform.position.x - agent.destination.x) <= 0.1f && Mathf.Abs(transform.position.z - agent.destination.z) <= 0.1f)
                    {
                        state = STATE.NOTHING;
                        stats.currentBladder = npcData.maxBladder;
                    }
                    break;
            }
        }
    }

    public Vector3 ChooseClosestTarget(Transform[] wayPoints)
    {
        Transform closestTarget = null;
        float closestTargetDistance = float.MaxValue;
        NavMeshPath path = new NavMeshPath();

        for (int i = 0; i < wayPoints.Length; i++)
        {
            if (wayPoints[i] == default)
            {
                continue;
            }

            if (NavMesh.CalculatePath(transform.position, wayPoints[i].position, agent.areaMask, path))
            {
                float distance = Vector3.Distance(transform.position, path.corners[0]);

                for (int j = 1; j < path.corners.Length; j++)
                {
                    distance += Vector3.Distance(path.corners[j-1], path.corners[j]);
                }

                if (distance < closestTargetDistance)
                {
                    closestTargetDistance = distance;
                    closestTarget = wayPoints[i];
                }
            }
        }
        

        return closestTarget.position;
    }

    public void RandomStats()
    {
        stats.currentBladder = Random.Range(0, npcData.maxBladder);
        stats.currentHunger = Random.Range(0, npcData.maxHunger);
        stats.currentThirst = Random.Range(0, npcData.maxThirst);
    }

    public void LoadWayPoints()
    {
        hungerPoints = new Transform[LevelManager.instance.GetCurrentLevel().hungerPoints.Length];
        thirstPoints = new Transform[LevelManager.instance.GetCurrentLevel().thirstPoints.Length];
        bladderPoints = new Transform[LevelManager.instance.GetCurrentLevel().bladderPoints.Length];
        runAwayPoints = new Transform[LevelManager.instance.GetCurrentLevel().runAwayPoints.Length];
        for (int i = 0; i < LevelManager.instance.GetCurrentLevel().hungerPoints.Length; i++)
        {
            hungerPoints[i] = LevelManager.instance.GetCurrentLevel().hungerPoints[i];
        }
        for (int i = 0; i < LevelManager.instance.GetCurrentLevel().thirstPoints.Length; i++)
        {
            thirstPoints[i] = LevelManager.instance.GetCurrentLevel().thirstPoints[i];
        }
        for (int i = 0; i < LevelManager.instance.GetCurrentLevel().bladderPoints.Length; i++)
        {
            bladderPoints[i] = LevelManager.instance.GetCurrentLevel().bladderPoints[i];
        }
        for (int i = 0; i < LevelManager.instance.GetCurrentLevel().runAwayPoints.Length; i++)
        {
            runAwayPoints[i] = LevelManager.instance.GetCurrentLevel().runAwayPoints[i];
        }
    }

    void CalculateRandomPosParty()
    {
        if (!agent.isStopped && randomPosParty == Vector2.zero)
        {
            randomPosParty = new Vector2(
                Random.Range(-LevelManager.instance.GetCurrentLevel().partyData.radius,
                    LevelManager.instance.GetCurrentLevel().partyData.radius),
                Random.Range(-LevelManager.instance.GetCurrentLevel().partyData.radius,
                    LevelManager.instance.GetCurrentLevel().partyData.radius));
            NavMeshPath path = new NavMeshPath();
            NavMesh.CalculatePath(transform.position, new Vector3(randomPosParty.x, runAwayPoints[0].position.y, randomPosParty.y), agent.areaMask, path);
            if (path.status == NavMeshPathStatus.PathInvalid)
            {
                CalculateRandomPosParty();
            }
        }
        else if (agent.isStopped && randomPosParty != Vector2.zero)
        {
            randomPosParty = Vector2.zero;
        }
    }
    
}

[Serializable]
public class Stats
{
    public float currentHunger = 100;
    public float currentThirst = 100;
    public float currentBladder = 100;
}
