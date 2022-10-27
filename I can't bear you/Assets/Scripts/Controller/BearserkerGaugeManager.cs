using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BearserkerGaugeManager : MonoBehaviour
{
    public static BearserkerGaugeManager instance;
    [SerializeField] private BearserkerState bearserkerState;
    [SerializeField,Range(0f,1f)] private float amount,deductionRate;
    
    private void Awake()
    {
        instance = this;
    }

    public void AddBearserker(float amountToAdd)
    {
        amount += amountToAdd;
        amount = Mathf.Clamp(amount, 0, 1);
        //UiManager.instance.UpdateBearserkerGauge(amount); //En commentaire parce que y'a pas d'UIManager sur ma scène actuellement
        if(amount == 0) bearserkerState.Sleep();
    }

    public void Use()
    {
        AddBearserker(-deductionRate);
    }
}
