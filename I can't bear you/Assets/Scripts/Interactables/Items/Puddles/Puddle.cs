using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Puddle : MonoBehaviour
{
    protected abstract void ApplyEffects(GameObject go);

    protected void OnTriggerEnter(Collider other)
    {
        ApplyEffects(other.gameObject);
        Debug.Log("Applying effects to " + other.gameObject.name);
    }
}
