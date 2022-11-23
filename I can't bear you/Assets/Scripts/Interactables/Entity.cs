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

    public bool charged { get; set; }
    public bool conductor { get; set; }

    public virtual void Electrocute() //Electrocute étant sa propre source électrique
    {
        Debug.Log("Electrocuted " + gameObject.name);
        Die();
    }

    public void Electrocute(GameObject emitter) //Electrocute dépendant d'un émetteur
    {
        Debug.Log("Electrocuted " + gameObject.name);
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

    public virtual void Die()
    {
        Debug.Log(gameObject.name + " died");
    }
}
