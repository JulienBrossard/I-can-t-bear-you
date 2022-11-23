using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricityZone : MonoBehaviour
{
    public void SetSize(float size)
    {
        transform.localScale = Vector3.one * size;
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<IAffectable>() != default){
            other.GetComponent<IAffectable>().Electrocute(transform.parent.gameObject);
        }
    }
}
