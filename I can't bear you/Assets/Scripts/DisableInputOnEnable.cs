using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableInputOnEnable : MonoBehaviour
{
    private void OnEnable()
    {
        InputManager.instance.input.Disable();
    }
}
