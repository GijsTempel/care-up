using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatherisationPutClothUnder : CinematicAnimation
{
    public int changeClothModelFrame;
    public int dropClothFrame;
    public Vector3 clothPosition;

    private HandsInventory inv;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        inv = GameObject.FindObjectOfType<HandsInventory>();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (PlayerAnimationManager.CompareFrames(frame, prevFrame, changeClothModelFrame))
        {
            if (inv.LeftHandObject != null)
            {
                inv.RemoveHandObject(true);
            }

            if (inv.RightHandObject != null)
            {
                inv.RemoveHandObject(false);
            }

            inv.CreateAnimationObject("", true);
        }

        if (PlayerAnimationManager.CompareFrames(frame, prevFrame, dropClothFrame))
        {
            inv.DeleteAnimationObject();

            inv.CreateObjectByName("", clothPosition);
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //}

    // OnStateMove is called right after Animator.OnAnimatorMove(). Code that processes and affects root motion should be implemented here
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK(). Code that sets up animation IK (inverse kinematics) should be implemented here.
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}
}
