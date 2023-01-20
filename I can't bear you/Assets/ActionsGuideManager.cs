using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionsGuideManager : MonoBehaviour
{
    public static ActionsGuideManager instance;
    [SerializeField] private GameObject actionsGuide;
    [SerializeField] private Image grabIcon, interactIcon, smashIcon, killIcon;
    private Camera cam;
    [Dracau.ReadOnly,SerializeField] private SightManager.State state;
    [Dracau.ReadOnly] public InterestPoint interestPoint;
    [SerializeField] private float offset;

    private void Awake()
    {
        if(instance != default) Destroy(gameObject);
        instance = this;
        cam = Camera.main;
    }
    public void SetInterestPoint(InterestPoint newInterestPoint, SightManager.State newState)
    {
        if(interestPoint == newInterestPoint && state == newState) return;
        Disable();
        interestPoint = newInterestPoint;
        state = newState;
        if (interestPoint.go.GetComponent<Entity>() != default)
        {
            actionsGuide.SetActive(true);
            killIcon.gameObject.SetActive(true);
            return;
        }
        if(interestPoint.go.GetComponent<Item>() == default)
        {
            Debug.Log("Can't identify the object");
            return;
        }

        if (state == SightManager.State.Stealth)
        {
            if (interestPoint.go.GetComponent<Item>().grabbable)
            {
                actionsGuide.SetActive(true);
                grabIcon.gameObject.SetActive(true);
            }
            if (interestPoint.go.GetComponent<IInteractable>() != default)
            {
                actionsGuide.SetActive(true);
                interactIcon.gameObject.SetActive(true);
            }
        }

        if (interestPoint.go.GetComponent<ISmashable>() == default) return;
        actionsGuide.SetActive(true);
        smashIcon.gameObject.SetActive(true);
    }
    public void RemoveInterestPoint()
    {
        if(interestPoint == default) return;
        interestPoint = default;
        Disable();
    }

    private void Disable()
    {
        actionsGuide.SetActive(false);
        grabIcon.gameObject.SetActive(false);
        interactIcon.gameObject.SetActive(false);
        smashIcon.gameObject.SetActive(false);
        killIcon.gameObject.SetActive(false);
    }

    private void Update()
    {
        if(interestPoint == default) return;
        actionsGuide.transform.position = cam.WorldToScreenPoint(interestPoint.go.transform.position) - Vector3.down * offset;
    }
}
