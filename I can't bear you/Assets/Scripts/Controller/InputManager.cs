using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    public static InputManager instance;
    public PlayerInput input;

    private void Awake()
    {
        if(instance != null) Destroy(gameObject);
        instance = this;
        
        input = new PlayerInput();
        input.Enable();
    }
}
