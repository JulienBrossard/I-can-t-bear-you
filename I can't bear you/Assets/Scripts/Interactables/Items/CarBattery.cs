using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarBattery : Item,ISmashable
{
    
    public void Smash()
    {
        Break();
    }
    public override void OnHitGround()
    {
        if (thrown) Break();
        base.OnHitGround();
    }
    private void Break()
    {
        Debug.Log("Breaking Car Battery");
        CreatePuddle().GetComponent<Puddle>().Electrocute();
        DeleteItem();
    }
}
