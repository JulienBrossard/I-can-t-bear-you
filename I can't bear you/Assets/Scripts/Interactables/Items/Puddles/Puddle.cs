using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puddle : MonoBehaviour
{
    public bool ignitable, ignited, conductor,charged, acid, slippy, sticky;

    public void ApplyEffects(GameObject go)
    {
        if(ignited) go.GetComponent<Entity>()?.Ignite();
        if(charged) go.GetComponent<Entity>()?.Electrocute();
        if(acid) go.GetComponent<Entity>()?.Dissolve();
        if(slippy) go.GetComponent<Entity>()?.Slide();
        if(sticky) go.GetComponent<Entity>()?.Slow();
    }

    protected void OnTriggerEnter(Collider other)
    {
        ApplyEffects(other.gameObject);
        Debug.Log("Applying effects to " + other.gameObject.name);
    }
}
