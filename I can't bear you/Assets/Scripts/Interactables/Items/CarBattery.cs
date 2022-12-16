using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarBattery : Item,ISmashable
{
    
    public void Smash()
    {
        Break();
    }
    public override void OnHitGround(Collision collision)
    {
        if (thrown) Break();
        base.OnHitGround(collision);
    }
    private void Break()
    {
        Debug.Log("Breaking Car Battery");
        CreatePuddle().GetComponent<Puddle>().Electrocute();
        DeleteItem();
    }
}
