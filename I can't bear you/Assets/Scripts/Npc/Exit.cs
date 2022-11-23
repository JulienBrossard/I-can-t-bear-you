using UnityEngine;

public class Exit : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Danger")
        {
            LevelManager.instance.RemoveExitPoint(transform);
        }
    }
}
