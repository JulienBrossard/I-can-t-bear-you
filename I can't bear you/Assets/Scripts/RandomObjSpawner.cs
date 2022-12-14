using UnityEngine;

public class RandomObjSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] objects;
    private int randomIndex;

    private void Start()
    {
        SpawnObject();
    }

    private void SpawnObject()
    {
        randomIndex = Random.Range(0, objects.Length);
        Instantiate(objects[randomIndex], transform.position, Quaternion.identity);
    }
}
