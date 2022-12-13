using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightColorSwitch : MonoBehaviour
{
    private Light light;

    [SerializeField] private Gradient gradient;
    [SerializeField] private AnimationCurve curve;

    private void Start()
    {
        light = gameObject.GetComponent<Light>();
    }

    void FixedUpdate()
    {
        light.color = gradient.Evaluate(curve.Evaluate(Time.time));
    }
}
