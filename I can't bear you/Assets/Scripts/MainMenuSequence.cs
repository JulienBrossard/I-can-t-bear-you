using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class MainMenuSequence : MonoBehaviour
{
    [SerializeField] private GameObject bear;
    [SerializeField] private GameObject bearserkerElement;
    [SerializeField] private Transform secondPlaceForCam;
    
    public void Sequence1()
    {
        bearserkerElement.SetActive(true);
        Camera.main.transform.DOMove(secondPlaceForCam.position, 1f);
        bear.transform.DORotate(new Vector3(-5.91f,-190,0f),1f);
    }
}
