using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public virtual void DeleteItem()
    {
        Destroy(gameObject);
    }
    
    [SerializeField] private GameObject puddlePrefab;
    public virtual void CreatePuddle()
    {
        Instantiate(puddlePrefab, transform.position, Quaternion.identity);
    }

    public bool charged;
    public virtual void Electrocute()
    {
        Debug.Log("Electrocuted " + gameObject.name);
        charged = true;
    }

    public bool ignitable;
    public virtual void Ignite()
    {
        if(!ignitable) return;
        Debug.Log("Ignited " + gameObject.name);
    }

    public virtual void Fall()
    {
        Debug.Log("Falling " + gameObject.name);
    }

    public virtual void Explode()
    {
        Debug.Log("Exploded " + gameObject.name);
    }

    public bool poisonable, poisoned;
    public virtual void Poison()
    {
        if(!poisonable) return;
        poisoned = true;
        Debug.Log("Is now poisoned " + gameObject.name);
    }

    public bool consumable;
    public virtual bool Consume()
    {
        if (!consumable) return false;
        Debug.Log("Being consumed " + gameObject.name);
        return poisoned;
    }
}
