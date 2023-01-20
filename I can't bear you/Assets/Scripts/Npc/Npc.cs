using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;
using Vector3 = UnityEngine.Vector3;

#if UNITY_EDITOR
using UnityEditor;
#endif

[RequireComponent(typeof(NpcScripts), typeof(NavMeshAgent), typeof(Rigidbody))]
public class Npc : Entity, ISmashable
{

    #region Enum
    enum STATS
    {
        RANDOM,
        NOT_RANDOM
    }
    public enum STATE
    {
        THIRST,
        HUNGER,
        BLADDER,
        PARTY,
        ATTRACTED,
        FREEZE,
        MOVEAWAY
    }
    #endregion

    [Header("Initialisation")]
    [SerializeField] STATS initStats = STATS.RANDOM;

    PartyData partyData;
    [SerializeField] private bool test;
    [Header("Data")]
    public Stats stats;
    [SerializeField] public NpcData npcData;

    [Header("State")]
    [ConditionalEnumHide("field", 0)] public STATE state = STATE.PARTY;
    [ConditionalEnumHide("field", 0)] [SerializeField] List<STATE> stateStack = new List<STATE>() { STATE.PARTY };
    public StateLayer[] stateLayers = new StateLayer[4]{new StateLayer(){states =  new [] { Npc.STATE.THIRST , Npc.STATE.HUNGER, Npc.STATE.BLADDER}},
        new StateLayer(){states = new [] { Npc.STATE.MOVEAWAY }},
        new StateLayer(){states = new [] { Npc.STATE.ATTRACTED }},
        new StateLayer(){states = new [] { Npc.STATE.PARTY }}};


    [Header("NavMesh")]
    public NavMeshAgent agent;
    public float minimumDistanceWithDestination = 2;

    [Header("Scripts")]
    public NpcScripts npcScripts;


    [HideInInspector] public Vector3 currentDestination;
    [HideInInspector] public Vector3 attractedPoint;
    [HideInInspector] public Vector3 moveAwayPoint;
    [HideInInspector] public float moveAwayRadius;
    [HideInInspector] public bool isAction;
    [HideInInspector] public Pathfinding pathfinding;
    private Transform player;
    [HideInInspector] public float currentSpeed;
    private float npcSpeed;

    [Header("Ambr :3")]
    [SerializeField] public GameObject deathAnimPrefab;

    //Init Npc
    private void Start()
    {
        //Init Pathfinding
        pathfinding = new Pathfinding();
        agent.speed = npcData.speed;

        //Init Speed
        UpdateSpeed(npcData.speed);

        //Init Stats
        if (initStats == STATS.RANDOM)
        {
            RandomStats();
        }
        else
        {
            SetStats(npcData.maxBladder, npcData.maxHunger, npcData.maxThirst);
        }

        //Init State
        //stateStack.Add(STATE.PARTY);

        //Init Player
        player = LevelManager.instance.GetPlayer();
    }

    private void Update()
    {
        if (!isDie)
        {

            //Player Walk animation when npc is moving
            if (!animator.GetBool("isWalking") && Vector3.Distance(transform.position, agent.destination) > 2f)
            {
                animator.SetBool("isWalking", true);
                animator.SetBool("isDancing", false);
            }

            //Npc calm
            if (npcScripts.panicData.panicState == global::Panic.PanicState.Calm)
            {
                Calm();
            }

            //Npc investigate
            else if (npcScripts.panicData.panicState == global::Panic.PanicState.Tense)
            {
                Investigate();
            }

            //Npc panic
            else
            {
                Panic();
            }

            if (gameObject.activeSelf)
            {
                agent.SetDestination(currentDestination);
            }
        }
    }

    /// <summary>
    /// Load Party Data
    /// </summary>
    /// <param name="partyData"> Current Party Data </param>
    public void InitPartyData(PartyData partyData, int agentPriority)
    {
        this.partyData = partyData;
        agent.avoidancePriority = agentPriority;
    }

