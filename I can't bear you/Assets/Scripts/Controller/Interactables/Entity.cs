using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Entity : MonoBehaviour, IAffectable
{
    [SerializeField] private Tools.FIELD field = Tools.FIELD.HIDDEN;
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
    public bool explosive { get; set; }

    public virtual void Electrocute()
    {
        if (!isDie)
        {
            animator.SetBool("isElectrocuted", true);
            Debug.Log("Electrocuted " + gameObject.name);
            Die(false);
        }
    }

    public virtual void Electrocute(GameObject emitter)
    {
        if (!isDie)
        {
            animator.SetBool("isElectrocuted", true);
            Debug.Log("Electrocuted " + gameObject.name);
            Die(false);
        }
    }

    public virtual void Stomp(Vector3 srcPos)
    {
        Debug.Log("Stomped " + gameObject.name);
    }

    [ContextMenu("Explode")]
    public virtual void Explode()
    {
        Debug.Log("Exploded " + gameObject.name);
        Die(false);
        animator.SetTrigger("isExploding");
    }

    public virtual void Die(bool unspawn)
    {
        isDie = true;
        NpcManager.instance.npcCountRemaining--;
        NpcManager.instance.CheckForLvlEnd();
        NpcManager.instance.npcCountkilled++;
        UiManager.instance.UpdateRemainingNpcText();
        Debug.Log(gameObject.name + " died");  
    }
}