using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerState : MonoBehaviour
{
    [SerializeField] protected PlayerStateManager playerStateManager;
    [SerializeField] protected InputManager inputManager;
    [SerializeField] protected Rigidbody rb;
    [SerializeField] protected PlayerStats playerStats;
    [SerializeField] private InteractablesManager interactablesManager;
    private float accelerationIndex;
    public abstract void Behave();
    public abstract void FixedBehave();

    public void Move()
    {
        if (inputManager.direction != Vector2.zero)
        {
            accelerationIndex += playerStats.accelerationStep;
            transform.LookAt(transform.position + new Vector3(inputManager.direction.x, 0, inputManager.direction.y));
        }
        else
        {
            accelerationIndex -= playerStats.accelerationStep * 1.35f;
        }
        accelerationIndex = Mathf.Clamp(accelerationIndex, 0, 1);
        rb.velocity = new Vector3(inputManager.direction.x,0,inputManager.direction.y) * (playerStats.accelerationCurve.Evaluate(accelerationIndex) * playerStats.maxSpeed);
    }
}
