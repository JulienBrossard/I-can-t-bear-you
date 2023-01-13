using System.Collections;
using UnityEngine;

public class AttractiveItem : Item, IInteractable, ISmashable
{
    [Header("Television Attraction")] 
    [SerializeField] public Awareness awareness;
    public bool functioning = false;
    [HideInInspector] public bool isPartying = false;
    public float attractedDistance = 5f;
    public int npcCount;
    [Range(0,180)]
    [SerializeField] public float angle;
    [SerializeField] public bool invertZAxis;
    [SerializeField] AudioClip turnedOnSound, turnedOffSound, activeSound, brokenSound;
    [SerializeField] AudioSource audioSource;
    public ParticleSystem interactParticle;
    public GameObject scrapPilePrefab;
    public Transform scrapPilePivot;

    private void Update()
    {
        if (functioning)
        {
            Attracted();
            if (!isPartying)
            {
                StartCoroutine(PartyLoop());
            }
        }
    }
    
    [ContextMenu("Switch")]
    public virtual void Switch()
    {
        functioning = !functioning;
        if (!functioning)
        {
            StopCoroutine(PartyLoop());
            audioSource.Stop();
            isPartying = false;
            audioSource.PlayOneShot(turnedOffSound);

            StopAttracted();
        }
        else
        {
            audioSource.PlayOneShot(turnedOnSound);
        }
    }
    
    public virtual void Attracted()
    {
        if (npcCount != awareness.visibleTargets.Count)
        {
            for (int i = 0; i < awareness.visibleTargets.Count; i++)
            {
                Npc npc = awareness.visibleTargets[i].GetComponent<Npc>();
                if (npc.state == Npc.STATE.PARTY)
                {
                    if (!invertZAxis)
                    {
                        npc.Attracted(attractedDistance * - Mathf.Sign(Vector3.Dot(transform.forward, Vector3.forward)), transform.position, angle);
                    }
                    else
                    {
                        npc.Attracted(-attractedDistance * - Mathf.Sign(Vector3.Dot(transform.forward, Vector3.forward)), transform.position, angle);
                    }
                }
            }

            npcCount = awareness.visibleTargets.Count;
        }
    }
    
    public virtual void StopAttracted()
    {
        for (int i = 0; i < awareness.visibleTargets.Count; i++)
        {
            awareness.visibleTargets[i].GetComponent<Npc>().StopAttracted();
        }

        npcCount = 0;
    }
    
    public Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal) {
        if (!angleIsGlobal) {
            angleInDegrees += transform.eulerAngles.y;
        }
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad),0,Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }

    public void Smash()
    {
        if (scrapPilePrefab != null && scrapPilePivot != null)
            Instantiate(scrapPilePrefab, scrapPilePivot.position, Quaternion.identity);
        else Debug.Log("No scrapPilePrefab or scrapPilePivot on " + this.name);
        functioning = false;
        StopCoroutine(PartyLoop());
        audioSource.Stop();
        isPartying = false;
        audioSource.PlayOneShot(brokenSound);
        if (charged) return;
        Electrocute();
    }

    public void Interact(Vector3 sourcePos)
    {
        if (interactParticle)
            interactParticle.Play();
        else Debug.Log("No interact Particle on " + this.name);
        Debug.Log("Interacting with " + gameObject.name);
        Switch();
    }

    IEnumerator PartyLoop()
    {
        isPartying = true;
        audioSource.loop = true;
        audioSource.clip = activeSound;
        audioSource.Play();
        yield return new WaitForSeconds(16f);
        isPartying = false;
    }
}
