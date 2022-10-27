using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamSwitch : MonoBehaviour
{
    public void CallCamSwitch(Transform tr)
    {
        CameraManager.instance.MoveToRoom(tr);
    }
}
