using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StompZone : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(DeleteCoroutine());
    }

    private void OnTriggerEnter(Collider other)
    {
        other.GetComponent<IAffectable>()?.Stomp();
    }

    IEnumerator DeleteCoroutine()
    {
        yield return new WaitForSeconds(0.1f);
        Destroy(gameObject);
    }
}
