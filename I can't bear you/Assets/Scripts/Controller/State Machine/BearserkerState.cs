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
        if (!locked)
        {
            if (InputManager.instance.input.Actions.Smash.triggered)
            {
                if (heldObject != default)
                {
                    heldObject.GetComponent<IGrabbable>().Throw(transform.forward);
                    heldObject = null;
                    return;
                }
                if (interestPointsManager.GetSmashable() != default)
                {
                    interestPointsManager.GetSmashable().Smash();
                    PlayerAnimatorManager.instance.SetAnimatorTrigger("Attack");
                }
            }
            if (InputManager.instance.input.Actions.Grab.triggered)
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
            }
            if ((InputManager.instance.input.Actions.Roar.triggered) && (roarReady))
            {
                animator.SetTrigger("roarInBerserk");
                Roar();
                Debug.Log("has roared in berserk state");
            }
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