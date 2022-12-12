using System;
using System.Collections;
using UnityEngine;

public class Gas : MonoBehaviour, IAffectable
{
    [SerializeField] private float timeBeforeFading = 10f;
    [SerializeField] private GameObject explosionPrefab;
    [SerializeField] private bool isExplosive = true;

    public bool charged { get; set; }
    public bool conductor { get; set; }
    public bool explosive { get => isExplosive; set => isExplosive = value; }

    private void OnEnable()
    {
        StartCoroutine(Fade());
    }

    public void Electrocute()
    {
    }

    public void Electrocute(GameObject emitter)
    {
    }

    public void Stomp()
    {
        return;
    }

    public void Explode()
    {
        Instantiate(explosionPrefab, transform.position, Quaternion.identity, transform);
        StopAllCoroutines();
        gameObject.SetActive(false);
    }
    
    
    IEnumerator Fade()
    {
        yield return new WaitForSeconds(timeBeforeFading);
        gameObject.SetActive(false);
    }
}