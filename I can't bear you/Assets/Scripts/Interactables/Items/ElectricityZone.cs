using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricityZone : MonoBehaviour
{
    public void SetSize(ZoneMode mode,float size)
    {
        switch (mode)
        {
            case ZoneMode.SPHERE:
                transform.localScale = Vector3.one * size;
                break;
            case ZoneMode.PLANE:
                transform.localScale = new Vector3(size, 1, size);
                break;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<IAffectable>() != default){
            other.GetComponent<IAffectable>().Electrocute(transform.parent.gameObject);
        }
    }
}
public enum ZoneMode
{
    SPHERE, PLANE
}
