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
            if (InputManager.instance.input.Actions.Roar.triggered)
            {
                Roar();
                Debug.Log("Switching to Stealth");
                playerStateManager.SwitchState(stealthState);
            }
        }
     
    }
    public override void FixedBehave()
    {
        if (!locked)
        {
            Move();
            PlayerAnimatorManager.instance.SetAnimatorFloat("Speed", rb.velocity.magnitude);
            LookForInterestPoints(playerStats.detectionAngle,playerStats.detectionRange,playerStats.detectionStep);
            BearserkerGaugeManager.instance.Use();
        }
    }
    protected override void SendRayCast(Vector3 origin, Vector3 dir, float length, float centerDistance)
    {
        RaycastHit hit;
        if (Physics.Raycast(origin, dir, out hit, length))
        {
            if (hit.collider.GetComponent<ISmashable>() != default || hit.collider.GetComponent<IGrabbable>() != default)
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
    public void Sleep()
    {
        Debug.Log("End lvl by sleeping");
        heldObject?.GetComponent<IGrabbable>().Drop();
        LevelManager.instance.EndLevel(true);
    }
}