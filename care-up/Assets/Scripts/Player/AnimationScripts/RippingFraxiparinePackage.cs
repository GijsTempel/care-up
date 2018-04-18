using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RippingFraxiparinePackage : AnimationCombine
{
    public bool hand;

    public int openTopFrame;
    public int dropTopFrame;
    public int removeBottomFrame;
    public int dropBottomFrame;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        inv = GameObject.Find("GameLogic").GetComponent<HandsInventory>();
        mode = GameObject.Find("GameLogic").GetComponent<CameraMode>();

        inv.ToggleControls(true);

        frame = 0f;
        prevFrame = 0f;

        if (combineFrame == 0)
        {
            inv.ExecuteDelayedCombination();
        }
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (PlayerAnimationManager.CompareFrames(frame, prevFrame, openTopFrame))
        {
            inv.RemoveHandObject(hand);

            //inv.CreateAnimationObject("fraxiPackageTop", !hand);
            //inv.CreateAnimationObject2("fraxiPackageSyringeBottom", hand);
        }
        else if (PlayerAnimationManager.CompareFrames(frame, prevFrame, dropTopFrame))
        {
            //inv.DeleteAnimationObject();
        }
        else if (PlayerAnimationManager.CompareFrames(frame, prevFrame, removeBottomFrame))
        {
            //inv.DeleteAnimationObject2();

            inv.CreateAnimationObject("Frexi_with_needle_cap", !hand);
            //inv.CreateAnimationObject2("fraxiPackageBottom", hand);
        }
        else if (PlayerAnimationManager.CompareFrames(frame, prevFrame, dropBottomFrame))
        {
            //inv.DeleteAnimationObject2();
        }
        else if (PlayerAnimationManager.CompareFrames(frame, prevFrame, combineFrame))
        {
            inv.DeleteAnimationObject();
        }

        base.OnStateUpdate(animator, stateInfo, layerIndex);
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
