using UnityEngine;

public class BearserkerGaugeManager : MonoBehaviour
{
    public static BearserkerGaugeManager instance;
    [SerializeField] private BearserkerState bearserkerState;
    [SerializeField,Range(0f,1f)] private float amount,deductionRate;
    [SerializeField] ParticleSystem rageGain;
    
    private void Awake()
    {
        instance = this;
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
        AddBearserker(-deductionRate,false);
    }
}
