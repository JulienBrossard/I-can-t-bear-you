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

    private void FixedUpdate()
    {
    }
}
