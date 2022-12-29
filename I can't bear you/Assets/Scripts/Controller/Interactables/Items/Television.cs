using System.Collections;
using UnityEngine;

public class Television : AttractiveItem, ISmashable, IInteractable
{
    [Header("Television Materials")]
    private Material[] tempMatList;
    [SerializeField] private MeshRenderer tvScreenMR;
    [SerializeField] private Material tvOff;
    [SerializeField] private Material tvOn;
    bool isPartying = false;
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
        functioning = false;
        StopCoroutine(PartyLoop());
        tvAudioSource.Stop();
        isPartying = false;
        tvAudioSource.PlayOneShot(tvBrokenSound);

        tempMatList = tvScreenMR.materials;
        tempMatList[1] = tvOff;
        tvScreenMR.materials = tempMatList;
        if (charged) return;
        Electrocute();
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
