using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarBattery : Item,ISmashable,IGrabbable
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private float throwForce;
    [SerializeField] private BoxCollider collider;
    private bool shouldBreakOnHit;
    
    public void Smash()
    {
        Debug.Log("Breaking the battery");
        CreatePuddle().GetComponent<Puddle>().Electrocute();
        DeleteItem();
    }

    public Transform Grab(Transform hand)
    {
        Debug.Log("Grabbing Car Battery");
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

    public void Throw(Vector3 dir)
    {
        SetAsReleased();
        rb.AddForce(dir * throwForce, ForceMode.Impulse);
        StartCoroutine(WaitForBreakable());
    }

    public void Drop()
    {
        SetAsReleased();
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
