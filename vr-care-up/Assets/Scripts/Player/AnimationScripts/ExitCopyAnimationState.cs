using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitCopyAnimationState : StateMachineBehaviour
{
    bool exited = false;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        exited = false;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        float frame = stateInfo.normalizedTime * stateInfo.length;
        if ((stateInfo.length - frame) < 0.5f && !exited)
        {
            exited = true;
            GameObject.FindObjectOfType<PlayerScript>().ExitCopyAnimationState();
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!exited)
        {
            exited = true;
            GameObject.FindObjectOfType<PlayerScript>().ExitCopyAnimationState();
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
}
