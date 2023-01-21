using DG.Tweening;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class CameraManager : MonoBehaviour
{
    public static CameraManager instance;
    [SerializeField] private Transform target; 
    private Vignette vignette;
    [SerializeField] private Volume volume;
    
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this);
        }
        instance = this;
        volume.profile.TryGet(out vignette);
    }
    
    public void MoveToRoom(Transform roomWayPoint)
    {
        transform.DOMove(roomWayPoint.transform.position, 1f);
    }

    public void CameraShake(float duration, Vector3 vector3, float strength, int vibrato, float randomness)
    {
        transform.DOShakePosition(duration, strength, vibrato, randomness);
    }

    public void CameraVignetteEffectOnHurt()
    {
        if (vignette.active == false)
        {
            vignette.active = true;
            DOTween.To(() => vignette.intensity.value, x => vignette.intensity.value = x, 0.5f, 0.5f).OnComplete(ResetVignette).SetLoops(2);
        }
    }
    
    public void StartLowBearserker()
    {
        vignette.active = true;
        DOTween.To(() => vignette.intensity.value, x => vignette.intensity.value = x, 0.3f, 0.5f).SetLoops(2);
    }
    
    public void StopLowBearserker()
    {
        DOTween.Kill("Vignette");
        ResetVignette();
    }

    private void ResetVignette()
    {
        vignette.intensity.value = 0;
        vignette.active = false;
    }

    private void FixedUpdate()
    {
    }
}
