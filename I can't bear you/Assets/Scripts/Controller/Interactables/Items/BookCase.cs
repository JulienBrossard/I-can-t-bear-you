using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BookCase : Item, ISmashable, IInteractable
{
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip objectFall, woodBreak;
    [SerializeField] ParticleSystem interactParticle, smashParticle;

    public void Interact(Vector3 sourcePos)
    {
        if (interactParticle)
            interactParticle.Play();
        else Debug.Log("No Interact Particle on " + this.name);
        Debug.Log("Interacting BookCase");
        audioSource.PlayOneShot(objectFall);
        Fall(sourcePos);
    }

    public void Smash()
    {
        StartCoroutine(FeedbackSmash());
    }

    IEnumerator FeedbackSmash()
    {
        Debug.Log("Breaking the BookCase");
        audioSource.PlayOneShot(woodBreak);
        if (smashParticle)
            smashParticle.Play();
        else Debug.Log("No Smash Particle on " + this.name);
        yield return new WaitForSeconds(.5f);
        DeleteItem();
    }
}
