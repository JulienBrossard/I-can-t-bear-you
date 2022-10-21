using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractablesManager : MonoBehaviour
{
    public List<GameObject> interactables, smashables;

    public void AddToInteractables(GameObject obj)
    {
        if (interactables.Contains(obj)) return;
        interactables.Add(obj);
    }
    public void RemoveFromInteractables(GameObject obj)
    {
        if (!interactables.Contains(obj)) return;
        interactables.Remove(obj);
    }
    public void AddToSmashables(GameObject obj)
    {
        if (smashables.Contains(obj)) return;
        smashables.Add(obj);
    }
    public void RemoveFromSmashables(GameObject obj)
    {
        if (!smashables.Contains(obj)) return;
        smashables.Remove(obj);
    }
}
