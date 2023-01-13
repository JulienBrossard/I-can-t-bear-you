using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestoyableObject : Item, ISmashable
{
    [SerializeField] ParticleSystem smashParticle;
    public void Smash()
    {
        if (smashParticle)
            smashParticle.Play();
        else Debug.Log("No Smash Particle on " + this.name);
        Debug.Log("Obj has been Destroyed");
        DeleteItem();
    }
}
