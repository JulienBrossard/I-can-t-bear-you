using DG.Tweening;
using UnityEngine;

public  class Explosion : MonoBehaviour
{

    public float explosionRadius;
    public GameObject npcPoolOfBloodPrefab;
    
    private void OnEnable()
    {
        Camera.main.transform.DOShakePosition(0.2f, 0.5f, 10, 90, false, true);
        transform.localScale = Vector3.zero;
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
