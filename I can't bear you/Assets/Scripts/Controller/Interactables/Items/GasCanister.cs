using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GasCanister : Item, ISmashable
{
    [SerializeField] AudioSource audioSource;
    [SerializeField] GameObject scrapPilePrefab;
    [SerializeField] Transform scrapPilePivot;
    public void Smash()
    {
        StartCoroutine(FeedbackSmash());
    }

    public override void OnHitGround(Collision collision)
    {
        if (thrown) Smash();
        base.OnHitGround(collision);
    }

    IEnumerator FeedbackSmash()
    {
        if (scrapPilePrefab != null && scrapPilePivot != null)
        Instantiate(scrapPilePrefab, scrapPilePivot.position, Quaternion.identity);
        else Debug.Log("No scrapPilePrefab or scrapPilePivot on " + this.name);
        Debug.Log("Breaking the Gas Canister");
        audioSource.Play();
        CreateGas();
        yield return new WaitForSeconds(1.6f);
        DeleteItem();
    }
}
