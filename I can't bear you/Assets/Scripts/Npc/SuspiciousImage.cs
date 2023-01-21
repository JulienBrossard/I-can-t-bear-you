using UnityEngine;

public class SuspiciousImage : PanicImage
{
    [SerializeField] private GameObject panicImage;
    
    private void OnEnable()
    {
        if (states.activeSelf)
        {
            states.SetActive(false);
        }
    }
    
    private void OnDisable()
    {
        if (!states.activeSelf && !panicImage.activeSelf)
        {
            states.SetActive(true);
        }
    }
}
