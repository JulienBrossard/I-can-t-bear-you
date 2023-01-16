using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Washer : Item, ISmashable
{
    [SerializeField] AudioSource audioSource;
    [SerializeField] GameObject scrapPilePrefab;
    [SerializeField] Transform scrapPilePivot;
    public void Smash()
    {
        if (scrapPilePrefab != null && scrapPilePivot != null)
            Instantiate(scrapPilePrefab, scrapPilePivot.position, Quaternion.identity);
        else Debug.Log("No scrapPilePrefab or scrapPilePivot on " + this.name);
        Debug.Log("Breaking the Wasger");
        audioSource.Play();
        Electrocute();
        DeleteItem();
    }
}
