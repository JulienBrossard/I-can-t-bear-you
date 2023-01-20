using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spin : MonoBehaviour
{
    [SerializeField] private float speed = 1f;
    void Rotate()
    {
        transform.Rotate(0, speed, 0);
    }
    
    void Update()
    {
        Rotate();
    }
}
