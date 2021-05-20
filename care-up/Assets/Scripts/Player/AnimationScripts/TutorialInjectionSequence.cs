using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialInjectionSequence : AnimationSequenceState
{
    public int dropClothFrame;
    public int takeSyringeFrame;
    public int swapHandsFrame;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);
        
        inv.PutAllOnTable();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (PlayerAnimationManager.CompareFrames(frame, prevFrame, dropClothFrame))
        {
            //inv.PutAllOnTable();
            inv.DeleteAnimationObject();
        }
        else if (PlayerAnimationManager.CompareFrames(frame, prevFrame, takeSyringeFrame))
        {
            inv.ForcePickItem("SyringeWithAbsorptionNeedle", false);
            PlayerAnimationManager.SetHandItem(false, GameObject.Find("SyringeWithAbsorptionNeedle"));
            inv.RightHandObject.GetComponent<Syringe>().updatePlunger = true;
        }
        else if (PlayerAnimationManager.CompareFrames(frame, prevFrame, swapHandsFrame))
        {
            inv.SwapHands();
        }

        if (keyFrame < keyFrames.Count)
        {
            if (animator.speed != 0)
            {
                if (PlayerAnimationManager.CompareFrames(frame, prevFrame, keyFrames[keyFrame]))
                {
                    if (keyFrame == 0)
                    {
                        inv.CreateAnimationObject("DesinfectionCloth", false);
                        //inv.ForcePickItem("DesinfectionCloth", false);
                        //PlayerAnimationManager.SetHandItem(false, "DesinfectionCloth");
                    }
                    
                    PlayerAnimationManager.NextSequenceStep(true);
                    if ( keyFrame == 0 )
                    {
                        PlayerAnimationManager.SequenceTutorialLock(true);
                    }

                    animator.speed = 0f;
                    ++keyFrame;
                }
            }
        }

        if (animator.speed != 0)
        {
            prevFrame = frame;
            frame = stateInfo.normalizedTime * stateInfo.length;
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateExit(animator, stateInfo, layerIndex);

        if ( inv.LeftHandObject && inv.LeftHandObject.GetComponent<Syringe>())
        inv.LeftHandObject.GetComponent<Syringe>().updatePlunger = false;

        //GameObject.Find("GameLogic").GetComponent<TutorialManager>().sequenceCompleted = true;
    }

    // OnStateMove is called right after Animator.OnAnimatorMove(). Code that processes and affects root motion should be implemented here
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK(). Code that sets up animation IK (inverse kinematics) should be implemented here.
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}
}
