using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAffectable
{
    [SerializeField] public bool charged { get; set; }
    [SerializeField] public bool conductor { get; set; }
    [SerializeField] public bool explosive { get; set; }
    public void Electrocute();
    public void Electrocute(GameObject emitter);
    public void Stomp();
    public void Explode();
}
