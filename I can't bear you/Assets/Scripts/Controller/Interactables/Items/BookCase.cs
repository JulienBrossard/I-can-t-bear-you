using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

public class BookCase : Item, ISmashable, IInteractable
{
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip objectFall, woodBreak;
    [SerializeField] ParticleSystem interactParticle;
    [SerializeField] GameObject scrapPilePrefab;
    [SerializeField] private Transform scrapPilePivot;
    [SerializeField] private Animation bookCaseAnim;
    [SerializeField] private BoxCollider outlineCollider;
    private bool hasBeenSmashed = false;
    private bool hasBeenInteracted = false;

    public void Interact(Vector3 sourcePos)
    {
        if (!hasBeenInteracted)
        {
            hasBeenInteracted = true;
            if (interactParticle)
                interactParticle.Play();
            else Debug.Log("No Interact Particle on " + this.name);
            Debug.Log("Interacting BookCase");
            audioSource.PlayOneShot(objectFall);
            //Fall(sourcePos);
            bookCaseAnim.Play();
            //outlineCollider.enabled = false;
        }
    }

    public void Smash()
    {
        if (!hasBeenSmashed)
        {
            StartCoroutine(FeedbackSmash());
        }
    }

    IEnumerator FeedbackSmash()
    {
        Debug.Log("Breaking the BookCase");
        hasBeenSmashed = true;
        audioSource.PlayOneShot(woodBreak);
        if (scrapPilePrefab != null && scrapPilePivot != null)
            Instantiate(scrapPilePrefab, scrapPilePivot.position, Quaternion.identity);
        else Debug.Log("No scrapPilePrefab or scrapPilePivot on " + this.name);
        yield return new WaitForSeconds(.5f);
        transform.GetChild(0).gameObject.SetActive(false);
    }
}
