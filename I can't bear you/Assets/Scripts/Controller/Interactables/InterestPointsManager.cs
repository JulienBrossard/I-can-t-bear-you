using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class InterestPointsManager : MonoBehaviour
{
    public List<InterestPoint> interestPoints;
    [SerializeField] private LayerMask obstacleLayerMask;

    #if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        foreach (InterestPoint interestPoint in interestPoints)
        {
            if(interestPoint.go == default) continue;
            Handles.DrawLine(transform.position,interestPoint.go.transform.position);
            Handles.Label(interestPoint.go.transform.position + Vector3.up,"Score : " + interestPoint.score + "\nCenter Dist : " + interestPoint.centerDistance + "\nDist : " + interestPoint.distance);
        }
    }
    #endif

    private void FixedUpdate()
    {
        SortInterestPoints();
        foreach (InterestPoint interestPoint in interestPoints)
        {
            if (!interestPoint.validity)
            {
                RemoveInterestPoint(interestPoint);
                return;
            }
            interestPoint.validity = false;
        }
    }

    private GameObject outlineGoBuffer;
    void SortInterestPoints()
    {
        interestPoints = interestPoints.OrderByDescending(o=>o.score).ToList();
        if (!outlineGoBuffer.IsDestroyed())
        {
            outlineGoBuffer?.GetComponent<Outline>()?.DisableOutline();
        }
        if (interestPoints.Count == 0)
        {
            outlineGoBuffer = default;
            return;
        }

        if (interestPoints[0]?.go == default) return;
        
        outlineGoBuffer = GetHighestPriorityItem().go;
        outlineGoBuffer.GetComponent<Outline>()?.EnableOutline();
    }

    InterestPoint GetHighestPriorityItem()
    {
        InterestPoint highestPriority = default;
        foreach (InterestPoint interestPoint in interestPoints)
        {
            if(highestPriority == default)
            {
                highestPriority = interestPoint;
                continue;
            }
            if(interestPoint.score > highestPriority.score)
            {
                highestPriority = interestPoint;
            }
        }
        return highestPriority;
    }

    public GameObject GetFirstItem()
    {
        return GetHighestPriorityItem().go;
    }
    public IInteractable GetInteractable()
    {
        if(interestPoints.Count == 0) return null;
        if (GetHighestPriorityItem().go.GetComponent<IInteractable>() == default) return null;
        return GetHighestPriorityItem().go.GetComponent<IInteractable>();
    }
    public ISmashable GetSmashable()
    {
        if(interestPoints.Count == 0) return null;
        if (GetHighestPriorityItem().go.GetComponent<ISmashable>() == default) return null;
        return GetHighestPriorityItem().go.GetComponent<ISmashable>();
    }
    public IGrabbable GetGrabbable()
    {
        if(interestPoints.Count == 0) return null;
        if (GetHighestPriorityItem().go.GetComponent<IGrabbable>() == default) return null;
        return GetHighestPriorityItem().go.GetComponent<IGrabbable>();
    }

    public void AddInterestPoint(InterestPoint newInterestPoint)
    {
        if (IsThereAWall(newInterestPoint.go.transform)) return;
        
        foreach (InterestPoint interestPoint in interestPoints)
        {
            if (interestPoint.go == newInterestPoint.go)
            {
                interestPoint.ReCalculateScore(newInterestPoint.distance, newInterestPoint.centerDistance);
                interestPoint.RefreshValidity();
                return;
            }
        }
        interestPoints.Add(newInterestPoint);
    }
    public void RemoveInterestPoint(InterestPoint interestPoint)
    {
        if (!interestPoints.Contains(interestPoint)) return;
        interestPoints.Remove(interestPoint);
    }

    public void Clear()
    {
        interestPoints.Clear();
    }
    
    public bool IsThereAWall(Transform objectToCheck)
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, objectToCheck.position - transform.position, out hit, Vector3.Distance(transform.position, objectToCheck.position), obstacleLayerMask))
        {
            return true;
        }
        return false;
    }
}

[Serializable]
public class InterestPoint
{
    public GameObject go;
    public double score;
    public float distance, centerDistance;
    public AnimationCurve rangeCurve, centerCurve;
    public bool validity;
    public InterestPoint(GameObject newGo, float newDistance, float newCenterDistance, AnimationCurve newRangeCurve, AnimationCurve newCenterCurve)
    {
        go = newGo;
        distance = newDistance;
        centerDistance = newCenterDistance;
        rangeCurve = newRangeCurve;
        centerCurve = newCenterCurve;
        score = rangeCurve.Evaluate(distance) + centerCurve.Evaluate(centerDistance);
    }

    public double ReCalculateScore(float newDistance, float newCenterDistance)
    {
        if (rangeCurve.Evaluate(newDistance) + centerCurve.Evaluate(newCenterDistance) != score)
        {
            distance = newDistance;
            centerDistance = newCenterDistance;
            score = rangeCurve.Evaluate(newDistance) + centerCurve.Evaluate(newCenterDistance);
        }
        return score;
    }

    public void RefreshValidity()
    {
        validity = true;
    }

    public double GetScore()
    {
        return score;
    }
}