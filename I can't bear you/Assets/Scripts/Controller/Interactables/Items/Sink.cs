using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sink : Item, ISmashable
{
    bool isBroken = false;
    [SerializeField] AudioSource audioSource;
    [SerializeField] GameObject scrapPilePrefab;
    [SerializeField] Transform scrapPilePivot;
    public void Smash()
    {
        if (!isBroken)
        {
            if (scrapPilePrefab != null && scrapPilePivot != null)
                Instantiate(scrapPilePrefab, scrapPilePivot.position, Quaternion.identity);
            else Debug.Log("No scrapPilePrefab or scrapPilePivot on " + this.name);
            isBroken = true;
            Debug.Log("Breaking the Sink");
            audioSource.Play();
            CreatePuddle();
            DeleteItem();
        }
    }
}
