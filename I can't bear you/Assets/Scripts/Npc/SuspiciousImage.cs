using UnityEngine;

public class SuspiciousImage : PanicImage
{
    [SerializeField] private GameObject panicImage;
    
    private void OnEnable()
    {
        onEnable.Invoke();
    }
    
    private void OnDisable()
    {
        if (!panicImage.transform.parent.gameObject.activeSelf)
        {
            onDisable.Invoke();
        }
    }
}
