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
        if (!locked)
        {
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
                //Dégueulasse mais c'est le seul moyen que j'ai trouvé pour avoir la position de la cible sans tout revoir
                foreach (InterestPoint interestPoint in interestPointsManager.interestPoints) 
                {
                    if(interestPoint.go.GetComponent<IInteractable>() != null)
                    {
                        interestPoint.go.GetComponent<IInteractable>().Interact((transform.position - interestPoint.go.transform.position).normalized);
                    }
                }
            }
            if (InputManager.instance.input.Actions.Smash.triggered)
            {
                if (heldObject != default)
                {
                    heldObject.GetComponent<IGrabbable>().Throw(transform.forward);
                    heldObject = null;
                    return;
                }
                interestPointsManager.GetSmashable()?.Smash();
                animator.SetTrigger("Attack");
            }
            if (InputManager.instance.input.Actions.Grab.triggered)
            {
                if (heldObject == default)
                {
                    if(interestPointsManager.GetGrabbable() == null) return;
                    if(interestPointsManager.GetGrabbable().Grab(handTransform) == default) return;
                    heldObject = interestPointsManager.GetGrabbable().Grab(handTransform).gameObject;
                }
                else
                {
                    heldObject.GetComponent<IGrabbable>().Drop();
                    heldObject = null;
                }
            }
            if (InputManager.instance.input.Actions.Roar.triggered)
            {
                Roar();
                playerStateManager.SwitchState(bearserkerState);
            }
        }
       
    }

  

    public override void FixedBehave()
    {
        if (!locked)
        {
            Move();
            PlayerAnimatorManager.instance.SetAnimatorFloat("Speed", rb.velocity.magnitude);
            LookForInterestPoints(playerStats.detectionAngle,playerStats.detectionRange,playerStats.detectionHeight,playerStats.detectionStep);
        }
       
    }

    protected override void SendRayCast(Vector3 origin, Vector3 dir, float length, float centerDistance)
    {
        RaycastHit hit;
        if (Physics.Raycast(origin, dir, out hit, length))
        {
            if (hit.collider.GetComponent<IInteractable>() != default || hit.collider.GetComponent<ISmashable>() != default || hit.collider.GetComponent<IGrabbable>() != default)
            {
                interestPointsManager.AddInterestPoint(new InterestPoint(hit.collider.gameObject, Mathf.InverseLerp(0,length,hit.distance),Mathf.Lerp(1,0,centerDistance),playerStats.detectionRangeCurve,playerStats.detectionAngleCurve));
                Debug.DrawRay(origin, dir * hit.distance, Color.blue);
                return;
            }
            Debug.DrawRay(origin, dir * length, Color.green);
            return;
        }
        Debug.DrawRay(origin, dir * length, Color.green);
        return;
    }
}
