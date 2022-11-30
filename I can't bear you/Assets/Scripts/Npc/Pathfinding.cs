using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Pathfinding
{
    public Transform ChooseClosestTarget(Transform[] wayPoints, Transform npcTransform, NavMeshAgent agent)
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

            if (NavMesh.CalculatePath(npcTransform.position, wayPoints[i].position, agent.areaMask, path))
            {
                float distance = Vector3.Distance(npcTransform.position, path.corners[0]);

                for (int j = 1; j < path.corners.Length; j++)
                {
                    distance += Vector3.Distance(path.corners[j - 1], path.corners[j]);
                }

                if (distance < closestTargetDistance)
                {
                    closestTargetDistance = distance;
                    closestTarget = wayPoints[i];
                }
            }
        }
        return closestTarget;
    }

    public void LoadWayPoints(out Transform[] hungerPoints, out Transform[] thirstPoints, out Transform[] bladderPoints, out Transform[] runAwayPoints, out List<Transform> exitPoints)
    {
        hungerPoints = new Transform[LevelManager.instance.level.hungerPoints.Length];
        thirstPoints = new Transform[LevelManager.instance.level.thirstPoints.Length];
        bladderPoints = new Transform[LevelManager.instance.level.bladderPoints.Length];
        runAwayPoints = new Transform[LevelManager.instance.level.notExitPoints.Length];
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
            runAwayPoints[i] = LevelManager.instance.level.notExitPoints[i];
        }

        for (int i = 0; i < LevelManager.instance.level.exitPoints.Length; i++)
        {
            exitPoints.Add(LevelManager.instance.level.exitPoints[i]);
        }
    }

    public Vector3 CalculateRandomPosInCircle(NavMeshAgent agent, Transform npcTransform ,float height, float radius,Vector3 center )
    {
        Vector2 randomPos = CalculateRandomPointInCircle(radius);
        NavMeshPath path = new NavMeshPath();
        NavMesh.CalculatePath(npcTransform.position, new Vector3(center.x + randomPos.x, 
            height, 
            center.z + randomPos.y), agent.areaMask, path);
        if (path.status == NavMeshPathStatus.PathInvalid)
        {
            return CalculateRandomPosInCircle(agent, npcTransform, height, radius, center);
        }
        return  center + new Vector3(randomPos.x, height, randomPos.y);
    }
    
    public Vector3 CalculateRandomPosInCone(NavMeshAgent agent, Transform npcTransform ,float height,float radius, float angle,Vector3 center )
    {
        float y = -Random.Range(0,radius);
        float x = Random.Range(-radius * Mathf.Sin(angle/2 * Mathf.Deg2Rad), radius * Mathf.Sin(angle/2 * Mathf.Deg2Rad));
        Vector2 randomPos = new Vector2(x, y);
        NavMeshPath path = new NavMeshPath();
        NavMesh.CalculatePath(npcTransform.position, new Vector3(center.x + randomPos.x, 
            height, 
            center.z + randomPos.y), agent.areaMask, path);
        if (path.status == NavMeshPathStatus.PathInvalid)
        {
            return CalculateRandomPosInCone(agent, npcTransform, height, radius, angle, center);
        }
        return  center + new Vector3(randomPos.x, height, randomPos.y);
    }
    
    /// <summary>
    /// Calcule a random position on the circle periphery
    /// </summary>
    /// <param name="agent"> NavMeshAgent of the gameobject</param>
    /// <param name="npcTransform"> Transform of the npc</param>
    /// <param name="height"> Height of the destination</param>
    /// <param name="radius"> Radius of the sphere</param>
    /// <param name="center"> Center of the sphere</param>
    /// <returns></returns>
    public Vector3 CalculateRandomPosOnCirclePeriphery(NavMeshAgent agent, Transform npcTransform ,float height, float radius,Vector3 center )
    {
        Vector2 direction = (CalculateRandomPointInCircle(radius) - new Vector2(center.x, center.z)).normalized ;
        Vector2 randomPos = direction * radius;
        NavMeshPath path = new NavMeshPath();
        NavMesh.CalculatePath(npcTransform.position, new Vector3(center.x + randomPos.x, 
            height, 
            center.z + randomPos.y), agent.areaMask, path);
        if (path.status == NavMeshPathStatus.PathInvalid)
        {
            return CalculateRandomPosInCircle(agent, npcTransform, height, radius, center);
        }
        return  center + new Vector3(randomPos.x, height, randomPos.y);
    }

    public Vector2 CalculateRandomPointInCircle(float radius)
    {
        return new Vector2(
            Random.Range(-radius,
                radius),
            Random.Range(-radius,
                radius));
    }
    
    

    public float Distance(Transform npcTransform, NavMeshAgent agent)
    {
        return Vector3.Distance(npcTransform.position, agent.destination);
    }
}
