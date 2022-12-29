using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sink : Item, ISmashable
{
    bool isBroken = false;
    [SerializeField] AudioSource audioSource;
    public void Smash()
    {
        if (!isBroken)
        {
            isBroken = true;
            Debug.Log("Breaking the Sink");
            audioSource.Play();
            CreatePuddle();
        }
    }
}
