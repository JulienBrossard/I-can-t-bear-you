using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Computer : Item, ISmashable
{
    [SerializeField] AudioSource audioSource;
    public void Smash()
    {
        Debug.Log("Breaking the Computer");
        audioSource.Play();
        Electrocute();
    }
}
