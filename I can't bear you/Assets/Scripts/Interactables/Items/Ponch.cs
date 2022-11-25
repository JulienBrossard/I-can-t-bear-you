using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ponch : Item,IInteractable,ISmashable
{
    [SerializeField] private GameObject psBubblePoisoned;
    public void Interact()
    {
        if(poisoned) return;
        Debug.Log("Poisoning Ponch");
        psBubblePoisoned.SetActive(true);
        Poison();
    }

    public void Smash()
    {
        Debug.Log("Breaking the ponch");
        CreatePuddle();
        DeleteItem();
    }
}
