using UnityEngine;

public class BearserkerState : PlayerState
{
    [SerializeField] private PlayerState stealthState;
    public override void OnStateEnter()
    {
        currentSusState = SUSSTATE.FREIGHTNED;
        StartCoroutine(RoarCd());
        PlayerAnimatorManager.instance.SetAnimatorBool("Bearserker", true);
        Roar();
        roarReady = false;
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
            //Dégueulasse mais c'est le seul moyen que j'ai trouvé pour avoir la position de la cible sans tout revoir
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
                heldObject.GetComponent<IGrabbable>().Throw(transform.forward);
                heldObject = null;
                return;
            }
            interestPointsManager.GetSmashable()?.Smash();
            animator.SetTrigger("Attack");
        }
            /*if (InputManager.instance.input.Actions.Grab.triggered)
            {
                if (heldObject == default)
                {
                    if(interestPointsManager.GetGrabbable() == null) return;
                    heldObject = interestPointsManager.GetGrabbable()?.Grab(handTransform).gameObject;
                }
                else
                {
                    heldObject.GetComponent<IGrabbable>().Drop();
                    heldObject = null;
                }
            }*/
        if ((InputManager.instance.input.Actions.Roar.triggered) && (roarReady))
        {
            animator.SetTrigger("roarInBerserk");
            Roar();
            Debug.Log("has roared in berserk state");
        }
    }
    public override void FixedBehave()
    {
        if (!locked)
        {
            Move();
            PlayerAnimatorManager.instance.SetAnimatorFloat("Speed", rb.velocity.magnitude);
            BearserkerGaugeManager.instance.Use();
        }
    }
    public void Sleep()
    {
        Debug.Log("End lvl by sleeping");
        if (heldObject != default)
        {
            heldObject.GetComponent<IGrabbable>().Drop();
        }
        LevelManager.instance.EndLevel(true);
    }
}