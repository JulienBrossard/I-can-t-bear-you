using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Skull : MonoBehaviour
{
    private void OnEnable()
    {
        transform.DOMoveY(transform.position.y+4,0.6f).OnComplete(() => transform.DOMoveY(transform.position.y-2,0.3f));
    }
}
