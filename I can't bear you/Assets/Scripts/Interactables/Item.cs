using System;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour, IAffectable
{
    public virtual void DeleteItem()
    {
        Destroy(gameObject);
    }

    [Header("Puddle")]
    [SerializeField] private PuddleType puddleType;
    [Range(0.5f,5f)]public float puddleSize = 1;
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
        
        Debug.Log("Electrocuted " + gameObject.name + " with no depedancy");
        charged = true;
        EnableZone();
    }
    public virtual void Electrocute(GameObject emitter)
    {
        if(!conductor) return;
        if (charged) return;
        
        Debug.Log("Electrocuted " + gameObject.name + " with depedancy of " + emitter.name);
        emitterDependant = true;
        this.emitter = emitter;
        charged = true;
        EnableZone();
    }
    public virtual void Stomp()
    {
        return;
    }
    public virtual void DeElectrocute()
    {
        Debug.Log("DeElectrocuted " + gameObject.name);
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
        Debug.Log( "Falling " + gameObject.name);
        GetComponent<Rigidbody>().isKinematic = false;
        GetComponent<Rigidbody>().AddForce(GetFall(source).Dir * GetFall(source).force);
        falling = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        OnHitGround();
    }

    public virtual void OnHitGround()
    {
        if(!falling) return;
        Instantiate((GameObject)Resources.Load("Stomp Zone"), transform.position, Quaternion.identity);
        DeleteItem();
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
    
    [Header("Explosive")]
    [SerializeField] private bool isExplosive;
    public bool explosive { get => isExplosive; set => isExplosive = value; }
    public virtual void Explode()
    {
        Instantiate((GameObject)Resources.Load("Explosion"), transform.position, Quaternion.identity);
        Debug.Log("Exploded " + gameObject.name);
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
