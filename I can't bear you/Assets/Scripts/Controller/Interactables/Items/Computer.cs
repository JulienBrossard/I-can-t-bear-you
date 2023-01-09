using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Computer : Item, ISmashable
{
    [SerializeField] AudioSource audioSource;
    [SerializeField] ParticleSystem smashParticle;
    public void Smash()
    {
        if (smashParticle)
            smashParticle.Play();
        else Debug.Log("No Smash Particle on " + this.name);
        Debug.Log("Breaking the Computer");
        audioSource.Play();
        Electrocute();
    }
}
