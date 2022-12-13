using System.Collections;
using DG.Tweening;
using UnityEngine;

public abstract class PlayerState : Entity
{
    [SerializeField] protected PlayerStateManager playerStateManager;
    [SerializeField] protected Rigidbody rb;
    [SerializeField] protected PlayerStats playerStats;
    [SerializeField] protected InterestPointsManager interestPointsManager;
    public GameObject heldObject;
    [SerializeField] protected Transform handTransform;
    private float accelerationIndex;
    [SerializeField] protected bool locked;
    [SerializeField] protected bool roarReady;
    [SerializeField] protected Transform roarFX;
    public enum SUSSTATE
    {
        NORMAL,
        SUSPICIOUS,
        FREIGHTNED
    }
    
    public SUSSTATE currentSusState = SUSSTATE.NORMAL;

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
        transform.forward = Vector3.Slerp(new Vector3(transform.forward.x,0,transform.forward.z), new Vector3(rb.velocity.x,0,rb.velocity.z), playerStats.turnTime); 
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

    public void Roar()
    {
        if (roarReady)
        {
            CameraManager.instance.CameraShake(playerStats.roarDuration, new Vector3(10f,10f,0f),5f, 5, 0.5f);
            roarFX.gameObject.SetActive(true);
            roarFX.localScale = Vector3.zero;
            roarFX.DOScale(playerStats.roarRange, playerStats.roarDuration).SetEase(Ease.OutBack).OnComplete(() => roarFX.DOScale(99999, 0.5f).OnComplete(() =>roarFX.gameObject.SetActive(false)));
            Collider[] npcAtRange = Physics.OverlapSphere(transform.position, playerStats.roarRange, LayerMask.GetMask("Npc"));
            foreach (var npc in npcAtRange)
            {
                Debug.Log("A NPC was in range of roar");
                if (Random.Range(0f, 1f) < playerStats.roarFreezeChance)
                {
                    Debug.Log(npc.gameObject.name + " got freezed by roar");
                    npc.GetComponent<Npc>().GetFreezed(playerStats.roarFreezeDuration, true);
                }
                else
                {
                    npc.GetComponent<Npc>().GetFreezed(playerStats.roarFreezeDuration, false);
                }
            }
            roarReady = false;
            locked = true;
            StartCoroutine(RoarCd());
            StartCoroutine(LockTime(playerStats.roarDuration));
        }
    }
    
    public IEnumerator LockTime(float time)
    {
        locked = true;
        yield return new WaitForSeconds(time);
        locked = false;
    } 
    
    public IEnumerator RoarCd()
    {
        yield return new WaitForSeconds(playerStats.roarCD); 
        roarReady = true;
    }
    
    protected abstract void SendRayCast(Vector3 origin, Vector3 dir, float length, float centerDistance);
}
