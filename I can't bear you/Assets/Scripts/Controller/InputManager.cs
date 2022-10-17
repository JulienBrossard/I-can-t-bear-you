using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public Vector2 direction;
    public bool interactDown;
    public bool roarDown;

    private void Update()
    {
        direction = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
        interactDown = Input.GetButtonDown("Fire1");
        roarDown = Input.GetButtonDown("Fire2");
    }
}
