using UnityEngine;

public class DrinkingBehavior : StateMachineBehaviour
{
    
    private Npc npc;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        npc = animator.gameObject.GetComponent<Npc>();
        npc.isAction = true;
        animator.SetBool("isDrinking", false);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    //override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    
    //}

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        npc.state = Npc.STATE.DANCING;
        npc.stats.currentThirst = npc.npcData.maxThirst;
        npc.isAction = false;
        if (npc.currentDestination.childCount != 0)
        {
            if (npc.currentDestination.GetChild(0).TryGetComponent(out Item item)) return;
            WasDrinkPoisonous(item);
        }
        
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
    
    public void WasDrinkPoisonous(Item item)
    {
        if (item.poisoned)
        {
            Debug.Log("died from drinking" + item.name);
            npc.Die();
        }
    }
}
