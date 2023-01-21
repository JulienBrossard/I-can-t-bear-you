using System;
using System.Collections;
using UnityEngine;

public class Gas : MonoBehaviour, IAffectable
{
    [SerializeField] private float timeBeforeFading = 10f;
    [SerializeField] private GameObject explosionPrefab;
    [SerializeField] private SphereCollider sphereCollider;
    private Explosion tempExplosion;
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
        Explode();
    }

    public void Electrocute(GameObject emitter)
    {
        Explode();
    }

    public void Stomp(Vector3 srcPos)
    {
        return;
    }

    public void Explode()
    {
        
        tempExplosion = Instantiate(explosionPrefab, transform.position, Quaternion.identity, transform).GetComponent<Explosion>();
        tempExplosion.Explode(sphereCollider.radius);
        tempExplosion.transform.SetParent(null);
        StopAllCoroutines();
        gameObject.SetActive(false);
    }
    
    
    IEnumerator Fade()
    {
        yield return new WaitForSeconds(timeBeforeFading);
        gameObject.SetActive(false);
    }
}
