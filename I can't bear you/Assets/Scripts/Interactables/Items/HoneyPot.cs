using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoneyPot : MonoBehaviour,IInteractable,ISmashable,IGrabbable
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private BoxCollider collider;
    [SerializeField, Range(0f, 1f)] private float bearserkerToAdd;
    public void Interact()
    {
        Debug.Log("Eating Honey Pot");
        BearserkerGaugeManager.instance.AddBearserker(bearserkerToAdd);
    }

    public void Smash()
    {
        Debug.Log("Breaking Honey Pot");
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
    public void Throw(Vector3 dir, float force)
    {
        SetAsReleased();
    }
    public void SetAsReleased()
    {
        collider.enabled = true;
        transform.SetParent(null);
        rb.isKinematic = false;
    }
}
