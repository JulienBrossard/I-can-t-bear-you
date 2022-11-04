using UnityEngine;
using UnityEngine.AI;

public class Pathfinding
{
    public Vector3 ChooseClosestTarget(Transform[] wayPoints, Transform npcTransform, NavMeshAgent agent)
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
        return closestTarget.position;
    }

    public void LoadWayPoints(out Transform[] hungerPoints, out Transform[] thirstPoints, out Transform[] bladderPoints, out Transform[] runAwayPoints)
    {
        hungerPoints = new Transform[LevelManager.instance.level.hungerPoints.Length];
        thirstPoints = new Transform[LevelManager.instance.level.thirstPoints.Length];
        bladderPoints = new Transform[LevelManager.instance.level.bladderPoints.Length];
        runAwayPoints = new Transform[LevelManager.instance.level.runAwayPoints.Length];
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
        for (int i = 0; i < LevelManager.instance.level.runAwayPoints.Length; i++)
        {
            runAwayPoints[i] = LevelManager.instance.level.runAwayPoints[i];
        }
    }

    public Vector3 CalculateRandomPosParty(NavMeshAgent agent, Transform npcTransform ,float height, float radius,Vector3 center )
    {
        Vector2 randomPos = new Vector2(
            Random.Range(-radius,
                radius),
            Random.Range(-radius,
                radius));
        NavMeshPath path = new NavMeshPath();
        NavMesh.CalculatePath(npcTransform.position, center 
                                                     + new Vector3(randomPos.x, 
                                                         height, 
                                                         randomPos.y), agent.areaMask, path);
        if (path.status == NavMeshPathStatus.PathInvalid)
        {
            return CalculateRandomPosParty(agent, npcTransform, height, radius, center);
        }
        return  center + new Vector3(randomPos.x, height, randomPos.y);
    }

    public float Distance(Transform npcTransform, NavMeshAgent agent)
    {
        return Vector3.Distance(npcTransform.position, agent.destination);
    }
}
