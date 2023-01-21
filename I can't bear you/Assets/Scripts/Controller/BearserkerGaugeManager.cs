using UnityEngine;

public class BearserkerGaugeManager : MonoBehaviour
{
    public static BearserkerGaugeManager instance;
    [SerializeField] private BearserkerState bearserkerState;
    [SerializeField,Range(0f,1f)] private float amount;
    [SerializeField] ParticleSystem rageGain;
    private bool cantWinPointAnymore;
    [SerializeField] private PlayerStats playerStats;
    
    private void Awake()
    {
        instance = this;
    }

    public void KillNpc()
    {
        AddBearserker(playerStats.bearserkerToAddPerKill, false);
    }

    public void AddBearserker(float amountToAdd,bool isDamage)
    {
        if (cantWinPointAnymore) return;
        rageGain.Play();
        amount += amountToAdd;
        amount = Mathf.Clamp(amount, 0, 1);
        UiManager.instance.UpdateBearserkerGauge(amount);
        if (amount == 0)
        {
            bearserkerState.Sleep();
            cantWinPointAnymore = true;
        }

        if (amount < 0.15f)
            CameraManager.instance.StartLowBearserker();
        if (amount > 0.15f)
            CameraManager.instance.StopLowBearserker();
        
        if (isDamage)
            CameraManager.instance.CameraVignetteEffectOnHurt();
        
    }

    public void Use()
    {
        AddBearserker(-bearserkerState.playerStats.bearserkerDeductionRate,false);
    }
}
