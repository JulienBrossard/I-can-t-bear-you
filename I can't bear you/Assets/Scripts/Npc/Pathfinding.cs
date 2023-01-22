using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class Pathfinding
{
    [Header("Waypoint Settings")]
    public Transform[] noExitPoints;
    public List<Transform> exitPoints;
    public Transform[] hungerPoints;
    public Transform[] thirstPoints;
    public Transform[] bladderPoints;
    public Dictionary<Vector3, float> dispersePoints = new Dictionary<Vector3, float>();
    public float pathHeight;

    public Pathfinding()
    {
        LoadWayPoints();
        pathHeight = noExitPoints[0].position.y;
    }
    
    /// <summary>
    /// Choose Closest Target
    /// </summary>
    /// <param name="wayPoints"> WayPoints of target </param>
    /// <param name="npcTransform"> Transform of the npc </param>
    /// <param name="agent"> Agent of the npc </param>
    /// <returns></returns>
    public Transform ChooseClosestTarget(Transform[] wayPoints, Transform npcTransform, NavMeshAgent agent)
    {
        if (wayPoints.Length == 0)
        {
            Debug.LogWarning("No waypoints found or no valid path found");
            return null;
        }
        Transform closestTarget = null;
        float closestTargetDistance = float.MaxValue;
        NavMeshPath path = new NavMeshPath();

        for (int i = 0; i < wayPoints.Length; i++)
        {
            if (wayPoints[i] == default)
            {
                continue;
            }

            if (NavMesh.CalculatePath(npcTransform.position, wayPoints[i].position, agent.areaMask, path))
            {
                if (path.corners.Length == 0)
                {
                    Debug.Log(wayPoints[0].name + " " + path.corners.Length);
                    continue;
                }
                float distance = Vector3.Distance(npcTransform.position, path.corners[0]);

                if (path.corners.Length == 1)
                {
                    distance += Vector3.Distance(npcTransform.position, path.corners[0]);
                }

                else
                {
                    for (int j = 1; j < path.corners.Length; j++)
                    {
                        distance += Vector3.Distance(path.corners[j - 1], path.corners[j]);
                    }
                }

                if (distance < closestTargetDistance)
                {
                    closestTargetDistance = distance;
                    closestTarget = wayPoints[i];
                }
            }
        }

        NavMesh.CalculatePath(npcTransform.position, closestTarget.position, agent.areaMask, path);
        if (!CheckPathStatus(path))
        {
            List<Transform> newWayPoints = new List<Transform>();
            for (int j = 0; j < wayPoints.Length; j++)
            {
                newWayPoints.Add(wayPoints[j]);
            }
            newWayPoints.Remove(closestTarget);
            return ChooseClosestTarget(newWayPoints.ToArray(), npcTransform, agent);
        }

        return closestTarget;
    }

    
    /// <summary>
    /// Load Way Points from the level
    /// </summary>
    public void LoadWayPoints()
    {
        hungerPoints = new Transform[LevelManager.instance.level.hungerPoints.Length];
        thirstPoints = new Transform[LevelManager.instance.level.thirstPoints.Length];
        bladderPoints = new Transform[LevelManager.instance.level.bladderPoints.Length];
        noExitPoints = new Transform[LevelManager.instance.level.notExitPoints.Length];
        exitPoints = new List<Transform>();
        for (int i = 0; i < LevelManager.instance.level.hungerPoints.Length; i++)
        {
            hungerPoints[i] = LevelManager.instance.level.hungerPoints[i];
        }
        for (int i = 0; i < LevelManager.instance.level.thirstPoints.Length; i++)
        {
            thirstPoints[i] = LevelManager.instance.level.thirstPoints[i];
        }
        for (int i = 0; i < LevelManager.instance.level.bladderPoints.Length; i++)
        {
            bladderPoints[i] = LevelManager.instance.level.bladderPoints[i];
        }
        for (int i = 0; i < LevelManager.instance.level.notExitPoints.Length; i++)
        {
            noExitPoints[i] = LevelManager.instance.level.notExitPoints[i];
        }

        for (int i = 0; i < LevelManager.instance.level.exitPoints.Length; i++)
        {
            exitPoints.Add(LevelManager.instance.level.exitPoints[i]);
        }
    }

    /// <summary>
    /// Calculate Random Pos In a circle
    /// </summary>
    /// <param name="agent"> Agent of the npc </param>
    /// <param name="npcTransform"> Transform of the npc </param>
    /// <param name="radius"> Radius of the circle </param>
    /// <param name="center"> Center of the circle </param>
    /// <returns></returns>
    public Vector3 CalculateRandomPosInCircle(NavMeshAgent agent, Transform npcTransform , float radius,Vector3 center )
    {
        Vector2 randomPos = Tools.instance.CalculateRandomPointInCircle(radius);
        NavMeshPath path = new NavMeshPath();
        NavMesh.CalculatePath(npcTransform.position, new Vector3(center.x + randomPos.x, 
            noExitPoints[0].position.y, 
            center.z + randomPos.y), agent.areaMask, path);
        if (!CheckPathStatus(path))
        {
            return CalculateRandomPosInCircle(agent, npcTransform, radius, center);
        }
        return  center + new Vector3(randomPos.x,  noExitPoints[0].position.y, randomPos.y);
    }
    
    /// <summary>
    /// Calculate Random Pos In a cone
    /// </summary>
    /// <param name="agent"> Agent of the npc </param>
    /// <param name="npcTransform"> Transform of the npc </param>
    /// <param name="radius"> Radius of the cone </param>
    /// <param name="angle"> Angle of the cone </param>
    /// <param name="center"> Center of the cone </param>
    /// <returns></returns>
    public Vector3 CalculateRandomPosInCone(NavMeshAgent agent, Transform npcTransform ,float radius, float angle,Vector3 center, Vector2 dirForward, Vector2 dirRight )
    {
        Vector2 randomPos = dirForward * -Random.Range(0, radius) + dirRight *
            Random.Range(-radius * Mathf.Sin(angle / 2 * Mathf.Deg2Rad), radius * Mathf.Sin(angle / 2 * Mathf.Deg2Rad));
        NavMeshPath path = new NavMeshPath();
        NavMesh.CalculatePath(npcTransform.position, new Vector3(center.x + randomPos.x, 
            noExitPoints[0].position.y, 
            center.z + randomPos.y), agent.areaMask, path);
        if (!CheckPathStatus(path))
        {
            return CalculateRandomPosInCone(agent, npcTransform, radius, angle, center, dirForward, dirRight);
        }
        return  center + new Vector3(randomPos.x, noExitPoints[0].position.y, randomPos.y);
    }
    
    /// <summary>
    /// Calculate Random Pos In a Rectangle
    /// </summary>
    /// <param name="agent"> Agent of the npc </param>
    /// <param name="npcTransform"> Transform of the npc </param>
    /// <param name="width"> Width of the rectangle </param>
    /// <param name="length"> Length of the rectangle </param>
    /// <param name="center"> Center of the rectangle </param>
    /// <returns></returns>
    public Vector3 CalculateRandomPosInRectangle(NavMeshAgent agent, Transform npcTransform , float width, float length,Transform center )
    {
        Vector2 randomPos = Tools.instance.CalculateRandomPointInRectangle(length, width);
        Vector3 result = new Vector3(randomPos.x, 0, randomPos.y);
        result = result.x * center.forward + result.z * center.right;
        randomPos = new Vector2(result.x , result.z);
        NavMeshPath path = new NavMeshPath();
        NavMesh.CalculatePath(npcTransform.position, new Vector3(center.position.x + randomPos.x, 
            noExitPoints[0].position.y, 
            center.position.z + randomPos.y), agent.areaMask, path);
        if (!CheckPathStatus(path))
        {
            return CalculateRandomPosInRectangle(agent, npcTransform, width, length, center);
        }
        return  new Vector3(center.position.x + randomPos.x, noExitPoints[0].position.y, center.position.z + randomPos.y);
    }
    
    /// <summary>
    /// Calcule a random position on the circle periphery
    /// </summary>
    /// <param name="agent"> NavMeshAgent of the gameobject</param>
    /// <param name="npcTransform"> Transform of the npc</param>
    /// <param name="radius"> Radius of the sphere</param>
    /// <param name="center"> Center of the sphere</param>
    /// <returns></returns>
    public Vector3 CalculateRandomPosOnCirclePeriphery(NavMeshAgent agent, Transform npcTransform , float radius,Vector3 center )
    {
        Vector2 direction = Tools.instance.CalculateRandomPointInCircle(radius).normalized ;
        Vector2 randomPos = direction * radius;
        NavMeshPath path = new NavMeshPath();
        NavMesh.CalculatePath(npcTransform.position, new Vector3(center.x + randomPos.x, 
            noExitPoints[0].position.y, 
            center.z + randomPos.y), agent.areaMask, path);
        if (!CheckPathStatus(path))
        {
            return CalculateRandomPosOnCirclePeriphery(agent, npcTransform, radius, center);
        }
        return  center + new Vector3(randomPos.x, noExitPoints[0].position.y, randomPos.y);
    }
    
    /// <summary>
    /// Calculate path
    /// </summary>
    /// <param name="agent"> NavMeshAgent of the gameobject </param>
    /// <param name="npcTransform"> Transform of the npc </param>
    /// <param name="target"> Destination </param>
    /// <returns></returns>
    public Vector3 CalculatePath(NavMeshAgent agent, Transform npcTransform , Vector3 target)
    {
        NavMeshPath path = new NavMeshPath();
        NavMesh.CalculatePath(npcTransform.position, target, agent.areaMask, path);
        if (!CheckPathStatus(path))
        {
            return Vector3.zero;
        }
        return  target;
    }
    
    /// <summary>
    /// Check if the path is valid
    /// </summary>
    /// <param name="path"> Path to check </param>
    /// <returns></returns>
    public bool CheckPathStatus(NavMeshPath path)
    {
        if (path.status == NavMeshPathStatus.PathInvalid)
        {
            return false;
        }

        if (path.corners.Length == 0)
        {
            return false;
        }

        if (CheckPointInDispersePoints(path.corners[^1]))
        {
            return false;
        }
        return true;
    }
    
    /// <summary>
    /// Check if point is in Disperse Points
    /// </summary>
    /// <param name="point"> Point to check </param>
    /// <returns></returns>
    public bool CheckPointInDispersePoints(Vector3 point)
    {
        foreach (var key in dispersePoints.Keys)
        {
            if (Tools.instance.CheckPointInCircle(point, dispersePoints[key], key))
            {
                return true;
            }
        }
        return false;
    }
    
    /// <summary>
    /// Get key of the point in disperse points
    /// </summary>
    /// <param name="point"> Point to check </param>
    /// <returns></returns>
    public Vector3 GetDispersePointKey(Vector3 point)
    {
        foreach (var key in dispersePoints.Keys)
        {
            if (Tools.instance.CheckPointInCircle(point, dispersePoints[key], key))
            {
                return key;
            }
        }
        Debug.LogWarning("Npc is not in a disperse point");
        return Vector3.zero;
    }
    
    /// <summary>
    /// Distance of path
    /// </summary>
    /// <param name="npcTransform"> Transform of the npc </param>
    /// <param name="agent"> Agent of the npc </param>
    /// <returns></returns>
    public float Distance(Transform npcTransform, NavMeshAgent agent)
    {
        return Vector3.Distance(npcTransform.position, agent.destination);
    }
}
