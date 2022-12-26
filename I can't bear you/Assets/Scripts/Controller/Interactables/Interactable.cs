using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour, IInteractable
{
    public void Interact(Vector3 sourcePos)
    {
        Debug.Log("Interacting with " + gameObject.name);
    }
}
