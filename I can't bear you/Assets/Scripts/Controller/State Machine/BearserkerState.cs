using UnityEngine;

public class BearserkerState : PlayerState
{
    [SerializeField] private PlayerState stealthState;
    bool endState = false;
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
        if (InputManager.instance.input.Actions.Smash.triggered)
        {
            interestPointsManager.GetSmashable()?.Smash();
            animator.SetTrigger("Attack");
        }
        if (InputManager.instance.input.Actions.Roar.triggered && roarReady)
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
        else
            rb.velocity = Vector3.zero;
    }
    public void Sleep()
    {
        if (endState) return;
        
        Debug.Log("End lvl by sleeping");
        if (heldObject != default)
                heldObject.GetComponent<IGrabbable>().Drop();
        LevelManager.instance.EndLevel(true);
        endState = true;
        


    }
}