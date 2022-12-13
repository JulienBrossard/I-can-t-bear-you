using UnityEngine;

public class PlayerStateManager : MonoBehaviour
{
    [SerializeField] private PlayerState baseState;
    public PlayerState bearserkerState;
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
        if (currentState != bearserkerState)
        {
            currentState = newState;
            currentState.OnStateEnter();
            interestPointsManager.Clear();
        }
    }
}
