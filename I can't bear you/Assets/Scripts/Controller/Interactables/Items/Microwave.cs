using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Microwave : Item,ISmashable
{
    [SerializeField] AudioSource breakSource;
    [SerializeField] AudioClip breakSound;
    [SerializeField] ParticleSystem smashParticle;
    bool isBroken = false;
    public void Smash()
    {
        Break();
    }
    public override void OnHitGround(Collision collision)
    {
        if (thrown) Break();
        base.OnHitGround(collision);
    }
    public void Break()
    {
        if (!isBroken)
        {
            if (smashParticle)
                smashParticle.Play();
            else Debug.Log("No Smash Particle on " + this.name);
            Debug.Log("Breaking Microwave");
            breakSource.PlayOneShot(breakSound);
            Electrocute();
            isBroken = true;
        }
    }
}
