using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Item : MonoBehaviour, IAffectable
{
    public virtual void DeleteItem()
    {
        Destroy(gameObject);
    }
    
    [SerializeField] private GameObject puddlePrefab;
    public virtual GameObject CreatePuddle()
    {
        return Instantiate(puddlePrefab, new Vector3(transform.position.x,0.5f,transform.position.z), Quaternion.identity);
    }
    
    [SerializeField] private GameObject zonePrefab;
    [HideInInspector] public GameObject zone;
    public virtual GameObject CreateZone()
    {
        zone = Instantiate(zonePrefab, transform.position, Quaternion.identity,transform);
        return zone;
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
    [SerializeField] private bool itemCharged, itemConductor;
    public bool charged { get => itemCharged; set => itemCharged = value;}
    public bool conductor { get => itemConductor; set => itemConductor = value; }

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

    public bool fallable;
    public List<FallAsset> falls = new List<FallAsset>();
    public virtual void Fall()
    {
        if(!fallable)return;
        Debug.Log( "Falling " + gameObject.name);
        GetComponent<Rigidbody>().isKinematic = false;
        GetComponent<Rigidbody>().AddForce(falls[0].Dir * falls[0].force);
    }

    public virtual void Explode()
    {
        Debug.Log("Exploded " + gameObject.name);
    }

    public bool consumable;
    public virtual bool Consume()
    {
        if (!consumable) return false;
        Debug.Log("Being consumed " + gameObject.name);
        return false;
    }
}

[Serializable]
public class FallAsset
{
    [SerializeField] private Vector3 dir;
    public Vector3 Dir{get => dir; set => dir = value.normalized;}
    public float force;
}
