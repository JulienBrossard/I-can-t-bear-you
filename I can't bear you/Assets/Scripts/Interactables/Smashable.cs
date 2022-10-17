using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Smashable : MonoBehaviour
{
    public void Smash()
    {
        Debug.Log("Smashing " + gameObject.name);
    }
}
