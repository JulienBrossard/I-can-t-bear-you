using UnityEngine;

public class BearserkerState : PlayerState
{
    [SerializeField] private PlayerState stealthState;
    [SerializeField] private int bearserkerMaxDuration;
    [SerializeField] private float bearserkerDurationRemaining;
    public override void Behave()
    {
        if (inputManager.interactDown)
        {
            //Tapotage très fort sur le front
        }
        if (inputManager.roarDown)
        {
            Debug.Log("Switching to Stealth");
            playerStateManager.SwitchState(stealthState);
        }
    }

    public override void FixedBehave()
    {
        Move();
    }
    
    public void AddBearserkerDuration(int duration)
    {
        bearserkerDurationRemaining += duration;
        if (bearserkerDurationRemaining > bearserkerMaxDuration)
        {
            bearserkerDurationRemaining = bearserkerMaxDuration;
        }
        UiManager.instance.UpdateBearserkerGauge(duration/bearserkerMaxDuration);
    }
    
    public void RemoveBearserkerDuration()
    {
        bearserkerDurationRemaining -= Time.deltaTime;
        if (bearserkerDurationRemaining < 0)
        {
            bearserkerDurationRemaining = 0;
            Sleep();
        }
        UiManager.instance.UpdateBearserkerGauge(bearserkerDurationRemaining/bearserkerMaxDuration);
    }

    public void Sleep()
    {
        //insert Sleep consequence
    }
}