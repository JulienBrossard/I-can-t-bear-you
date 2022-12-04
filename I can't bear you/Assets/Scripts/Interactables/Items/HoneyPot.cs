using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoneyPot : Item,IInteractable,ISmashable,IGrabbable
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private BoxCollider collider;
    [SerializeField, Range(0f, 1f)] private float bearserkerToAdd;
    [SerializeField] private float throwForce;
    private bool shouldBreakOnHit;
    public void Interact(Vector3 sourcePos)
    {
        Debug.Log("Eating Honey Pot");
        BearserkerGaugeManager.instance.AddBearserker(bearserkerToAdd);
    }

    public void Smash()
    {
        Debug.Log("Breaking Honey Pot");
        CreatePuddle();
        DeleteItem();
    }

    public Transform Grab(Transform hand)
    {
        Debug.Log("Grabbing Honey Pot");
        SetAsGrabbed(hand);
        return transform;
    }

    public void SetAsGrabbed(Transform hand)
    {
        collider.enabled = false;
        transform.SetParent(hand);
        rb.isKinematic = true;
        transform.localPosition = Vector3.zero;
    }
    public void Drop()
    {
        SetAsReleased();
    }
    public void Throw(Vector3 dir)
    {
        SetAsReleased();
        rb.AddForce(dir * throwForce, ForceMode.Impulse);
        StartCoroutine(WaitForBreakable());
    }
    public void SetAsReleased()
    {
        collider.enabled = true;
        transform.SetParent(null);
        rb.isKinematic = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(!shouldBreakOnHit) return;
        Smash();
    }

    IEnumerator WaitForBreakable()
    {
        yield return new WaitForSeconds(0.25f);
        shouldBreakOnHit = true;
    }
}
