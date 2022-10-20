using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StealthState : PlayerState
{
    [SerializeField] private PlayerState bearserkerState;
    [SerializeField, Range(0.01f,Mathf.PI)] private float detectionAngle;
    [SerializeField] private float detectionRange;
    [SerializeField, Range(0.01f,0.5f)] private float detectionStep;
    public override void Behave()
    {
        if (inputManager.interactDown)
        {
            //Sabotage ou attrapage d'item
        }
        if (inputManager.roarDown)
        {
            Debug.Log("Switching to Bearserker");
            playerStateManager.SwitchState(bearserkerState);
        }
    }

    public override void FixedBehave()
    {
        Move();
        LookForInteractables();
    }
    
    public void LookForInteractables()
    {
        for (float i = 0; i < detectionAngle; i = i+detectionStep)
        {
            SendRayCast(transform.position, new Vector3(Mathf.Sin(i),0,Mathf.Cos(i)), detectionRange);
            SendRayCast(transform.position, new Vector3(Mathf.Sin(-i),0,Mathf.Cos(-i)), detectionRange);
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
    public static Vector3 GetVectorFromAngle(float angle) {
        float angleRad = angle * (Mathf.PI/180f);
        return new Vector3(Mathf.Cos(angleRad), Mathf.Sin(angleRad));
    }
}
