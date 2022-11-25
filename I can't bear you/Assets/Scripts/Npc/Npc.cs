using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;
using Vector3 = UnityEngine.Vector3;

public class Npc : Entity,ISmashable
{
    public enum STATE {
        THIRST,
        HUNGER,
        BLADDER,
        DANCING,
        ATTRACTED,
        FREEZE
    }
    
    [Header("Data")] 
    public Stats stats;
    [SerializeField] public NpcData npcData;
    public STATE state = STATE.DANCING;

    
    [Header("NavMesh")]
    public NavMeshAgent agent;
    public float minimumDistanceWithDestination = 0.5f;

    [HideInInspector] public Transform currentDestination;
    Transform runAwayDestination;
    
    [Header("Waypoint Settings")]
    Transform[] noExitPoints;
    public List<Transform> exitPoints;
    Transform[] hungerPoints;
    Transform[] thirstPoints;
    Transform[] bladderPoints;
    Vector3 investigatePoint;
    


    private Vector3 randomPosParty;
    [HideInInspector] public Vector3 attractedPoint;

    [HideInInspector] public bool isAction;

    Pathfinding pathfinding;

    [Header("Scripts")] 
    [SerializeField] private Panic panicData;
    [SerializeField] private StatusEffects statusEffects;
    
    private Transform player;

    [HideInInspector] public float currentSpeed;

    private float npcSpeed;
    
    [Header("State Feedback")]
    [SerializeField] private GameObject thirstImage;
    [SerializeField] private GameObject hungerImage;
    [SerializeField] private GameObject bladderImage;

    private void Start()
    {
        pathfinding = new Pathfinding();
        agent.speed = npcData.speed;
        RandomStats();
        pathfinding.LoadWayPoints(out hungerPoints, out thirstPoints, out bladderPoints, out noExitPoints, out exitPoints);
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

        if (panicData.panicState == global::Panic.PanicState.Calm)
        {
            Calm();
        }
        else if(panicData.panicState == global::Panic.PanicState.Tense)
        {
            Investigate();
        }
        else
        {
            Panic();
        }
    }

    void Calm()
    {
        stats.currentHunger -= Time.deltaTime;
        stats.currentThirst -= Time.deltaTime;
        stats.currentBladder -= Time.deltaTime;

        if (state == STATE.DANCING || state == STATE.ATTRACTED)
        {
            if (stats.currentHunger <= 0)
            {
                state = STATE.HUNGER;
                currentDestination = pathfinding.ChooseClosestTarget(hungerPoints, transform, agent);
                hungerImage.SetActive(true);
            }
            else if (stats.currentThirst <= 0)
            {
                state = STATE.THIRST;
                currentDestination = pathfinding.ChooseClosestTarget(thirstPoints, transform, agent);
                thirstImage.SetActive(true);
            }
            else if (stats.currentBladder <= 0)
            {
                state = STATE.BLADDER;
                currentDestination = pathfinding.ChooseClosestTarget(bladderPoints, transform, agent);
                bladderImage.SetActive(true);
            }
            else
            {
                if (!agent.isStopped && randomPosParty == Vector3.zero)
                {
                    if (state == STATE.ATTRACTED)
                    {
                        randomPosParty = attractedPoint;
                    }
                    else
                    {
                        randomPosParty = pathfinding.CalculateRandomPosInSphere(agent,  transform, noExitPoints[0].position.y,
                            LevelManager.instance.level.partyData.radius, LevelManager.instance.level.partyData.partyPosition.position);
                    }
                }
                else if (agent.isStopped && randomPosParty != Vector3.zero)
                {
                    randomPosParty = Vector3.zero;
                }
                agent.SetDestination(randomPosParty);
                if (Vector3.Distance(transform.position, agent.destination) < agent.stoppingDistance + minimumDistanceWithDestination )
                {
                    if (state == STATE.DANCING)
                    {
                        animator.SetBool("isDancing", true);
                    }
                    else
                    {
                        animator.SetBool("isIdle", true);
                    }
                }
            }
        }
        else if(!isAction)
        {
            switch (state)
            {
                case STATE.HUNGER :
                    agent.SetDestination(currentDestination.position);
                    if (Mathf.Abs(transform.position.x - agent.destination.x) <= agent.stoppingDistance + minimumDistanceWithDestination &&
                        Mathf.Abs(transform.position.z - agent.destination.z) <= agent.stoppingDistance + minimumDistanceWithDestination)
                    {
                        state = STATE.DANCING;
                        stats.currentHunger = npcData.maxHunger;
                        hungerImage.SetActive(false);
                    }
                    break;
                case STATE.THIRST :
                    agent.SetDestination(currentDestination.position);
                    if(Mathf.Abs(transform.position.x - agent.destination.x) <= agent.stoppingDistance + minimumDistanceWithDestination && Mathf.Abs(transform.position.z - agent.destination.z) <= agent.stoppingDistance + minimumDistanceWithDestination)
                    {
                        animator.SetBool("isDrinking", true);
                        thirstImage.SetActive(false);
                    }
                    break;
                case STATE.BLADDER :
                    agent.SetDestination(currentDestination.position);
                    if(Mathf.Abs(transform.position.x - agent.destination.x) <= agent.stoppingDistance + minimumDistanceWithDestination && Mathf.Abs(transform.position.z - agent.destination.z) <= agent.stoppingDistance + minimumDistanceWithDestination)
                    {
                        animator.SetBool("isBladder", true);
                        bladderImage.SetActive(false);
                    }
                    break;
            }
        }
    }