    /// <summary>
    /// Play Calm State
    /// </summary>
    void Calm()
    {
        //Update Stats
        stats.currentHunger -= Time.deltaTime;
        stats.currentThirst -= Time.deltaTime;
        stats.currentBladder -= Time.deltaTime;

        UpdateStateStack();

        state = stateStack[0];

        // Set Npc Destination
        if (currentDestination == Vector3.zero)
        {
            switch (state)
            {
                case STATE.HUNGER:
                    SetStateDestination(pathfinding.hungerPoints, npcScripts.npcUI.hungerImage);
                    return;
                case STATE.THIRST:
                    SetStateDestination(pathfinding.thirstPoints, npcScripts.npcUI.thirstImage);
                    return;
                case STATE.BLADDER:
                    SetStateDestination(pathfinding.bladderPoints, npcScripts.npcUI.bladderImage);
                    return;
                case STATE.MOVEAWAY:
                    // disperseCenter = pathfinding.GetDispersePointKey(transform.position);
                    Vector3 dest = moveAwayPoint + (transform.position - new Vector3(moveAwayPoint.x, transform.position.y, moveAwayPoint.z)).normalized * (moveAwayRadius + 3);
                    dest = new Vector3(dest.x, pathfinding.pathHeight, dest.z);
                    if (pathfinding.CalculatePath(agent, transform, dest) != Vector3.zero)
                    {
                        currentDestination = dest;
                    }
                    else
                    {
                        currentDestination = pathfinding.CalculateRandomPosOnCirclePeriphery(agent, transform, moveAwayRadius + 3, new Vector3(moveAwayPoint.x, transform.position.y, moveAwayPoint.z));
                    }
                    return;
                case STATE.ATTRACTED:
                    currentDestination = attractedPoint;
                    npcScripts.npcUI.tvImage.gameObject.SetActive(true);
                    return;
                default:
                    if (partyData.shape == PartyData.Shape.CIRCLE)
                    {
                        currentDestination = pathfinding.CalculateRandomPosInCircle(agent, transform,
                            partyData.radius, partyData.partyPosition.position);
                    }
                    else
                    {
                        currentDestination = pathfinding.CalculateRandomPosInRectangle(agent, transform,
                            partyData.width, partyData.length, partyData.partyPosition);
                    }
                    return;
            }
        }

        //Npc is near to Destination
        if (CheckNpcDistanceToDestination() && !isAction)
        {
            switch (state)
            {
                case STATE.HUNGER:
                    animator.SetBool("isEating", true);
                    npcScripts.npcUI.hungerImage.SetActive(false);
                    return;
                case STATE.THIRST:
                    animator.SetBool("isDrinking", true);
                    npcScripts.npcUI.thirstImage.SetActive(false);
                    return;
                case STATE.BLADDER:
                    animator.SetBool("isBladder", true);
                    npcScripts.npcUI.bladderImage.SetActive(false);
                    return;
                case STATE.ATTRACTED:
                    animator.SetBool("isIdle", true);
                    return;
                default:
                    animator.SetBool("isDancing", true);
                    return;
            }
        }
    }


    /// <summary>
    /// Play Tense State
    /// </summary>
    void Investigate()
    {
        if (currentDestination == Vector3.zero)
        {
            currentDestination = pathfinding.CalculateRandomPosInCircle(agent, transform, npcScripts.panicData.panicData.investigateRadius, player.position);
        }
        else if (CheckNpcDistanceToDestination())
        {
            currentDestination = Vector3.zero;
        }
    }

    /// <summary>
    /// Play Panic State
    /// </summary>
    void Panic()
    {
        // Set Destination

        if (currentDestination == Vector3.zero)
        {
            if (pathfinding.exitPoints.Count > 0)
            {
                currentDestination = pathfinding.ChooseClosestTarget(pathfinding.exitPoints.ToArray(), transform, agent).position;
            }
            else
            {
                currentDestination = pathfinding.noExitPoints[Random.Range(0, pathfinding.noExitPoints.Length)].position;
            }
            return;
        }

        // Check if npc is near destination
        if (CheckNpcDistanceToDestination())
        {
            if (pathfinding.exitPoints.Count > 0)
            {
                NpcManager.instance.UnSpawnNpc(gameObject.name.Replace("(Clone)", String.Empty), gameObject);
            }
            else
            {
                currentDestination = pathfinding.noExitPoints[Random.Range(0, pathfinding.noExitPoints.Length)].position;
            }
        }
    }


    /// <summary>
    /// Set Random Stats To Npc
    /// </summary>
    public void RandomStats()
    {
        stats.currentBladder = Random.Range(0, npcData.maxBladder);
        stats.currentHunger = Random.Range(0, npcData.maxHunger);
        stats.currentThirst = Random.Range(0, npcData.maxThirst);
    }

    /// <summary>
    /// Set Stats To Npc
    /// </summary>
    /// <param name="bladder"> Bladder Stat</param>
    /// <param name="hunger"> Hunger Stat</param>
    /// <param name="thirst"> Thirst Stat</param>
    public void SetStats(float bladder, float hunger, float thirst)
    {
        stats.currentBladder = bladder;
        stats.currentHunger = hunger;
        stats.currentThirst = thirst;
    }

