using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GasCanister : Item, ISmashable
{
    [SerializeField] AudioSource audioSource;
    [SerializeField] GameObject scrapPilePrefab;
    [SerializeField] Transform scrapPilePivot;
    private bool hasBeenSmashed = false;
    public void Smash()
    {
        if (!hasBeenSmashed)
        {
            hasBeenSmashed = true;
            StartCoroutine(FeedbackSmash());    
        }
    }

    public override void OnHitGround(Collision collision)
    {
        if (thrown) Smash();
        base.OnHitGround(collision);
    }

    private void FeedbackSmash()
    {
        if (scrapPilePrefab != null && scrapPilePivot != null)
        Instantiate(scrapPilePrefab, scrapPilePivot.position, Quaternion.identity);
        else Debug.Log("No scrapPilePrefab or scrapPilePivot on " + this.name);
        Debug.Log("Breaking the Gas Canister");
        audioSource.Play();
        CreateGas();
        transform.GetChild(0).gameObject.SetActive(false);
    }
}
