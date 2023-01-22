using System.Collections;
using UnityEngine;

public class Television : AttractiveItem, ISmashable, IInteractable
{
    [Header("Television Materials")]
    private Material[] tempMatList;
    [SerializeField] private MeshRenderer tvScreenMR;
    [SerializeField] private Material tvOff;
    [SerializeField] private Material tvOn;
    [SerializeField] AudioClip tvOnSound, tvOffSound, footballSound, tvBrokenSound;
    [SerializeField] AudioSource tvAudioSource;

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

    public void Smash()
    {
        if (scrapPilePrefab != null && scrapPilePivot != null)
            Instantiate(scrapPilePrefab, scrapPilePivot.position, Quaternion.identity);
        else Debug.Log("No scrapPilePrefab or scrapPilePivot on " + this.name);
        functioning = false;
        StopCoroutine(PartyLoop());
        tvAudioSource.Stop();
        isPartying = false;
        tvAudioSource.PlayOneShot(tvBrokenSound);

        tempMatList = tvScreenMR.materials;
        tempMatList[1] = tvOff;
        tvScreenMR.materials = tempMatList;
        StopAttracted();
        if (charged) return;
        Electrocute();
        GetComponent<MeshRenderer>().enabled = false;
        GetComponent<Collider>().enabled = false;
    }

    [ContextMenu("Switch")]
    public override void Switch()
    {
        functioning = !functioning;
        tempMatList = tvScreenMR.materials;
        if (!functioning)
        {
            StopCoroutine(PartyLoop());
            tvAudioSource.Stop();
            isPartying = false;
            tvAudioSource.PlayOneShot(tvOffSound);

            tempMatList[1] = tvOff;
            StopAttracted();
        }
        else
        {
            tvAudioSource.PlayOneShot(tvOnSound);
            tempMatList[1] = tvOn;
        }
        tvScreenMR.materials = tempMatList;
    }
    
    public void Interact(Vector3 sourcePos)
    {
        if (interactParticle)
            interactParticle.Play();
        else Debug.Log("No interact Particle on "+this.name);
        Debug.Log("Interacting with " + gameObject.name);
        Switch();
    }

    IEnumerator PartyLoop()
    {
        isPartying = true;
        tvAudioSource.loop = true;
        tvAudioSource.clip = footballSound;
        tvAudioSource.Play();
        yield return new WaitForSeconds(16f);
        isPartying = false;
    }

}
