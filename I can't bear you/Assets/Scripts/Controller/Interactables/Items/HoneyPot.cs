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
    [SerializeField] ParticleSystem interactParticle;
    [SerializeField] GameObject scrapPilePrefab;
    [SerializeField] Transform scrapPilePivot;
    public void Interact(Vector3 sourcePos)
    {
        if (!hasBeenEaten)
        {
            if (interactParticle)
                interactParticle.Play();
            else Debug.Log("No Interact Particle on " + this.name);
            Debug.Log("Eating Honey Pot");
            audioSource.PlayOneShot(honeyEat);
            BearserkerGaugeManager.instance.AddBearserker(bearserkerToAdd);
            DeleteItem();
            hasBeenEaten = true;
        }
    }

    public void Smash()
    {
        FeedbackSmash();
    }

    public override void OnHitGround(Collision collision)
    {
        if (thrown) FeedbackSmash();
        base.OnHitGround(collision);
    }

    private void FeedbackSmash()
    {
        Debug.Log("Breaking the ponch");
        audioSource.PlayOneShot(potBreak);
        if (!hasBeenEaten)
            CreatePuddle();
        transform.GetChild(0).gameObject.SetActive(false);
        if (scrapPilePrefab != null && scrapPilePivot != null)
            Instantiate(scrapPilePrefab, scrapPilePivot.position, Quaternion.identity);
        else Debug.Log("No scrapPilePrefab or scrapPilePivot on " + this.name);
    }
}
