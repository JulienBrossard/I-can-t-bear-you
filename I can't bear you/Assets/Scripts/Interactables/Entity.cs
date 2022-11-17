using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour, IAffectable
{
    public virtual void Slow()
    {
        Debug.Log("Slowing " + gameObject.name);
    }

    public virtual void Slide()
    {
        Debug.Log("Sliding " + gameObject.name);
    }

    public virtual void Electrocute()
    {
        Debug.Log("Electrocuted " + gameObject.name);
    }

    public bool ignitable;
    public virtual void Ignite()
    {
        if(ignitable) return;
        Debug.Log("Ignited " + gameObject.name);
    }

    public virtual void Stomp()
    {
        Debug.Log("Stomped " + gameObject.name);
    }

    public virtual void Explode()
    {
        Debug.Log("Exploded " + gameObject.name);
    }

    public float asphyxiation;
    public float asphyxiationToDie;
    public virtual void Asphyxiate()
    {
        asphyxiation += 0.001f;
        if (asphyxiation > asphyxiationToDie)
        {
            DieFromAsphyxiation();
        }
    }
    public virtual void DieFromAsphyxiation()
    {
        
        Debug.Log("Asphyxiated " + gameObject.name);
    }

    public virtual void Poison()
    {
        Debug.Log("Poisoned " + gameObject.name);
    }

    public virtual void Dissolve()
    {
        Debug.Log("Dissolved " + gameObject.name);
    }

    public void Grind()
    {
        Debug.Log("Grinded " + gameObject.name);
    }
}
