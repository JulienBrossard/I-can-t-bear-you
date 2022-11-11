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

    public virtual void Electrocute()
    {
        
    }

    public bool ignitable;
    public virtual void Ignite()
    {
        if(!ignitable) return;
    }

    public virtual void Fall()
    {
        
    }

    public virtual void Explode()
    {
        
    }

    public bool poisonable, poisoned;
    public virtual void Poison()
    {
        if(!poisonable) return;
        poisoned = true;
    }

    public bool consumable;
    public virtual bool Consume()
    {
        return poisoned;
    }
}
