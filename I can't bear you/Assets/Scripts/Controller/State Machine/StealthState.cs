using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StealthState : PlayerState
{
    [SerializeField] private PlayerState bearserkerState;
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
    }
    public void LookForInteractables()
    {
        Debug.Log(transform.forward);
        Debug.DrawRay(transform.position, transform.forward * 2, Color.red);
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, 2f))
        {
            /*if (hit.collider.gameObject.GetComponent<Interactable>())
            {
                hit.collider.gameObject.GetComponent<Interactable>().
            }*/
        }
    }
}
