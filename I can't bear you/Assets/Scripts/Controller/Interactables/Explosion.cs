using DG.Tweening;
using UnityEngine;

public  class Explosion : MonoBehaviour
{

    public float explosionRadius;
    [SerializeField] private GameObject npcPoolOfBloodPrefab;
    [SerializeField] private GameObject ExplosionVFX;
    
    public void OnEnable()
    {
        explosionRadius = GetComponentInParent<SphereCollider>().radius;
        ExplosionVFX = Instantiate(ExplosionVFX, transform.position, Quaternion.identity);
        Camera.main.transform.DOShakePosition(0.2f, 0.5f, 10, 90, false, true);
        transform.localScale = Vector3.zero;
        ExplosionVFX.transform.localScale = Vector3.one * explosionRadius;
        transform.DOScale(explosionRadius,0.5f).OnComplete(() => gameObject.SetActive(false));
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Entity entity))
        {
            entity.Explode();
            Instantiate(npcPoolOfBloodPrefab, entity.transform.position, Quaternion.identity);
        }

        if (other.TryGetComponent(out IAffectable affectable))
        {
            affectable.Explode();
        }
    }
}
