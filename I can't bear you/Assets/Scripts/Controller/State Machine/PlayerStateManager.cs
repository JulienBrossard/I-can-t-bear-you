using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateManager : MonoBehaviour
{
    [SerializeField] private PlayerState baseState;
    private PlayerState currentState;
    [SerializeField] private InterestPointsManager interestPointsManager;
    
    private void Start()
    {
        currentState = baseState;
    }
    void Update()
    {
        currentState.Behave();
    }

    private void FixedUpdate()
    {
        currentState.FixedBehave();
    }
    public void SwitchState(PlayerState newState)
    {
        currentState = newState;
        interestPointsManager.Clear();
    }
}
