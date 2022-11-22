using UnityEngine;

public class Entity : MonoBehaviour, IAffectable
{
    [HideInInspector] public float currentSpeedRatio = 1f;
    [Header("Entity Data")]
    public EntityData entityData;
    [Header("Animator")]
    public Animator animator;
    [HideInInspector] public bool isDie;
    
    [ContextMenu("Slow")]
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

    [ContextMenu("Electrocute")]
    public virtual void Electrocute()
    {
        if (!isDie)
        {
            animator.SetBool("isElectrocuted", true);
            Debug.Log("Electrocuted " + gameObject.name);
            Die();
        }
    }

    [HideInInspector] public bool ignitable;
    [ContextMenu("Ignite")]
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

    [ContextMenu("Explode")]
    public virtual void Explode()
    {
        Debug.Log("Exploded " + gameObject.name);
        Die();
    }

    [HideInInspector] public float asphyxiation;
    [HideInInspector] public float asphyxiationToDie;
    public virtual void Asphyxiate()
    {
        asphyxiation += 0.001f;
        if (asphyxiation > asphyxiationToDie)
        {
            DieFromAsphyxiation();
        }
    }
    [ContextMenu("Die From Asphyxiation")]
    public virtual void DieFromAsphyxiation()
    {
        
        Debug.Log("Asphyxiated " + gameObject.name);
        Die();
    }

    [ContextMenu("Poison")]
    public virtual void Poison()
    {
        if (!isDie)
        {
            animator.SetBool("isPoisoned", true);
            Debug.Log("Poisoned " + gameObject.name);
            Die();
        }
    }

    [ContextMenu("Dissolve")]
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
        isDie = true;
        Debug.Log(gameObject.name + " died");
    }

    Puddle puddle = null;
    /*private void OnCollisionEnter(Collision collision)
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
    }*/
}
