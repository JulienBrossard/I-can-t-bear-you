using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ponch : Item,IInteractable,ISmashable
{
    [SerializeField] AudioClip breakClip, bubbleClip;
    [SerializeField] AudioSource audioSource;
    [SerializeField] private GameObject psBubblePoisoned;
    [SerializeField] ParticleSystem interactParticle, smashParticle;
    [HideInInspector] public bool spicedUp;
    [SerializeField] GameObject scrapPilePrefab;
    [SerializeField] Transform scrapPilePivot;
    public void Interact(Vector3 sourcePos)
    {
        // Spice up the Ponch
        if (!spicedUp)
        {
            if (interactParticle)
                interactParticle.Play();
            else Debug.Log("No Interact Particle on " + this.name);
            Debug.Log("Interacting Ponch");
            audioSource.PlayOneShot(bubbleClip);
            psBubblePoisoned.SetActive(true);
            spicedUp = true;
        }
    }
        

    public void Smash()
    {
        StartCoroutine(FeedbackSmash());
    }

    IEnumerator FeedbackSmash()
    {
        Debug.Log("Breaking the ponch");
        GetComponent<Collider>().enabled = false;
        audioSource.PlayOneShot(breakClip);
        if (smashParticle)
            smashParticle.Play();
        else Debug.Log("No Smash Particle on " + this.name);
        yield return new WaitForSeconds(.5f);
        CreatePuddle();
        DeleteItem();
    }
}
