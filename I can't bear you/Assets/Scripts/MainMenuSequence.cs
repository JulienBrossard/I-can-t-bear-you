using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class MainMenuSequence : MonoBehaviour
{
    [SerializeField] private GameObject bear;
    [SerializeField] private Transform secondPlaceForCam;
    
    public void Sequence1()
    {
        Camera.main.transform.DOMove(secondPlaceForCam.position, 1f);
        bear.GetComponent<Animator>().SetTrigger("Go");
        bear.transform.DORotate(new Vector3(-5.91f,-190,0f),1f);
    }
}
