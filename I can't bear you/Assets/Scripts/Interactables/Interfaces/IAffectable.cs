using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAffectable
{
    public bool charged { get; set; }
    public bool conductor { get; set; }
    public void Electrocute();
    public void Electrocute(GameObject emitter);
    public void Stomp();
    public void Explode();
}
