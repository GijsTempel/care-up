using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RippingFraxiparinePackage : AnimationCombine
{
    public bool hand;

    public int openTopFrame;
    public int dropTopFrame;
    public int swapHandsFrame;
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

        mode.dontMoveCamera = true;
        mode.SetCinematicMode(GameObject.Find("TrashBucket").transform);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (PlayerAnimationManager.CompareFrames(frame, prevFrame, openTopFrame))
        {
            inv.CreateObjectByName("fraxiPackageBottom", Vector3.zero);
            inv.ForcePickItem("fraxiPackageBottom", !hand);

            inv.ReplaceHandObject(hand, "fraxiPackageSyringeBottom");
        }
        else if (PlayerAnimationManager.CompareFrames(frame, prevFrame, dropTopFrame))
        {
            inv.FreezeObject(!hand);
        }
        else if (PlayerAnimationManager.CompareFrames(frame, prevFrame, swapHandsFrame))
        {
            inv.SwapHands();
        }
        else if (PlayerAnimationManager.CompareFrames(frame, prevFrame, removeBottomFrame))
        {
            inv.ReplaceHandObject(!hand, "fraxiPackageTop");

            inv.CreateAnimationObject("Frexi_with_needle_cap", hand);
        }
        else if (PlayerAnimationManager.CompareFrames(frame, prevFrame, dropBottomFrame))
        {
            inv.FreezeObject(!hand);
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
