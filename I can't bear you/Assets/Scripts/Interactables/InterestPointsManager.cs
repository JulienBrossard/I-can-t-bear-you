using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class InterestPointsManager : MonoBehaviour
{
    public List<InterestPoint> interestPoints;

    private void FixedUpdate()
    {
        foreach (InterestPoint interestPoint in interestPoints)
        {
            if (!interestPoint.validity)
            {
                RemoveInterestPoint(interestPoint);
                return;
            }
            interestPoint.validity = false;
        }
        SortInterestPoints();
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
        outlineGoBuffer = interestPoints[0]?.go;
        outlineGoBuffer.GetComponent<Outline>()?.EnableOutline();
    }
    public IInteractable GetInteractable()
    {
        foreach (InterestPoint interestPoint in interestPoints)
        {
            if(interestPoint.go.GetComponent<IInteractable>() != null)
            {
                return interestPoint.go.GetComponent<IInteractable>();
            }
        }
        return null;
    }
    public ISmashable GetSmashable()
    {
        foreach (InterestPoint interestPoint in interestPoints)
        {
            if(interestPoint.go.GetComponent<ISmashable>() != null)
            {
                return interestPoint.go.GetComponent<ISmashable>();
            }
        }
        return null;
    }
    public IGrabbable GetGrabbable()
    {
        foreach (InterestPoint interestPoint in interestPoints)
        {
            if(interestPoint.go.GetComponent<IGrabbable>() != null)
            {
                return interestPoint.go.GetComponent<IGrabbable>();
            }
        }
        return null;
    }

    public void AddInterestPoint(InterestPoint newInterestPoint)
    {
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
}

[Serializable]
public class InterestPoint
{
    public GameObject go;
    public float score, distance, centerDistance;
    public AnimationCurve rangeCurve, centerCurve;
    public bool validity;
    public InterestPoint(GameObject newGo, float newDistance, float newCenterDistance, AnimationCurve newRangeCurve, AnimationCurve newCenterCurve)
    {
        go = newGo;
        distance = newDistance;
        centerDistance = newCenterDistance;
        rangeCurve = newRangeCurve;
        centerCurve = newCenterCurve;
        score = rangeCurve.Evaluate(distance) * centerCurve.Evaluate(centerDistance);
    }

    public float ReCalculateScore(float newDistance, float newCenterDistance)
    {
        if (rangeCurve.Evaluate(newDistance) * centerCurve.Evaluate(newCenterDistance) > score)
        {
            distance = newDistance;
            centerDistance = newCenterDistance;
            score = rangeCurve.Evaluate(newDistance) * centerCurve.Evaluate(newCenterDistance);
        }
        return score;
    }

    public void RefreshValidity()
    {
        validity = true;
    }

    public float GetScore()
    {
        return score;
    }
}