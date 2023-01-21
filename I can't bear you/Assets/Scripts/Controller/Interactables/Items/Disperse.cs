using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshObstacle), typeof(Awareness))]
public class Disperse : Item, IInteractable
{
    public enum DisperseType
    {
        RANDOM
    }
    
    [Header("Disperse Data")]
    [SerializeField] private DisperseType disperseType;
    [SerializeField] public NavMeshObstacle obstacle;
    [SerializeField] public Awareness awareness;
    [SerializeField] private AudioSource audioSource;
    List<GameObject> currentTargets = new List<GameObject>();

    private bool setDispersePoint;
    private bool isDispersing;

    private void Update()
    {
        if (isDispersing)
        {
            DisperseNpc();
        }
    }

    private void Start()
    {
        if (!setDispersePoint)
        {
            NpcManager.instance.SetDispersePoint(transform.position, awareness.viewRadius, disperseType);
            awareness.Init();
        }
    }

    /// <summary>
    /// Disperse All Npc in view
    /// </summary>
    void DisperseNpc()
    {
        if (awareness.visibleTargets.Count > 0)
        {
            List<Transform> targets = new List<Transform>();
            foreach (var target in awareness.visibleTargets)
            {
                targets.Add(target);
            }
            targets = CheckNpcStillInView(targets);
            DisperseRemainingNpcInView(targets);
        }
        else if (!obstacle.enabled)
        {
            obstacle.enabled = true;
        }
    }

    /// <summary>
    /// Stop dispersing the NPC
    /// </summary>
    /// <param name="target"> Target to stop dispersing </param>
    void StopDisperseNpc(GameObject target)
    {
        if (NpcManager.instance.npcScriptDict.ContainsKey(target))
        {
            NpcManager.instance.npcScriptDict[target].StopDisperse();
        }
        else
        {
            Debug.Log(target.name+ " is not in dictionary when u tried to stop dispersing");
        }
    }

    /// <summary>
    /// Active or disable item
    /// </summary>
    [ContextMenu("Switch")]
    void Switch()
    {
        if (isDispersing)
        {
            isDispersing = false;
            NpcManager.instance.RemoveDispersePoint(transform.position, disperseType);
            foreach (var target in currentTargets)
            {
                NpcManager.instance.npcScriptDict[target].StopDisperse();
            }
            obstacle.enabled = false;
        }
        else
        {
            isDispersing = true;
        }
    }
    
    /// <summary>
    /// Check if Npc are still in view, if yes we do nothing, if no we stop dispersing them and return the list of new targets in view
    /// </summary>
    /// <param name="targets"> Copy of visible targets in view </param>
    List<Transform> CheckNpcStillInView(List<Transform> targets)
    {
        List<GameObject> currentTargetsToRemove = new List<GameObject>();
        foreach (GameObject target in currentTargets)
        {
            if (!targets.Contains(target.transform))
            {
                StopDisperseNpc(target);
                currentTargetsToRemove.Add(target);
            }
            else
            {
                targets.Remove(target.transform);
            }
        }
        currentTargets = currentTargets.Except(currentTargetsToRemove).ToList();
        return targets;
    }

    /// <summary>
    /// If there is still npc in view, we disperse them
    /// </summary>
    /// <param name="targets"> Copy of visible targets in view </param>
    void DisperseRemainingNpcInView(List<Transform> targets)
    {
        foreach (var target in targets)
        {
            Vector3 direction = transform.position - target.position;
            direction.Normalize();
            NpcManager.instance.npcScriptDict[target.gameObject].Disperse(transform.position, direction, awareness.viewRadius);
            currentTargets.Add(target.gameObject);
        }
    }

    private void OnEnable()
    {
        if (NpcManager.instance != default)
        {
            NpcManager.instance.SetDispersePoint(transform.position, awareness.viewRadius, disperseType);
            awareness.Init();
            setDispersePoint = true;
        }
    }

    private void OnDisable()
    {
        NpcManager.instance.RemoveDispersePoint(transform.position, disperseType);
        foreach (var target in currentTargets)
        {
            NpcManager.instance.npcScriptDict[target].StopDisperse();
        }
        obstacle.enabled = false;
    }

    public void Interact(Vector3 sourcePos)
    {
        Switch();
        audioSource.enabled = !audioSource.enabled;
    }
}
