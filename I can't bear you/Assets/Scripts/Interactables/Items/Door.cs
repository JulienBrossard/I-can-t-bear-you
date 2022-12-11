using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.AI;

public class Door : MonoBehaviour, IInteractable
{
    private Vector3 initRotation;
    private bool isOpen;
    List<GameObject> entitiesInTrigger = new List<GameObject>(); 
    [SerializeField] NavMeshObstacle obstacle;

    void Start()
    {
        initRotation = transform.GetChild(0).transform.eulerAngles;
    }
    
    /// <summary>
    /// Open the door
    /// </summary>
    /// <param name="entity"> Entity (Npc or Player)  opening the door</param>
    void Open(Transform entity)
    {
        if (!isOpen)
        {
            float direction = Mathf.Sign(Vector3.Dot(transform.GetChild(0).transform.forward, entity.forward));
            transform.GetChild(0).transform.DORotate(new Vector3(0, initRotation.y - 90 * direction , 0), 2f);
            isOpen = true;
            obstacle.enabled = true;
        }
    }
    
    void Close()
    {
        isOpen = false;
        transform.GetChild(0).transform.DORotate(initRotation, 2f);
        obstacle.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Npc"))
        {
            entitiesInTrigger.Add(other.gameObject);
            if (!isOpen)
            {
                Open(other.transform);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Npc"))
        {
            entitiesInTrigger.Remove(other.gameObject);
            if (entitiesInTrigger.Count == 0)
            {
                Close();
            }
        }
    }

    [ContextMenu("Interact")]
    public void Interact()
    {
        if (entitiesInTrigger.Count == 0)
        {
            if (isOpen)
            {
                Close();
            }
            else
            {
                Open(LevelManager.instance.GetPlayer());
            }
        }
    }
}
