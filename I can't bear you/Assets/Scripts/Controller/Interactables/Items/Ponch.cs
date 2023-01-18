using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ponch : Item,IInteractable,ISmashable
{
    [SerializeField] AudioClip breakClip, bubbleClip;
    [SerializeField] AudioSource audioSource;
    [SerializeField] private GameObject psBubblePoisoned;
    [SerializeField] ParticleSystem interactParticle, smashParticle;
    bool spicedUp;
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
        audioSource.PlayOneShot(breakClip);
        if (smashParticle)
            smashParticle.Play();
        else Debug.Log("No Smash Particle on " + this.name);
        yield return new WaitForSeconds(.5f);
        CreatePuddle();
        transform.GetChild(0).gameObject.SetActive(false);
    }
}
