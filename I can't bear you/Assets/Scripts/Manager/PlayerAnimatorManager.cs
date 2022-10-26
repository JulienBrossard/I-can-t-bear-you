using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimatorManager : MonoBehaviour
{
    public static PlayerAnimatorManager instance;
    [SerializeField] private Animator animator;
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this);
        }
        instance = this;
    }
    
    public void SetAnimatorBool(string boolName, bool value)
    {
        animator.SetBool(boolName, value);
    }
    
    public void SetAnimatorTrigger(string triggerName)
    {
        animator.SetTrigger(triggerName);
    }
    
    public void SetAnimatorFloat(string floatName, float value)
    {
        animator.SetFloat(floatName, value);
    }
    
    public void SetAnimatorInt(string intName, int value)
    {
        animator.SetInteger(intName, value);
    }
}
