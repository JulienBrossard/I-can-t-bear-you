using System.Collections;
using UnityEngine;

public class StealthState : PlayerState
{
    [SerializeField] private PlayerState bearserkerState;
    public override void OnStateEnter()
    {
        currentSusState = SUSSTATE.NORMAL;
        PlayerAnimatorManager.instance.SetAnimatorBool("Bearserker", false);
        if (heldObject != default)
        {
            heldObject.GetComponent<IGrabbable>().Drop();
        }
    }
    public override void Behave()
    {
        if (locked) return;
        if (InputManager.instance.input.Actions.Interact.triggered)
        {
            if (heldObject != default)
            {
                if (heldObject.GetComponent<IInteractable>() != default)
                {
                    heldObject.GetComponent<IInteractable>().Interact((transform.position - heldObject.transform.position).normalized);
                    return;
                }
            }
            if(TryGrab()) return;
            if(interestPointsManager.GetFirstItem()?.GetComponent<IInteractable>() != null)
            {
                interestPointsManager.GetFirstItem().GetComponent<IInteractable>().Interact((transform.position - interestPointsManager.GetFirstItem().transform.position).normalized);
                return;
            }
        }
        if (InputManager.instance.input.Actions.Smash.triggered)
        {
            if (heldObject != default)
            {
                StartCoroutine(EvaluateThrowForce());
                heldObjectGrabbable = heldObject.GetComponent<IGrabbable>();
                return;
            }
            interestPointsManager.GetSmashable()?.Smash();
            animator.SetTrigger("Attack");
        }

        if (InputManager.instance.input.Actions.Smash.IsPressed()) 
        { 
            if (heldObject != default) 
            { 
                heldObjectGrabbable.DrawProjection(); 
            } 
        }
        
        if (InputManager.instance.input.Actions.Roar.triggered)
        {
            bearserkerElement.SetActive(true);
            Roar();
            playerStateManager.SwitchState(bearserkerState);
        }
    }

  

    public override void FixedBehave()
    {
        if (!locked)
        {
            Move();
            PlayerAnimatorManager.instance.SetAnimatorFloat("Speed", rb.velocity.magnitude);
        }
    }
}
