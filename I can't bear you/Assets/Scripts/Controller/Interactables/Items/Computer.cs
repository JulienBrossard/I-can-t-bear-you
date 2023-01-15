using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Computer : Item, ISmashable
{
    [SerializeField] AudioSource audioSource;
    [SerializeField] GameObject scrapPilePrefab;
    [SerializeField] Transform scrapPilePivot;
    public void Smash()
    {
        if (scrapPilePrefab != null && scrapPilePivot != null)
            Instantiate(scrapPilePrefab, scrapPilePivot.position, Quaternion.identity);
        else Debug.Log("No scrapPilePrefab or scrapPilePivot on " + this.name);
        Debug.Log("Breaking the Computer");
        audioSource.Play();
        Electrocute();
        DeleteItem();
    }
}
