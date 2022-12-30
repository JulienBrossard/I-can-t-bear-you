using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ponch : Item,IInteractable,ISmashable
{
    [SerializeField] AudioClip breakClip, bubbleClip;
    [SerializeField] AudioSource audioSource;
    [SerializeField] private GameObject psBubblePoisoned;
    public void Interact(Vector3 sourcePos)
    {
        // Spice up the Ponch
        Debug.Log("Interacting Ponch");
        audioSource.PlayOneShot(bubbleClip);
        psBubblePoisoned.SetActive(true);
    }

    public void Smash()
    {
        StartCoroutine(FeedbackSmash());
    }

    IEnumerator FeedbackSmash()
    {
        Debug.Log("Breaking the ponch");
        audioSource.PlayOneShot(breakClip);
        yield return new WaitForSeconds(.5f);
        CreatePuddle();
        DeleteItem();
    }
}
