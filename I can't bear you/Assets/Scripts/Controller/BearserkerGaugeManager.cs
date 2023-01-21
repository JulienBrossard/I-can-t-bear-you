using UnityEngine;

public class BearserkerGaugeManager : MonoBehaviour
{
    public static BearserkerGaugeManager instance;
    [SerializeField] private BearserkerState bearserkerState;
    [SerializeField,Range(0f,1f)] private float amount;
    [SerializeField] ParticleSystem rageGain;
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
        rageGain.Play();
        amount += amountToAdd;
        amount = Mathf.Clamp(amount, 0, 1);
        UiManager.instance.UpdateBearserkerGauge(amount);
        if(amount == 0) bearserkerState.Sleep();

        if (isDamage)
            CameraManager.instance.CameraVignetteEffectOnHurt();
    }

    public void Use()
    {
        AddBearserker(-bearserkerState.playerStats.bearserkerDeductionRate,false);
    }
}
