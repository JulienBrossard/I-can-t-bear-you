using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricityZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<IAffectable>() != default){
            other.GetComponent<IAffectable>().Electrocute();
        }
    }
}
