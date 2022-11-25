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

    public bool charged { get; set; }
    public bool conductor { get; set; }

    public virtual void Electrocute()
    {
        if (!isDie)
        {
            //animator.SetBool("isElectrocuted", true);
            Debug.Log("Electrocuted " + gameObject.name);
            Die();
        }
    }

    public void Electrocute(GameObject emitter)
    {
        Debug.Log("Electrocuted " + gameObject.name);
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

    public virtual void Die()
    {
        NpcManager.instance.npcCountRemaining--;
        NpcManager.instance.CheckForLvlEnd();
        NpcManager.instance.npcCountkilled++;
        UiManager.instance.UpdateRemainingNpcText();
        isDie = true;
        Debug.Log(gameObject.name + " died");
    }
}