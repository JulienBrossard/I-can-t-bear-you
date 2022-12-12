using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.AI;

public class Door : MonoBehaviour, IInteractable
{
    private Vector3 initRotation;
    private bool isOpen;
    List<GameObject> npcIntTrigger = new List<GameObject>(); 
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
    
    /// <summary>
    /// Close the door
    /// </summary>
    void Close()
    {
        isOpen = false;
        transform.GetChild(0).transform.DORotate(initRotation, 2f);
        obstacle.enabled = false;
    }

    // Npc open the door
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Npc"))
        {
            npcIntTrigger.Add(other.gameObject);
            if (!isOpen)
            {
                Open(other.transform);
            }
        }
    }

    // Npc close the door
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Npc"))
        {
            npcIntTrigger.Remove(other.gameObject);
            if (npcIntTrigger.Count == 0)
            {
                Close();
            }
        }
    }

    // Player open or close the door
    [ContextMenu("Interact")]
    public void Interact(Vector3 direction)
    {
        if (npcIntTrigger.Count == 0)
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
