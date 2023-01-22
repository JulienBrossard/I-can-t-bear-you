using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class OnEnableChangeFirstSelected : MonoBehaviour
{
    [SerializeField] private EventSystem firstSelected;
    [SerializeField] private GameObject firstSelectedObject;

    private void OnEnable()
    {
        firstSelected.SetSelectedGameObject(null);
        firstSelected.SetSelectedGameObject(firstSelectedObject);
    }
}
