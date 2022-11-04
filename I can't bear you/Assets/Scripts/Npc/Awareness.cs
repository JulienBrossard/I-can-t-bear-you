using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Awareness : MonoBehaviour
{
    public float viewRadius;
    [Range(0,360)]
    public float viewAngle;

    public LayerMask targetMask;
    public LayerMask obstacleMask;

    [HideInInspector]
    public Transform visibleTarget;

    [Header("Scripts")] 
    [SerializeField] private Panic panicData;
    [SerializeField] private StatusEffects statusEffects;

    void Start() {
        StartCoroutine ("FindTargetsWithDelay", .2f);
        viewRadius = viewRadius + (panicData.currentPanic * viewRadius) -
                     (statusEffects.currentData.currentAwarenessRatio * viewRadius);
    }


    IEnumerator FindTargetsWithDelay(float delay) {
        while (true) {
            yield return new WaitForSeconds (delay);
            FindVisibleTargets ();
        }
    }

    void FindVisibleTargets()
    {
        visibleTarget = null;
        Collider[] targetsInViewRadius = Physics.OverlapSphere (transform.position, viewRadius, targetMask);

        for (int i = 0; i < targetsInViewRadius.Length; i++) {
            Transform target = targetsInViewRadius [i].transform;
            Vector3 dirToTarget = (target.position - transform.position).normalized;
            if (Vector3.Angle (transform.forward, dirToTarget) < viewAngle / 2) {
                float dstToTarget = Vector3.Distance (transform.position, target.position);

                if (!Physics.Raycast (transform.position, dirToTarget, dstToTarget, obstacleMask)) {
                    visibleTarget = target;
                }
            }
        }
    }
    
    public Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal) {
        if (!angleIsGlobal) {
            angleInDegrees += transform.eulerAngles.y;
        }
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad),0,Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }
}