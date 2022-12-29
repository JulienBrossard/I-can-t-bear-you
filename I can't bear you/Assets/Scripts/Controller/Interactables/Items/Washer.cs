using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Washer : Item, ISmashable
{
    [SerializeField] AudioSource audioSource;
    public void Smash()
    {
        Debug.Log("Breaking the Wasger");
        audioSource.Play();
        Electrocute();
    }
}
