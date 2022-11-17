using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Gas : MonoBehaviour
{
    protected abstract void ApplyEffects(GameObject go);

    protected void OnTriggerStay(Collider other)
    {
        ApplyEffects(other.gameObject);
        Debug.Log("Applying effects to " + other.gameObject.name);
    }
}
