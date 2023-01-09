using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoneyPot : Item,IInteractable,ISmashable
{
    bool hasBeenEaten = false;
    [SerializeField, Range(0f, 1f)] private float bearserkerToAdd;
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip honeyEat, potBreak;
    [SerializeField] ParticleSystem interactParticle, smashParticle;
    public void Interact(Vector3 sourcePos)
    {
        if (!hasBeenEaten)
        {
            if (interactParticle)
                interactParticle.Play();
            else Debug.Log("No Interact Particle on " + this.name);
            hasBeenEaten = true;
            Debug.Log("Eating Honey Pot");
            audioSource.PlayOneShot(honeyEat);
            BearserkerGaugeManager.instance.AddBearserker(bearserkerToAdd);
        }
    }

    public void Smash()
    {
        StartCoroutine(FeedbackSmash());
    }

    public override void OnHitGround(Collision collision)
    {
        if (thrown) StartCoroutine(FeedbackSmash());
        base.OnHitGround(collision);
    }

    IEnumerator FeedbackSmash()
    {
        Debug.Log("Breaking the ponch");
        audioSource.PlayOneShot(potBreak);
        if (smashParticle)
            smashParticle.Play();
        else Debug.Log("No Smash Particle on " + this.name);
        yield return new WaitForSeconds(.5f);
        if (!hasBeenEaten)
        {
            CreatePuddle();
        }
        DeleteItem();
    }
}
