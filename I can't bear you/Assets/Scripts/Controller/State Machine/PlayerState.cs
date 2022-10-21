using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerState : MonoBehaviour
{
    [SerializeField] protected PlayerStateManager playerStateManager;
    [SerializeField] protected Rigidbody rb;
    [SerializeField] protected PlayerStats playerStats;
    [SerializeField] private InteractablesManager interactablesManager;
    [SerializeField, Range(0.01f,Mathf.PI)] private float detectionAngle;
    [SerializeField] private float detectionRange;
    [SerializeField, Range(0.01f,0.5f)] private float detectionStep;
    private float accelerationIndex;
    public abstract void Behave();
    public abstract void FixedBehave();

    public void Move()
    {
        if (InputManager.instance.input.Movement.Move.ReadValue<Vector2>() != Vector2.zero)
        {
            accelerationIndex += playerStats.accelerationStep;
            transform.LookAt(transform.position + new Vector3(InputManager.instance.input.Movement.Move.ReadValue<Vector2>().x, 0, InputManager.instance.input.Movement.Move.ReadValue<Vector2>().y));
        }
        else
        {
            accelerationIndex -= playerStats.accelerationStep * 1.35f;
        }
        accelerationIndex = Mathf.Clamp(accelerationIndex, 0, 1);
        rb.velocity = new Vector3(InputManager.instance.input.Movement.Move.ReadValue<Vector2>().x,0,InputManager.instance.input.Movement.Move.ReadValue<Vector2>().y) * (playerStats.accelerationCurve.Evaluate(accelerationIndex) * playerStats.maxSpeed);
    }
    private float tempAngle;
    public void LookForInteractables()
    {
        for (float i = 0; i < detectionAngle; i += detectionStep)
        {
            tempAngle = transform.rotation.eulerAngles.y/180f*Mathf.PI;
            SendRayCast(transform.position,new Vector3(Mathf.Sin(i+tempAngle),0,Mathf.Cos(i+tempAngle)), detectionRange);
            SendRayCast(transform.position,new Vector3(Mathf.Sin(-i+tempAngle),0,Mathf.Cos(-i+tempAngle)), detectionRange);
        }
    }

    private bool SendRayCast(Vector3 origin, Vector3 dir, float length)
    {
        RaycastHit hit;
        if (Physics.Raycast(origin, dir, out hit, length))
        {
            if (hit.collider.gameObject.GetComponent<IInteractable>() != default)
            {
                //hit.collider.gameObject.GetComponent<IInteractable>().Interact();
                Debug.DrawRay(origin, dir * hit.distance, Color.red);
                return true;
            }
            Debug.DrawRay(origin, dir * length, Color.green);
            return false;
        }
        Debug.DrawRay(origin, dir * length, Color.green);
        return false;
    }
}
