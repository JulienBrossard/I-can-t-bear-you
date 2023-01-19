using System.Collections;
using DG.Tweening;
using UnityEngine;

public abstract class PlayerState : Entity
{
    [SerializeField] protected PlayerStateManager playerStateManager;
    [SerializeField] protected Rigidbody rb;
    [SerializeField] public PlayerStats playerStats;
    [SerializeField] protected InterestPointsManager interestPointsManager;
    [Dracau.ReadOnly] public GameObject heldObject;
    protected IGrabbable heldObjectGrabbable;
    [SerializeField] protected Transform handTransform;
    private float accelerationIndex;
    [SerializeField] protected bool locked;
    [SerializeField] protected bool stopMoving;
    [SerializeField] protected bool roarReady;
    [SerializeField] protected Transform roarFX;
    [SerializeField] protected GameObject bearserkerElement;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip roarSound, grabSound, throwSound;
    
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

    public bool TryGrab()
    {
        if(interestPointsManager.GetGrabbable() == null) return false;
        if(interestPointsManager.GetGrabbable().Grab(handTransform) == default) return false;
        
        heldObject = interestPointsManager.GetGrabbable().Grab(handTransform).gameObject;
        audioSource.PlayOneShot(grabSound);
        
        return true;
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
            audioSource.PlayOneShot(roarSound);
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

    private float time;
    public IEnumerator EvaluateThrowForce()
    {
        stopMoving = true;
        animator.SetBool("Throw", true);
        time = 0;
        
        while (!InputManager.instance.input.Actions.Smash.WasReleasedThisFrame() && time < playerStats.maxTimeThrowHeld)
        {
            yield return new WaitForEndOfFrame();
            time += Time.deltaTime; 
            time = Mathf.Clamp(time, 0f, playerStats.maxTimeThrowHeld);
        }
        
        if (time / playerStats.maxTimeThrowHeld < playerStats.mitigationRatioDropThrow)
        {
            heldObject.GetComponent<IGrabbable>().Drop();
            animator.SetTrigger("Drop");
            animator.SetBool("Throw", false);
            stopMoving = false;

        }
        else
        {
            heldObject.GetComponent<IGrabbable>().Throw(transform.forward,time / playerStats.maxTimeThrowHeld);
            heldObject.transform.localScale = Vector3.one;
            animator.SetBool("Throw", false);
            stopMoving = false;
        }
        audioSource.PlayOneShot(throwSound);

        heldObject = null;
        heldObjectGrabbable = null; 
    }
    public IEnumerator RoarCd()
    {
        yield return new WaitForSeconds(playerStats.roarCD); 
        roarReady = true;
    }

    public override void Electrocute()
    {
        Debug.Log("Player was electrocuted");
        BearserkerGaugeManager.instance.AddBearserker(-playerStats.bearserkerReductionWhenElectrocuted);
    }
    public override void Electrocute(GameObject emitter)
    {
        Debug.Log("Player was electrocuted by " + emitter.name);
        BearserkerGaugeManager.instance.AddBearserker(-playerStats.bearserkerReductionWhenElectrocuted);
    }

    public override void Explode()
    {
        Debug.Log("Player was exploded");
        BearserkerGaugeManager.instance.AddBearserker(-playerStats.bearserkerReductionWhenExploded);
    }
}
