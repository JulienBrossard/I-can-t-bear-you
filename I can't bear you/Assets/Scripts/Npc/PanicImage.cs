using UnityEngine;
using UnityEngine.Events;

public class PanicImage : MonoBehaviour
{
    public UnityEvent onEnable;
    public UnityEvent onDisable;
    [SerializeField] private GameObject canvas;
    [SerializeField] private GameObject npc;
    
    private void OnEnable()
    {
        onEnable?.Invoke();
    }

    private void OnDisable()
    {
        if (canvas.activeSelf && npc.activeSelf)
        {
            onDisable?.Invoke();
        }
    }
}