    /// <summary>
    /// Update Npc Speed
    /// </summary>
    /// <param name="newSpeed"></param>
    public void UpdateSpeed(float newSpeed)
    {
        currentSpeed = newSpeed * npcScripts.statusEffects.currentData.currentSpeedRatio * currentSpeedRatio;
        npcSpeed = newSpeed;
    }

    /// <summary>
    /// Slow Npc Speed
    /// </summary>
    public override void Slow()
    {
        base.Slow();
        UpdateSpeed(npcSpeed);
    }

    /// <summary>
    /// Update Npc Speed
    /// </summary>
    public void UpdateWalking()
    {
        agent.speed = Mathf.Lerp(agent.speed, currentSpeed, npcData.acceleration * Time.deltaTime);
        animator.SetFloat("Speed", agent.speed);
    }


    /// <summary>
    /// Stop Npc
    /// </summary>
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
            if (PlayerStateManager.instance.currentState == PlayerStateManager.instance.bearserkerState)
            {
                Collider[] targetsInViewRadius = Physics.OverlapSphere(transform.position, npcData.screamRadius, npcData.screamLayerMask);
                foreach (var target in targetsInViewRadius)
                {
                    if (NpcManager.instance.npcScriptDict.ContainsKey(target.gameObject))
                    {
                        NpcManager.instance.npcScriptDict[target.gameObject].npcScripts.panicData.UpdatePanic(1);
                    }
                }
            }
            animator.SetBool("isSmashing", true);
            Die(false);
        }
    }

    /// <summary>
    /// Call when npc die
    /// </summary>
    /// <param name="unspawn"> True if unspawn, else false</param>
    public override void Die(bool unspawn)
    {
        animator.speed = 1;
        BearserkerGaugeManager.instance.AddBearserker(0.1f,false);
        base.Die(unspawn);
        
        npcScripts.npcUI.canvas.SetActive(false);
    }

    /// <summary>
    /// Attracted Npc
    /// </summary>
    /// <param name="radius"> Radius of the attracted object </param>
    /// <param name="position"> Position of the attracted object </param>
    /// <param name="angle"> Angle of the attracted object </param>
    public void Attracted(float radius, Vector3 position, float angle, Vector2 dirForward, Vector2 dirRight)
    {
        AddStateToStack(STATE.ATTRACTED);
        attractedPoint = pathfinding.CalculateRandomPosInCone(agent, transform,
            radius, angle, position, dirForward, dirRight);
    }

    /// <summary>
    /// Stop Attracted Npc
    /// </summary>
    public void StopAttracted()
    {
        stateStack.Remove(STATE.ATTRACTED);
        npcScripts.npcUI.tvImage.gameObject.SetActive(false);
        currentDestination = Vector3.zero;
    }

    /// <summary>
    /// Freezed Npc
    /// </summary>
    /// <param name="freezeTime"> Freeze Time </param>
    /// <param name="isFreeze"> If is freeze </param>
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
            npcScripts.panicData.UpdatePanic(1);
        }

    }

    /// <summary>
    /// Freeze Cooldown
    /// </summary>
    /// <param name="freezeTime"> Freeze Time </param>
    /// <returns></returns>
    IEnumerator FreezeCD(float freezeTime)
    {
        yield return new WaitForSeconds(freezeTime);
        UpdateSpeed(npcSpeed);
        animator.speed = 1;
        npcScripts.panicData.UpdatePanic(1);
    }

    /// <summary>
    /// Remove Exit Point from the list
    /// </summary>
    /// <param name="exitPoint"> Exit Point to remove </param>
    public void RemoveExitPoint(Transform exitPoint)
    {
        pathfinding.exitPoints.Remove(exitPoint);
        currentDestination = Vector3.zero;
    }

    /// <summary>
    /// Disperse npc if they are in the radius
    /// </summary>
    /// /// <param name="center"> Center of the item </param>
    /// <param name="direction"> Direction normalized between item center and npc </param>
    /// <param name="radius"> Radius of the item </param>
    public void Disperse(Vector3 center, Vector3 direction, float radius)
    {
        AddStateToStack(STATE.MOVEAWAY);
        moveAwayPoint = center;
        moveAwayRadius = radius + 2;
        currentDestination = Vector3.zero;
    }

    /// <summary>
    /// Stop Disperse Npc
    /// </summary>
    public void StopDisperse()
    {
        stateStack.Remove(STATE.MOVEAWAY);
        currentDestination = Vector3.zero;
    }

    /// <summary>
    /// Update State Stack
    /// </summary>
    void UpdateStateStack()
    {
        if (stats.currentHunger <= 0 && !stateStack.Contains(STATE.HUNGER))
        {
            AddStateToStack(STATE.HUNGER);
            return;
        }

        if (stats.currentThirst <= 0 && !stateStack.Contains(STATE.THIRST))
        {
            AddStateToStack(STATE.THIRST);
            return;
        }

        if (stats.currentBladder <= 0 && !stateStack.Contains(STATE.BLADDER))
        {
            AddStateToStack(STATE.BLADDER);
            return;
        }
    }

    /// <summary>
    /// Add State to State Stack
    /// </summary>
    /// <param name="state"> State to add </param>
    void AddStateToStack(STATE state)
    {
        stateStack.Add(state);
        SortStateStack();
    }

    /// <summary>
    /// Sort State Stack with layers states
    /// </summary>
    void SortStateStack()
    {
        List<STATE> sortedList = new List<STATE>();
        for (int i = 0; i < stateLayers.Length; i++)
        {
            foreach (var state in stateLayers[i].states)
            {
                if (stateStack.Contains(state))
                {
                    sortedList.Add(state);
                }
            }
        }

        if (stateStack[0] != sortedList[0])
        {
            currentDestination = Vector3.zero;
            npcScripts.npcUI.bladderImage.SetActive(false);
            npcScripts.npcUI.hungerImage.SetActive(false);
            npcScripts.npcUI.thirstImage.SetActive(false);
            npcScripts.npcUI.tvImage.SetActive(false);
            switch (sortedList[0])
            {
                case STATE.HUNGER:
                    npcScripts.npcUI.hungerImage.SetActive(true);
                    break;
                case STATE.THIRST:
                    npcScripts.npcUI.thirstImage.SetActive(true);
                    break;
                case STATE.BLADDER:
                    npcScripts.npcUI.bladderImage.SetActive(true);
                    break;
                case STATE.ATTRACTED:
                    npcScripts.npcUI.tvImage.SetActive(true);
                    break;
            }

            if (sortedList[0] != STATE.ATTRACTED)
            {
                animator.SetBool("isIdle", false);
            }
        }
        stateStack = sortedList;
    }

    /// <summary>
    /// Check if is near to destination
    /// </summary>
    /// <returns></returns>
    bool CheckNpcDistanceToDestination()
    {
        if (Vector3.Distance(transform.position, agent.destination) < agent.stoppingDistance + minimumDistanceWithDestination)
        {
            return true;
        }

        return false;
    }

    /// <summary>
    /// Call when finished to drink
    /// </summary>
    public void Drink()
    {
        stateStack.Remove(STATE.THIRST);
        stats.currentThirst = npcData.maxThirst;
        isAction = false;
        currentDestination = Vector3.zero;
        Debug.Log("Drinking");
    }

    /// <summary>
    /// Call when finished to pee
    /// </summary>
    public void Pee()
    {
        stateStack.Remove(STATE.BLADDER);
        stats.currentBladder = npcData.maxBladder;
        isAction = false;
        currentDestination = Vector3.zero;
        Debug.Log("Peeing");
    }

    /// <summary>
    /// Call when finished to eat
    /// </summary>
    public void Eat()
    {
        stateStack.Remove(STATE.HUNGER);
        stats.currentHunger = npcData.maxHunger;
        isAction = false;
        currentDestination = Vector3.zero;
        Debug.Log("Eating");
    }

    /// <summary>
    /// Set State Destination
    /// </summary>
    /// <param name="waypoints"> Way Points of the state</param>
    /// <param name="image"> Image of the state </param>
    void SetStateDestination(Transform[] waypoints, GameObject image)
    {
        currentDestination = pathfinding.ChooseClosestTarget(waypoints, transform, agent).position;
        image.SetActive(true);
    }

    public override void Stomp(Vector3 srcPos)
    {
        base.Stomp(srcPos);
        Die(false);
    }

    private void OnDrawGizmos()
    {
#if UNITY_EDITOR
       Handles.Label(transform.position, "CurrentDestination Distance: " + Vector3.Distance(transform.position, currentDestination)); 
       Handles.DrawWireArc(transform.position, Vector3.up, Vector3.forward, 360, npcData.screamRadius);
#endif
    }
}

[Serializable]
public class Stats
{
    [SerializeField] Tools.FIELD field = Tools.FIELD.HIDDEN;

    [ConditionalEnumHide("field", 0)] public float currentHunger = 100;
    [ConditionalEnumHide("field", 0)] public float currentThirst = 100;
    [ConditionalEnumHide("field", 0)] public float currentBladder = 100;
}

[Serializable]
public class StateLayer
{
    public Npc.STATE[] states;
}
