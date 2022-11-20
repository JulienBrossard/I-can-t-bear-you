using UnityEngine;

public abstract class PlayerState : Entity
{
    [SerializeField] protected PlayerStateManager playerStateManager;
    [SerializeField] protected Rigidbody rb;
    [SerializeField] protected PlayerStats playerStats;
    [SerializeField] protected InterestPointsManager interestPointsManager;
    protected GameObject heldObject;
    [SerializeField] protected Transform handTransform;
    private float accelerationIndex;
    //a ajouter dans player stats
    private float turnTime = 0.1f;
    public abstract void OnStateEnter();
    public abstract void Behave();
    public abstract void FixedBehave();
    public void Move()
    {
        if (InputManager.instance.input.Movement.Move.ReadValue<Vector2>() == Vector2.zero)
        {
            Deccelerate();
            return;
        }
        accelerationIndex = Mathf.Clamp(accelerationIndex + playerStats.accelerationStep, 0, 1);
        transform.forward = Vector3.Slerp(new Vector3(transform.forward.x,0,transform.forward.z), new Vector3(rb.velocity.x,0,rb.velocity.z), turnTime); 
        //transform.LookAt(transform.position + new Vector3(InputManager.instance.input.Movement.Move.ReadValue<Vector2>().x, 0, InputManager.instance.input.Movement.Move.ReadValue<Vector2>().y).normalized);
        rb.velocity = new Vector3(InputManager.instance.input.Movement.Move.ReadValue<Vector2>().x * playerStats.accelerationCurve.Evaluate(accelerationIndex) * playerStats.maxSpeed * currentSpeedRatio,
            rb.velocity.y,
            InputManager.instance.input.Movement.Move.ReadValue<Vector2>().y * (playerStats.accelerationCurve.Evaluate(accelerationIndex) * playerStats.maxSpeed * currentSpeedRatio));
    }
    public void Deccelerate()
    {
        accelerationIndex = Mathf.Clamp(accelerationIndex - playerStats.slowdownStep, 0, 1);
        rb.velocity = new Vector3(transform.forward.x,0,transform.forward.z) * (playerStats.slowdownCurve.Evaluate(Mathf.Lerp(1,0,accelerationIndex)) * playerStats.maxSpeed * currentSpeedRatio) + new Vector3(0,rb.velocity.y,0);
    }
    private float tempAngle;
    public void LookForInterestPoints(float angle, float range, float step)
    {
        tempAngle = transform.rotation.eulerAngles.y/180f*Mathf.PI;
        for (float i = 0; i < angle; i += step)
        {
            SendRayCast(transform.position,new Vector3(Mathf.Sin(i+tempAngle),0,Mathf.Cos(i+tempAngle)), range, i/angle);
            SendRayCast(transform.position,new Vector3(Mathf.Sin(-i+tempAngle),0,Mathf.Cos(-i+tempAngle)), range, i/angle);
        }
    }
    protected abstract void SendRayCast(Vector3 origin, Vector3 dir, float length, float centerDistance);
}
