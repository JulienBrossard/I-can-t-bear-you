using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InterestPointsManager : MonoBehaviour
{
    public List<InterestPoint> interactables, smashables;

    public void AddToInteractables(InterestPoint interactable)
    {
        foreach (InterestPoint interestPoint in interactables)
        {
            if (interestPoint.go == interactable.go)
            {
                interestPoint.ReCalculateScore(interactable.distance, interactable.centerDistance);
                return;
            }
        }
        interactables.Add(interactable);
    }
    public void RemoveFromInteractables(InterestPoint interactable)
    {
        if (!interactables.Contains(interactable)) return;
        interactables.Remove(interactable);
    }
    public void AddToSmashables(InterestPoint smashable)
    {
        foreach (InterestPoint interestPoint in smashables)
        {
            interestPoint.ReCalculateScore(smashable.distance, smashable.centerDistance);
            return;
        }
        smashables.Add(smashable);
    }
    public void RemoveFromSmashables(InterestPoint smashable)
    {
        if (!smashables.Contains(smashable)) return;
        smashables.Remove(smashable);
    }
}

[Serializable]
public class InterestPoint
{
    public GameObject go;
    public float score, distance, centerDistance;
    public InterestPoint(GameObject newGo, float newDistance, float newCenterDistance)
    {
        go = newGo;
        distance = newDistance;
        centerDistance = newCenterDistance;
        score = distance * centerDistance;
    }

    public float ReCalculateScore(float newDistance, float newCenterDistance)
    {
        if (newDistance * newCenterDistance > score)
        {
            score = newDistance * newCenterDistance;
        }
        return score;
    }

    public float GetScore()
    {
        return score;
    }
}
