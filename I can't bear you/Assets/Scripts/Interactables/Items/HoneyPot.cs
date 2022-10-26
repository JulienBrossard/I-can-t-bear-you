using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoneyPot : MonoBehaviour,IInteractable,ISmashable,IGrabbable
{
    public void Interact()
    {
        Debug.Log("Eating Honey Pot");
    }

    public void Smash()
    {
        Debug.Log("Breaking Honey Pot");
    }

    public void Grab()
    {
        Debug.Log("Grabbing Honey Pot");
    }
}
