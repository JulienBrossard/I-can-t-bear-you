using DG.Tweening;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager instance;
    [SerializeField] private Transform target;
    
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this);
        }
        instance = this;
    }
    
    public void MoveToRoom(Transform roomWayPoint)
    {
        transform.DOMove(roomWayPoint.transform.position, 1f);
    }

    public void CameraShake(float duration, Vector3 vector3, float strength, int vibrato, float randomness)
    {
        transform.DOShakePosition(duration, strength, vibrato, randomness);
    }

    private void FixedUpdate()
    {
    }
}
