using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class OnTriggerEvent : MonoBehaviour
{
    [SerializeField] private UnityEvent onTriggerEnter;
    [SerializeField] private UnityEvent onTriggerExit;

    [SerializeField] private bool checkForTag;
    [SerializeField] private string tagToCheckFor = "Player";
    
    private void OnTriggerEnter(Collider other)
    {
        if (checkForTag)
        {
            if (other.CompareTag(tagToCheckFor))
            {
                onTriggerEnter.Invoke();
            }
        }
        else
        {
            onTriggerEnter.Invoke();
        }
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (checkForTag)
        {
            if (other.CompareTag(tagToCheckFor))
            {
                onTriggerExit.Invoke();
            }
        }
        else
        {
            onTriggerExit.Invoke();
        }
    }

}
