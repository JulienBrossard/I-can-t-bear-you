using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GasCanister : Item, ISmashable
{
    [SerializeField] AudioSource audioSource;
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
        Debug.Log("Breaking the Gas Canister");
        audioSource.Play();
        CreateGas();
        yield return new WaitForSeconds(1.6f);
        DeleteItem();
    }
}
