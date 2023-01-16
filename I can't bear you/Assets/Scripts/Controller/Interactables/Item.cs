using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour,IGrabbable, IAffectable
{
    enum DangerState
    {
        Safe,
        Suspicious,
        Dangerous
    }
    [Header("Danger State")]
    [SerializeField] DangerState dangerState;
    
    public virtual void DeleteItem()
    {
        Destroy(gameObject);
    }

    /// <summary>
    /// Set whether the item can de detected by the player.
    /// </summary>
    /// <param name="detectable">Detectability</param>
    public void SetDetectability(bool detectable)
    {
        if (detectable) gameObject.layer = LayerMask.NameToLayer("Default");
        else gameObject.layer = LayerMask.NameToLayer("Not Interactable Item");
    }

    [Header("Puddle")]
    [SerializeField] private PuddleType puddleType;
    [Range(0.5f,5f)] public float puddleSize;
    public virtual GameObject CreatePuddle()
    {
        GameObject puddleBuffer;
        switch (puddleType)
        {
            case PuddleType.NONE:
                Debug.LogError("Puddle type of " + gameObject.name + " is set to NONE.");
                return null;
            case PuddleType.WATER:
                puddleBuffer = Instantiate((GameObject)Resources.Load("Water Puddle"), new Vector3(transform.position.x,0.5f,transform.position.z), Quaternion.identity);
                break;
            case PuddleType.HONEY:
                puddleBuffer = Instantiate((GameObject)Resources.Load("Honey Puddle"), new Vector3(transform.position.x,0.5f,transform.position.z), Quaternion.identity);
                break;
            case PuddleType.ALCOOL:
                puddleBuffer = Instantiate((GameObject)Resources.Load("Alcool Puddle"), new Vector3(transform.position.x,0.5f,transform.position.z), Quaternion.identity);
                break;
            case PuddleType.ACID:
                puddleBuffer = Instantiate((GameObject)Resources.Load("Acid Puddle"), new Vector3(transform.position.x,0.5f,transform.position.z), Quaternion.identity);
                break;
            case PuddleType.BLOOD:
                puddleBuffer = Instantiate((GameObject)Resources.Load("Blood Puddle"), new Vector3(transform.position.x,0.5f,transform.position.z), Quaternion.identity);
                break;
            default:
                Debug.LogError("Unknown puddle type of " + gameObject.name);
                return null;
        }
        puddleBuffer.transform.localScale = Vector3.one * puddleSize;
        return puddleBuffer;
    }
    
    [HideInInspector] public GameObject zone;
    public virtual GameObject CreateZone()
    {
        zone = Instantiate((GameObject)Resources.Load("Electricity Zone"), transform.position, Quaternion.identity,transform);
        return zone;
    }
    public virtual void EnableZone()
    {
        zone.SetActive(true);
    }
    public virtual void DisableZone()
    {
        zone.SetActive(false);
    }

    [Header("Grab")] 
    public bool grabbable;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private BoxCollider collider;
    [Header("Throw Data")]
    [SerializeField] private float throwForce;
    public LineRenderer lineRenderer;
    [SerializeField] [Range(10, 100)] private int linePoints = 25;
    [SerializeField] [Range(0.01f, 0.25f)] private float timeBetweenPoints = 0.1f;
    private LayerMask itemCollisionMask;
    [SerializeField] private LayerMask raycastAimMask;
    private RaycastHit hit;
    [HideInInspector] public bool thrown;

    private void Awake()
    {
        InitLayerMaskForProjection();
    }

    /// <summary>
    /// Init layer mask for the projection
    /// </summary>
    void InitLayerMaskForProjection()
    {
        int itemLayer = gameObject.layer;
        for (int i = 0; i < 32; i++)
        {
            if (Physics.GetIgnoreLayerCollision(itemLayer, i))
            {
                itemCollisionMask |= 1 << i;
            }
        }
    }

    public Transform Grab(Transform hand)
    {
        if (!grabbable) return default;
        
        // Set Player SusState
        switch (dangerState)
        {
            case DangerState.Safe:
                break;
            case DangerState.Suspicious:
                PlayerStateManager.instance.currentState.currentSusState = PlayerState.SUSSTATE.SUSPICIOUS;
                break;
            case DangerState.Dangerous:
                PlayerStateManager.instance.currentState.currentSusState = PlayerState.SUSSTATE.FREIGHTNED;
                break;
        }
        
        //Debug.Log("Grabbing " + gameObject.name);
        SetAsGrabbed(hand);
        return transform;
    }

    public void SetAsGrabbed(Transform hand)
    {
        collider.enabled = false;
        gameObject.layer = LayerMask.NameToLayer("Held Item");
        transform.SetParent(hand);
        rb.isKinematic = true;
        transform.localPosition = Vector3.zero;
    }
    public void Drop()
    {
        // Set Player SusState to normal
        PlayerStateManager.instance.currentState.currentSusState = PlayerState.SUSSTATE.NORMAL;
        
        SetAsReleased();
    }
    public void Throw(Vector3 dir, float forceRatio)
    {
        SetAsReleased();
        rb.AddForce(dir * (throwForce*forceRatio), ForceMode.Impulse);
        thrown = true;
    }

    /// <summary>
    /// Draw Trajectory of the object
    /// </summary>
    public void DrawProjection()
    {
        Debug.Log("Drawing projection");
        lineRenderer.enabled = true;
        lineRenderer.positionCount = Mathf.CeilToInt(linePoints / timeBetweenPoints) + 1;
        Vector3 startVelocity = throwForce * LevelManager.instance.GetPlayer().transform.forward / rb.mass;
        int i = 0;
        lineRenderer.SetPosition(i, transform.position);
        Vector3 point;
        for (float time = 0; time < linePoints; time += timeBetweenPoints)
        {
            i++;
            point = transform.position + time * startVelocity;
            point.y = transform.position.y + startVelocity.y * time + (Physics.gravity.y / 2f * time * time);
            lineRenderer.SetPosition(i, point);

            Vector3 lastPosition = lineRenderer.GetPosition(i - 1);
            if (Physics.Raycast(lastPosition, (point - lastPosition).normalized, out hit, (point - lastPosition).magnitude, raycastAimMask))
            {
                lineRenderer.SetPosition(i , hit.point);
                lineRenderer.positionCount = i + 1;
                return;
            }
        }
    }
    
    public void SetAsReleased()
    {
        lineRenderer.enabled = false;
        collider.enabled = true;
        transform.SetParent(null);
        rb.isKinematic = false;
        StartCoroutine(WaitForPlayerCollision());
    }

    IEnumerator WaitForPlayerCollision()
    {
        yield return new WaitForSeconds(0.25f);
        gameObject.layer = LayerMask.NameToLayer("Default");
    }

    [Header("Electricity")]
    [SerializeField] private bool itemCharged, itemConductor;
    public float zoneSize;
    public bool emitterDependant;
    public GameObject emitter;
    public bool charged { get => itemCharged; set => itemCharged = value;}
    public bool conductor { get => itemConductor; set => itemConductor = value; }

    public virtual void Electrocute()
    {
        if(!conductor) return;
        if (charged) return;
        
        //Debug.Log("Electrocuted " + gameObject.name + " with no depedancy");
        charged = true;
        EnableZone();
    }
    public virtual void Electrocute(GameObject emitter)
    {
        if(!conductor) return;
        if (charged) return;
        
        //Debug.Log("Electrocuted " + gameObject.name + " with depedancy of " + emitter.name);
        emitterDependant = true;
        this.emitter = emitter;
        charged = true;
        EnableZone();
    }

    public virtual void Stomp(Vector3 srcPos)
    {
        if(fallable) return;
        Fall(srcPos);
    }
    public virtual void DeElectrocute()
    {
        //Debug.Log("DeElectrocuted " + gameObject.name);
        charged = false;
        DisableZone();
    } 

    private void Update()
    {
        if(!charged) return;
        if(emitterDependant)
        {
            if(emitter == null)
            {
                DeElectrocute();
                return;
            }
            if(!emitter.GetComponent<IAffectable>().charged)
            {
                DeElectrocute();
                return;
            }
        }
    }

    [Header("Heavy")]
    public bool fallable;
    public List<FallAsset> falls;
    private bool falling;
    public virtual void Fall(Vector3 source)
    {
        if(!fallable)return;
        //Debug.Log( "Falling " + gameObject.name);
        GetComponent<Rigidbody>().isKinematic = false;
        GetComponent<Rigidbody>().AddForceAtPosition(GetFall(source).Dir * GetFall(source).force, transform.position + Vector3.up);
        falling = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        OnHitGround(collision);
    }

    public virtual void OnHitGround(Collision collision)
    {
        thrown = false;
        
        if(!falling) return;
        Instantiate((GameObject)Resources.Load("Stomp Zone"), transform.position, Quaternion.identity);
    }

    private FallAsset fallBuffer = new (Vector3.zero, 0);
    private FallAsset GetFall(Vector3 source)
    {
        foreach (FallAsset fall in falls)
        {
            if (fallBuffer.Dir.normalized == default || Vector3.Dot(fall.Dir.normalized,source.normalized) < Vector3.Dot(fallBuffer.Dir.normalized,source.normalized)) fallBuffer = fall;
        }
        return fallBuffer;
    }
    
    [Header("Gas")]
    [Range(0.5f,5f)]
    [SerializeField] private float gasSize;
    public void CreateGas()
    {
        Instantiate((GameObject)Resources.Load("Gas"), transform.position, Quaternion.identity).transform.localScale = Vector3.one * gasSize;
    }
    
    [Header("Explosive")]
    [SerializeField] private bool isExplosive;
    public bool explosive { get => isExplosive; set => isExplosive = value; }
    public virtual void Explode()
    {
        if(!explosive) return;
        Instantiate((GameObject)Resources.Load("Explosion"), transform.position, Quaternion.identity);
        if (TryGetComponent<Collider>(out Collider col)) col.enabled = false;
        //Debug.Log("Exploded " + gameObject.name);
        DeleteItem();
    }
}

[Serializable]
public class FallAsset
{
    [SerializeField] private Vector3 dir;

    public FallAsset(Vector3 newDir, float newForce)
    {
        Dir = newDir;
        force = newForce;
    }
    public Vector3 Dir{get => dir; set => dir = value.normalized;}
    public float force;
}
