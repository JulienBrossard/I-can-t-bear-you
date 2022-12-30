using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sink : Item, ISmashable
{
    bool isBroken = false;
    [SerializeField] AudioSource audioSource;
    [SerializeField] ParticleSystem smashParticle;
    public void Smash()
    {
        if (!isBroken)
        {
            if (smashParticle)
                smashParticle.Play();
            else Debug.Log("No Smash Particle on " + this.name);
            isBroken = true;
            Debug.Log("Breaking the Sink");
            audioSource.Play();
            CreatePuddle();
        }
    }
}
