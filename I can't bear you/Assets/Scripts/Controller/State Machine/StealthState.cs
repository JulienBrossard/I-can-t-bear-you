using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

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

            if (interestPointsManager.GetSmashable() != default)
            {
                interestPointsManager.GetSmashable().Smash();
                SetInvincibility();
            }
            animator.SetTrigger("Attack");
        }

        if (InputManager.instance.input.Actions.Roar.triggered)
        {
            if (SceneManager.GetActiveScene().name == "WorldMap") return;
            Roar();
            playerStateManager.SwitchState(bearserkerState);
        }

        if (InputManager.instance.input.Actions.Drop.triggered)
        {
            if (heldObject != default)
            {
                heldObject.GetComponent<IGrabbable>().Drop();
                UiManager.instance.DisableGrabbedItemPreview();
                heldObject = default;
            }
        }
    }

  

    public override void FixedBehave()
    {
        if (!locked)
        {
            Move();
            PlayerAnimatorManager.instance.SetAnimatorFloat("Speed", rb.velocity.magnitude);
        }
        else
        {
            rb.velocity = Vector3.zero;
        }
    }
}
