using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    public virtual void Slow()
    {
        
    }

    public virtual void Slide()
    {
        
    }

    public virtual void Electrocute()
    {
        
    }

    public bool ignitable;
    public virtual void Ignite()
    {
        if(ignitable) return;
    }

    public virtual void Stomp()
    {
        
    }

    public virtual void Explode()
    {
        
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
        
    }

    public virtual void Poison()
    {
        
    }

    public virtual void Dissolve()
    {
        
    }

    public void Grind()
    {
        
    }
}
