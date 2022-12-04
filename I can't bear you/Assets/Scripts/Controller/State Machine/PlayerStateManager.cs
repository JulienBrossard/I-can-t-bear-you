using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateManager : MonoBehaviour
{
    [SerializeField] private PlayerState baseState;
    public PlayerState currentState;
    [SerializeField] private InterestPointsManager interestPointsManager;
    public static PlayerStateManager instance;



    private void Awake()
    {
        instance = this;
    }

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
        currentState.OnStateEnter();
        interestPointsManager.Clear();
    }
}
