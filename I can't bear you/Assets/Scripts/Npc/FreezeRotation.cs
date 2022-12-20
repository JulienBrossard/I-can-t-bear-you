using UnityEngine;

public class FreezeRotation : MonoBehaviour
{
    private Quaternion rotation;

    void Start()
    {
        rotation = transform.rotation;
    }

    void Update()
    {
        transform.rotation = rotation;
    }
}
