using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BookCase : Item, ISmashable, IInteractable
{
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip objectFall, woodBreak;

    public void Interact(Vector3 sourcePos)
    {
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
        yield return new WaitForSeconds(.5f);
        DeleteItem();
    }
}
