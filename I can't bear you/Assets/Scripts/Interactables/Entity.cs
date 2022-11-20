using UnityEngine;

public class Entity : MonoBehaviour, IAffectable
{
    [HideInInspector] public float currentSpeedRatio = 1f;
    public EntityData entityData;
    public virtual void Slow()
    {
        currentSpeedRatio = entityData.slowSpeedRatio;
        Debug.Log("Slowing " + gameObject.name);
    }
    
    public virtual void StopSlow()
    {
        currentSpeedRatio = 1f;
        Debug.Log("Stopping slow on " + gameObject.name);
    }

    public virtual void Slide()
    {
        Debug.Log("Sliding " + gameObject.name);
    }

    public virtual void Electrocute()
    {
        Debug.Log("Electrocuted " + gameObject.name);
        Die();
    }

    public bool ignitable;
    public virtual void Ignite()
    {
        if(ignitable) return;
        Debug.Log("Ignited " + gameObject.name);
        Die();
    }

    public virtual void Stomp()
    {
        Debug.Log("Stomped " + gameObject.name);
    }

    public virtual void Explode()
    {
        Debug.Log("Exploded " + gameObject.name);
        Die();
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
        Die();
    }

    public virtual void Poison()
    {
        Debug.Log("Poisoned " + gameObject.name);
    }

    public virtual void Dissolve()
    {
        Debug.Log("Dissolved " + gameObject.name);
        Die();
    }

    public void Grind()
    {
        Debug.Log("Grinded " + gameObject.name);
    }

    public virtual void Die()
    {
        Debug.Log(gameObject.name + " died");
    }

    Puddle puddle = null;
    private void OnCollisionEnter(Collision collision)
    {
        puddle = collision.gameObject.GetComponent<Puddle>();
        if (puddle != null)
        {
            if (puddle.sticky)
            {
                Slow();
            }

            if (puddle.acid)
            {
                Dissolve();
            }

            if (puddle.slippy)
            {
                Slide();
            }
            
            if(puddle.ignited)
            {
                Ignite();
            }

            puddle = null;
        }
    }
}
