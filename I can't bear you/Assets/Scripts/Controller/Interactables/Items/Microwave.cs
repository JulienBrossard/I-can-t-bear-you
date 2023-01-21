using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Microwave : Item,ISmashable
{
    [SerializeField] AudioSource breakSource;
    [SerializeField] AudioClip breakSound;
    [SerializeField] GameObject scrapPilePrefab;
    [SerializeField] Transform scrapPilePivot;
    bool isBroken = false;
    public void Smash()
    {
        Break();
    }
    public override void OnHitGround(Collision collision)
    {
        if (thrown) Break();
        base.OnHitGround(collision);
    }
    public void Break()
    {
        if (!isBroken)
        {
            transform.position = new Vector3(transform.position.x,0.5f,transform.position.z);
            if (scrapPilePrefab != null && scrapPilePivot != null)
            {
                Instantiate(scrapPilePrefab, scrapPilePivot.position, Quaternion.identity);
                GetComponent<Rigidbody>().isKinematic = true;
            }
            else Debug.Log("No scrapPilePrefab or scrapPilePivot on " + this.name);
            Debug.Log("Breaking Microwave");
            breakSource.PlayOneShot(breakSound);
            Electrocute();
            isBroken = true;
            GetComponent<Collider>().enabled = false;
            transform.GetChild(0).gameObject.SetActive(false);
        }
    }
}
