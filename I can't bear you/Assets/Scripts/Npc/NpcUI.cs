using UnityEngine;
using UnityEngine.UI;

public class NpcUI : MonoBehaviour
{
    [Header("Canvas")]
    public GameObject canvas;
    
    [Header("State Feedback")]
    public GameObject thirstImage;
    public GameObject hungerImage;
    public GameObject bladderImage;
    
    [Header("Panic Feedback")] 
    public Image suspiciousImage;
    public Image panicImage;
}
