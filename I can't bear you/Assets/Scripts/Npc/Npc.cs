using System;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;
using Vector3 = UnityEngine.Vector3;

public class Npc : MonoBehaviour
{
    public enum STATE {
        THIRST,
        HUNGER,
        BLADDER,
        DANCING
    }
    
    [Header("Data")] 
    public Stats stats;
    [SerializeField] public NpcData npcData;
    public STATE state = STATE.DANCING;

    
    [Header("NavMesh")]
    [SerializeField] NavMeshAgent agent;
    
    [Header("Animator")]
    [SerializeField] Animator animator;

    private Vector3 currentDestination;
    
    [Header("Waypoint Settings")]
    Transform[] runAwayPoints;
    Transform[] hungerPoints;
    Transform[] thirstPoints;
    Transform[] bladderPoints;
    Vector3 investigatePoint;
    


    private Vector3 randomPosParty;

    [HideInInspector] public bool isAction;

    Pathfinding pathfinding;

    [Header("Scripts")] 
    [SerializeField] private Panic panicData;
    [SerializeField] private StatusEffects statusEffects;
    
    private Transform player;

    [HideInInspector] public float currentSpeed;

    private void Start()
    {
        pathfinding = new Pathfinding();
        agent.speed = npcData.speed;
        RandomStats();
        pathfinding.LoadWayPoints(out hungerPoints, out thirstPoints, out bladderPoints, out runAwayPoints);
        player = LevelManager.instance.GetPlayer();
        UpdateSpeed(npcData.speed);
    }

    private void Update()
    {
        if (!animator.GetBool("isWalking") && Vector3.Distance(transform.position, agent.destination) > 2f)
        {
            animator.SetBool("isWalking", true);
            animator.SetBool("isDancing", false);
        }

        if (panicData.panicState == Panic.PanicState.Calm)
        {
            Calm();
        }
        else if(panicData.panicState == Panic.PanicState.Tense)
        {
            Investigate();
        }
        else
        {
            RunAway();
        }
    }

    void Calm()
    {
        stats.currentHunger -= Time.deltaTime;
        stats.currentThirst -= Time.deltaTime;
        stats.currentBladder -= Time.deltaTime;

        if (state == STATE.DANCING)
        {
            if (stats.currentHunger <= 0)
            {
                state = STATE.HUNGER;
                currentDestination = pathfinding.ChooseClosestTarget(hungerPoints, transform, agent);
            }
            else if (stats.currentThirst <= 0)
            {
                state = STATE.THIRST;
                currentDestination = pathfinding.ChooseClosestTarget(thirstPoints, transform, agent);
            }
            else if (stats.currentBladder <= 0)
            {
                state = STATE.BLADDER;
                currentDestination = pathfinding.ChooseClosestTarget(bladderPoints, transform, agent);
            }
            else
            {
                if (!agent.isStopped && randomPosParty == Vector3.zero)
                {
                    randomPosParty = pathfinding.CalculateRandomPosParty(agent,  transform, runAwayPoints[0].position.y,
                        LevelManager.instance.level.partyData.radius, LevelManager.instance.level.partyData.partyPosition.position);
                }
                else if (agent.isStopped && randomPosParty != Vector3.zero)
                {
                    randomPosParty = Vector3.zero;
                }
                agent.SetDestination(randomPosParty);
                if (Vector3.Distance(transform.position, agent.destination) < 2f )
                {
                    animator.SetBool("isDancing", true);
                }
            }
        }
        else if(!isAction)
        {
            switch (state)
            {
                case STATE.HUNGER :
                    agent.SetDestination(currentDestination);
                    if (Mathf.Abs(transform.position.x - agent.destination.x) <= 0.1f &&
                        Mathf.Abs(transform.position.z - agent.destination.z) <= 0.1f)
                    {
                        state = STATE.DANCING;
                        stats.currentHunger = npcData.maxHunger;
                    }
                    break;
                case STATE.THIRST :
                    agent.SetDestination(currentDestination);
                    if(Mathf.Abs(transform.position.x - agent.destination.x) <= 1f && Mathf.Abs(transform.position.z - agent.destination.z) <= 1f)
                    {
                        animator.SetBool("isDrinking", true);
                    }
                    break;
                case STATE.BLADDER :
                    agent.SetDestination(currentDestination);
                    if(Mathf.Abs(transform.position.x - agent.destination.x) <= 0.1f && Mathf.Abs(transform.position.z - agent.destination.z) <= 0.1f)
                    {
                        animator.SetBool("isBladder", true);
                    }
                    break;
            }
        }
    }

    void Investigate()
    {
        if (investigatePoint == Vector3.zero)
        {
            investigatePoint = pathfinding.CalculateRandomPosParty(agent, transform, runAwayPoints[0].position.y, panicData.panicData.investigateRadius, player.position);
        }
        else if (pathfinding.Distance(transform, agent) < 2 && investigatePoint != Vector3.zero)
        {
            investigatePoint = Vector3.zero;
        }
        agent.SetDestination(investigatePoint);
    }

    void RunAway()
    {
        agent.SetDestination(pathfinding.ChooseClosestTarget(runAwayPoints, transform, agent));
        if (Mathf.Abs(transform.position.x - agent.destination.x) <= 0.5f &&
            Mathf.Abs(transform.position.z - agent.destination.z) <= 0.5f)
        {
            NpcManager.instance.UnSpawnNpc(gameObject.name.Replace("(Clone)", String.Empty),gameObject);
        }
    }

    public void RandomStats()
    {
        stats.currentBladder = Random.Range(0, npcData.maxBladder);
        stats.currentHunger = Random.Range(0, npcData.maxHunger);
        stats.currentThirst = Random.Range(0, npcData.maxThirst);
    }

    public void UpdateSpeed(float newSpeed)
    {
        currentSpeed = newSpeed * statusEffects.currentData.currentSpeedRatio;
    }

    public void UpdateWalking()
    {
        agent.speed = Mathf.Lerp(agent.speed, currentSpeed, npcData.acceleration * Time.deltaTime);
        animator.SetFloat("Speed", agent.speed);
    }

    public void StopWalking()
    {
        agent.speed = 0;
        animator.SetBool("isWalking", false);
        animator.SetFloat("Speed", agent.speed);
    }
    
}

[Serializable]
public class Stats
{
    public float currentHunger = 100;
    public float currentThirst = 100;
    public float currentBladder = 100;
}
