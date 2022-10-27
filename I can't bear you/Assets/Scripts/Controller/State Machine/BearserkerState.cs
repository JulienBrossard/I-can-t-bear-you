using UnityEngine;

public class BearserkerState : PlayerState
{
    [SerializeField] private PlayerState stealthState;
    public override void OnStateEnter()
    {
        Debug.Log(0);
        PlayerAnimatorManager.instance.SetAnimatorBool("Bearserker", true);
    }
    public override void Behave()
    {
        if (InputManager.instance.input.Actions.Smash.triggered)
        {
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
        if (InputManager.instance.input.Actions.Roar.triggered)
        {
            //Temporaire pour le debug
            Debug.Log("Switching to Stealth");
            heldObject?.GetComponent<IGrabbable>().Drop();
            playerStateManager.SwitchState(stealthState);
        }
    }
    public override void FixedBehave()
    {
        Move();
        PlayerAnimatorManager.instance.SetAnimatorFloat("Speed", rb.velocity.magnitude);
        LookForInterestPoints(playerStats.detectionAngle,playerStats.detectionRange,playerStats.detectionStep);
        BearserkerGaugeManager.instance.Use();
    }
    protected override void SendRayCast(Vector3 origin, Vector3 dir, float length, float centerDistance)
    {
        RaycastHit hit;
        if (Physics.Raycast(origin, dir, out hit, length))
        {
            if (hit.collider.GetComponent<ISmashable>() != default || hit.collider.GetComponent<IGrabbable>() != default)
            {
                interestPointsManager.AddInterestPoint(new InterestPoint(hit.collider.gameObject, hit.distance,centerDistance));
                Debug.DrawRay(origin, dir * hit.distance, Color.blue);
                return;
            }
            Debug.DrawRay(origin, dir * length, Color.green);
            return;
        }
        Debug.DrawRay(origin, dir * length, Color.green);
        return;
    }
    public void Sleep()
    {
        Debug.Log("Switching to Stealth");
        heldObject?.GetComponent<IGrabbable>().Drop();
        playerStateManager.SwitchState(stealthState); //Pour le debug
        //insert Sleep consequence
    }
}