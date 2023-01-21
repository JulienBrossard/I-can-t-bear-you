using UnityEngine;
using UnityEngine.Events;

public class PanicImage : MonoBehaviour
{
    [SerializeField] private GameObject canvas;
    public GameObject npc;
    public GameObject states;
    public GameObject suspicious;

    private void OnEnable()
    {
        if (states.activeSelf)
        {
            states.SetActive(false);
        }
        if (suspicious.activeSelf)
        {
            suspicious.SetActive(false);
        }
    }

    private void OnDisable()
    {
        if (!states.activeSelf)
        {
            states.SetActive(true);
        }
    }
}
