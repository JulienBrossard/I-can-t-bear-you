using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grabbable : MonoBehaviour, IInteractable
{
    public void Interact()
    {
        Debug.Log("Grabbing " + gameObject.name);
    }
}
