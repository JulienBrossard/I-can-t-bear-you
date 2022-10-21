using UnityEngine;

public class BearserkerState : PlayerState
{
    [SerializeField] private PlayerState stealthState;
    [SerializeField] private int bearserkerMaxDuration;
    [SerializeField] private float bearserkerDurationRemaining;
    public override void Behave()
    {
        if (InputManager.instance.input.Actions.Smash.triggered)
        {
            //Tapotage sur le front
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