using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGrabbable
{
    public Transform Grab(Transform hand);
    public void SetAsGrabbed(Transform hand);
    public void Throw(Vector3 dir);
    public void Drop();
    public void SetAsReleased();
    public void DrawProjection();
}
