using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerState : MonoBehaviour
{
    [SerializeField] protected PlayerStateManager playerStateManager;
    [SerializeField] protected Rigidbody rb;
    [SerializeField] protected PlayerStats playerStats;
    [SerializeField] protected InterestPointsManager interestPointsManager;
    private float accelerationIndex;
    public abstract void Behave();
    public abstract void FixedBehave();

    public void Move()
    {
        if (InputManager.instance.input.Movement.Move.ReadValue<Vector2>() == Vector2.zero)
        {
            Deccelerate();
            return;
        }
        accelerationIndex = Mathf.Clamp(accelerationIndex + playerStats.accelerationStep, 0, 1);
        transform.LookAt(transform.position + new Vector3(InputManager.instance.input.Movement.Move.ReadValue<Vector2>().x, 0, InputManager.instance.input.Movement.Move.ReadValue<Vector2>().y).normalized);
        rb.velocity = new Vector3(InputManager.instance.input.Movement.Move.ReadValue<Vector2>().x,0,InputManager.instance.input.Movement.Move.ReadValue<Vector2>().y).normalized * (playerStats.accelerationCurve.Evaluate(accelerationIndex) * playerStats.maxSpeed);
    }
    public void Deccelerate()
    {
        accelerationIndex = Mathf.Clamp(accelerationIndex - playerStats.slowdownStep, 0, 1);
        rb.velocity = transform.forward * (playerStats.slowdownCurve.Evaluate(Mathf.Lerp(1,0,accelerationIndex)) * playerStats.maxSpeed);
    }
    private float tempAngle;
    public void LookForInterestPoints(float angle, float range, float step)
    {
        tempAngle = transform.rotation.eulerAngles.y/180f*Mathf.PI;
        for (float i = 0; i < angle; i += step)
        {
            SendRayCast(transform.position,new Vector3(Mathf.Sin(i+tempAngle),0,Mathf.Cos(i+tempAngle)), range, i/angle);
            SendRayCast(transform.position,new Vector3(Mathf.Sin(-i+tempAngle),0,Mathf.Cos(-i+tempAngle)), range, i/angle);
        }
    }

    protected abstract void SendRayCast(Vector3 origin, Vector3 dir, float length, float centerDistance);
}
