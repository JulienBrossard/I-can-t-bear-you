using System.Collections;
using UnityEngine;

public class BearserkerState : PlayerState
{
    [SerializeField] private PlayerState stealthState;
    [SerializeField] private int bearserkerMaxDuration;
    [SerializeField] private float bearserkerDurationRemaining;
    protected override void OnStateEnter()
    {
        StartCoroutine(RemoveBearserkerDuration());
    }

    public override void Behave()
    {
        if (InputManager.instance.input.Actions.Smash.triggered)
        {
            interestPointsManager.GetSmashable()?.Smash();
        }
        if (InputManager.instance.input.Actions.Grab.triggered)
        {
            if (heldObject == default)
            {
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
            //Temporaire pour le debug
            Debug.Log("Switching to Bearserker");
            playerStateManager.SwitchState(stealthState);
        }
    }
    
    public override void FixedBehave()
    {
        Move();
        LookForInterestPoints(playerStats.detectionAngle,playerStats.detectionRange,playerStats.detectionStep);
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
    
    public void AddBearserkerDuration(int duration)
    {
        bearserkerDurationRemaining += duration;
        if (bearserkerDurationRemaining > bearserkerMaxDuration)
        {
            bearserkerDurationRemaining = bearserkerMaxDuration;
        }
        UiManager.instance.UpdateBearserkerGauge(duration/bearserkerMaxDuration, 0.2f);
    }
    
    IEnumerator RemoveBearserkerDuration()
    {
        bearserkerDurationRemaining -= 1;
        UiManager.instance.UpdateBearserkerGauge(bearserkerDurationRemaining/bearserkerMaxDuration, 1f);

        
        if (bearserkerDurationRemaining < 0)
        {
            bearserkerDurationRemaining = 0;
            Sleep();
        }

        if (bearserkerDurationRemaining > 0)
        {
            yield return new WaitForSeconds(1);
            StartCoroutine(RemoveBearserkerDuration());
        }
    }
    

    public void Sleep()
    {
        //insert Sleep consequence
    }
}