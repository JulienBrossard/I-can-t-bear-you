using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puddle : MonoBehaviour, IAffectable
{
    public bool slippy, sticky;

    public void ApplyEffects(GameObject go)
    {
        if(slippy) go.GetComponent<Entity>().Slide();
        if(sticky) go.GetComponent<Entity>().Slow();
    }

    public void DisapplyEffects(GameObject go)
    {
        if(sticky) go.GetComponent<Entity>()?.StopSlow();
    }

    public void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.GetComponent<IAffectable>() != default)
        {
            if(charged) other.gameObject.GetComponent<IAffectable>().Electrocute(gameObject);
            Debug.Log("Applying effects to " + other.gameObject.name);
            return;
        }
        if(other.gameObject.GetComponent<Entity>() != default)
        {
            ApplyEffects(other.gameObject);
            Debug.Log("Applying effects to " + other.gameObject.name);
            return;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        DisapplyEffects(other.gameObject);
        Debug.Log("Disapplying effects to " + other.gameObject.name);
    }

    [SerializeField] private GameObject zonePrefab;
    [HideInInspector] public GameObject zone;
    public virtual void CreateZone()
    {
        zone = Instantiate(zonePrefab, transform.position, Quaternion.identity,transform);
    }
    public virtual void EnableZone()
    {
        zone.SetActive(true);
    }
    public virtual void DisableZone()
    {
        zone.SetActive(false);
    }


    public bool emitterDependant;
    public GameObject emitter;
    public float zoneSize;
    [SerializeField] private bool puddleCharged,puddleConductor;
    public bool charged { get => puddleCharged; set => puddleCharged = value;}
    public bool conductor { get => puddleConductor; set => puddleConductor = value; }

    public virtual void Electrocute()
    {
        if(!conductor) return;
        if (charged) return;
        
        Debug.Log("Electrocuted " + gameObject.name + " with no depedancy");
        charged = true;
        EnableZone();
    }
    public virtual void Electrocute(GameObject emitter)
    {
        if(!conductor) return;
        if (charged) return;
        
        Debug.Log("Electrocuted " + gameObject.name + " with depedancy of " + emitter.name);
        emitterDependant = true;
        this.emitter = emitter;
        charged = true;
        EnableZone();
    }
    public virtual void DeElectrocute()
    {
        Debug.Log("DeElectrocuted " + gameObject.name);
        charged = false;
        DisableZone();
    } 

    private void Update()
    {
        if(!charged) return;
        if(emitterDependant)
        {
            if(emitter == null)
            {
                DeElectrocute();
                return;
            }
            if(!emitter.GetComponent<IAffectable>().charged)
            {
                DeElectrocute();
                return;
            }
        }
    }

    public void Explode()
    {
        
    }
}
