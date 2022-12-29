using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Douche : Item,IInteractable,ISmashable
{
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip brokenSound, showerOpenSound;
    bool isBroken = false, hasBeenTurnedOn = false;
    public void Interact(Vector3 sourcePos)
    {
        if (!hasBeenTurnedOn)
        {
            hasBeenTurnedOn = true;
            Debug.Log("Interacting douche");
            audioSource.PlayOneShot(showerOpenSound);
            CreatePuddle();
        }
    }

    public void Smash()
    {
        if (!isBroken)
        {
            isBroken = true;
            Debug.Log("Breaking the douche");
            audioSource.PlayOneShot(brokenSound);
            CreatePuddle();
        }
    }
}