    void Investigate()
    {
        if (investigatePoint == Vector3.zero)
        {
            investigatePoint = pathfinding.CalculateRandomPosInSphere(agent, transform, noExitPoints[0].position.y, panicData.panicData.investigateRadius, player.position);
        }
        else if (pathfinding.Distance(transform, agent) < 2 && investigatePoint != Vector3.zero)
        {
            investigatePoint = Vector3.zero;
        }
        agent.SetDestination(investigatePoint);
    }

    void Panic()
    {
 
        
        if ((Mathf.Abs(transform.position.x - agent.destination.x) <= agent.stoppingDistance + minimumDistanceWithDestination &&
             Mathf.Abs(transform.position.z - agent.destination.z) <= agent.stoppingDistance + minimumDistanceWithDestination) || runAwayDestination == null)
        {
            if (exitPoints.Count > 0)
            {
                if (runAwayDestination != null && Mathf.Abs(transform.position.x - runAwayDestination.position.x) <= agent.stoppingDistance + minimumDistanceWithDestination &&
                    Mathf.Abs(transform.position.z - runAwayDestination.position.z) <= agent.stoppingDistance + minimumDistanceWithDestination)
                {
                    NpcManager.instance.UnSpawnNpc(gameObject.name.Replace("(Clone)", String.Empty), gameObject);
                    return;
                }
                runAwayDestination = pathfinding.ChooseClosestTarget(exitPoints.ToArray(), transform, agent);
            }
            else
            {
                runAwayDestination = noExitPoints[Random.Range(0, noExitPoints.Length)];
            }
            agent.SetDestination(runAwayDestination.position);
        }
        agent.SetDestination(runAwayDestination.position);
    }

    public void RandomStats()
    {
        stats.currentBladder = Random.Range(0, npcData.maxBladder);
        stats.currentHunger = Random.Range(0, npcData.maxHunger);
        stats.currentThirst = Random.Range(0, npcData.maxThirst);
    }

    public void UpdateSpeed(float newSpeed)
    {
        currentSpeed = newSpeed * statusEffects.currentData.currentSpeedRatio * currentSpeedRatio;
        npcSpeed = newSpeed;
    }

    public override void Slow()
    {
        base.Slow();
        UpdateSpeed(npcSpeed);
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

    public void Smash() //Fonction appelée quand le joueur tape sur le NPC
    {
        if (!isDie)
        { 
            animator.SetBool("isSmashing",true);
            Die();
        }
    }

    public override void Die()
    {
        animator.speed = 1;
        base.Die();
    }
    
    
    public void Attracted(float radius, Vector3 position, float angle)
    {
        state = STATE.ATTRACTED;
        randomPosParty = Vector3.zero;
        attractedPoint = pathfinding.CalculateRandomPosInCone(agent,  transform, noExitPoints[0].position.y,
            radius, angle, position);
    }

    public void StopAttracted()
    {
        state = STATE.DANCING;
        randomPosParty = Vector3.zero;
    }

    public void GetFreezed(float freezeTime, bool isFreeze)
    {
        if (isFreeze)
        {
            state = STATE.FREEZE;
            StopWalking();
            animator.speed = 0;
            UpdateSpeed(0);
            StartCoroutine(FreezeCD(freezeTime));
        }
        else
        {
            panicData.UpdatePanic(1);
        }
 
    }
    IEnumerator FreezeCD(float freezeTime)
    {
        yield return new WaitForSeconds(freezeTime);
        UpdateSpeed(npcSpeed);
        animator.speed = 1;
        panicData.UpdatePanic(1);
    }

    public void RemoveExitPoint(Transform exitPoint)
    {
        exitPoints.Remove(exitPoint);
        runAwayDestination = null;
    }

}

[Serializable]
public class Stats
{
    public float currentHunger = 100;
    public float currentThirst = 100;
    public float currentBladder = 100;
}
