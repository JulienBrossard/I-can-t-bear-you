using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Douche : Item,IInteractable,ISmashable
{
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip brokenSound, showerOpenSound;
    [SerializeField] ParticleSystem interactParticle;
    [SerializeField] GameObject scrapPilePrefab;
    [SerializeField] Transform scrapPilePivot;
    bool isBroken = false, hasBeenTurnedOn = false;
    public void Interact(Vector3 sourcePos)
    {
        if (!hasBeenTurnedOn)
        {
            if (interactParticle)
                interactParticle.Play();
            else Debug.Log("No Interact Particle on " + this.name);
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
            if (scrapPilePrefab != null && scrapPilePivot != null)
                Instantiate(scrapPilePrefab, scrapPilePivot.position, Quaternion.identity);
            else Debug.Log("No scrapPilePrefab or scrapPilePivot on " + this.name);
            isBroken = true;
            Debug.Log("Breaking the douche");
            audioSource.PlayOneShot(brokenSound);
            CreatePuddle();
        }
    }
}
