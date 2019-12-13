using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InsulinSequence : AnimationSequenceState
{
    public int takePenFrame;
    public int startButtonFrame;
    public int endButtonFrame;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);

        inv.PutAllOnTable();

        //inv.DropLeftObject();
        //inv.DropRightObject();

        inv.sequenceAborted = false;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (animator.speed != 0)
        {
            if (PlayerAnimationManager.CompareFrames(frame, prevFrame, takePenFrame))
            {
                inv.ForcePickItem("InsulinPenWithNeedle", false, true);

                PlayerAnimationManager.SetHandItem(false, GameObject.Find("InsulinPenWithNeedle"));
            }
            else if (PlayerAnimationManager.CompareFrames(frame, prevFrame, startButtonFrame))
            {
                inv.RightHandObject.GetComponent<InsulinPen>().animateButton = true;
            }
            else if (PlayerAnimationManager.CompareFrames(frame, prevFrame, endButtonFrame))
            {
                inv.RightHandObject.GetComponent<InsulinPen>().animateButton = false;
            }

            if (keyFrame < keyFrames.Count)
            {
                if (animator.speed != 0)
                {
                    if (PlayerAnimationManager.CompareFrames(frame, prevFrame, keyFrames[keyFrame]))
                    {
                        PlayerAnimationManager.NextSequenceStep(true);
                        animator.speed = 0f;
                        ++keyFrame;
                    }
                }
            }


            prevFrame = frame;
            frame += Time.deltaTime;
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateExit(animator, stateInfo, layerIndex);

        if (inv.LeftHandObject && inv.LeftHandObject.GetComponent<InsulinPen>())
            inv.LeftHandObject.GetComponent<InsulinPen>().animateButton = false;

        if (keyFrame >= keyFrames.Count && !inv.sequenceAborted)
        {
            GameObject.FindObjectOfType<InjectionPatient>().AfterSequenceDialogue();
            GameObject.FindObjectOfType<InjectionPatient>().GetComponent<Animator>().SetTrigger("ShirtDown");
        }
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
