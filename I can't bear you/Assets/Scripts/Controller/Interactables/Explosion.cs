using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public  class Explosion : MonoBehaviour
{

    public float explosionRadius;
    [SerializeField] private GameObject npcPoolOfBloodPrefab;
    [SerializeField] private GameObject ExplosionVFX;
    [SerializeField] private SphereCollider sphereCollider;

    [SerializeField] private LayerMask obstacleLayerMask;

    public void Explode(float radius)
    {
        explosionRadius = radius;
        ExplosionVFX = Instantiate(ExplosionVFX, transform.position, Quaternion.identity);
        Camera.main.transform.DOShakePosition(0.2f, 0.5f, 10, 90, false, true);
        transform.localScale = Vector3.zero;
        ExplosionVFX.transform.localScale = Vector3.one * explosionRadius;
        transform.DOScale(explosionRadius,0.5f).OnComplete(() => gameObject.SetActive(false));
        sphereCollider.radius = explosionRadius;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (IsThereAWall(other.transform)) return;

        
        Debug.Log(other.gameObject.name);
        if (other.TryGetComponent(out Entity entity))
        {
            entity.Explode();
            Instantiate(npcPoolOfBloodPrefab, entity.transform.position, Quaternion.identity);
        }

        if (other.TryGetComponent(out IAffectable affectable) && !(other.gameObject.CompareTag("Npc") || other.gameObject.CompareTag("Player")))
        {
            affectable.Explode();
        }
    }

    private bool IsThereAWall(Transform objectToCheck)
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, objectToCheck.position - transform.position, out hit, Vector3.Distance(transform.position, objectToCheck.position), obstacleLayerMask))
        {
            return true;
        }
        return false;
    }
}
