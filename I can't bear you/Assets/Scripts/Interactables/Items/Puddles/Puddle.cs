using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puddle : MonoBehaviour, IAffectable
{
    public bool ignitable, ignited, conductor,charged, acid, slippy, sticky;

    public void ApplyEffects(GameObject go)
    {
        if(ignited) go.GetComponent<Entity>().Ignite();
        if(acid) go.GetComponent<Entity>().Dissolve();
        if(slippy) go.GetComponent<Entity>().Slide();
        if(sticky) go.GetComponent<Entity>().Slow();
    }

    public void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.GetComponent<Entity>() == default) return;
        ApplyEffects(other.gameObject);
        Debug.Log("Applying effects to " + other.gameObject.name);
    }

    private void OnTriggerExit(Collider other)
    {
        if(sticky) other.gameObject.GetComponent<Entity>()?.StopSlow();
    }

    [SerializeField] private GameObject zonePrefab;
    public virtual void CreateZone()
    {
        Instantiate(zonePrefab, transform.position, Quaternion.identity, transform);
    }
    public void Electrocute()
    {
        if(!conductor) return;
        if(charged) return;
        charged = true;
        CreateZone();
    }

    public void Ignite()
    {
        ignited = true;
    }

    public void Explode()
    {
        if(ignitable) Ignite();
    }
}
