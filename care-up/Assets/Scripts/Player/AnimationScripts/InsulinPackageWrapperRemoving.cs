using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InsulinPackageWrapperRemoving : StateMachineBehaviour
{
    public int combineFrame;
    public int wrapperDestroyingFrame;
    public bool hand;

    protected float frame;
    protected float prevFrame;

    protected HandsInventory inv;
    protected CameraMode mode;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

        inv = GameObject.Find("GameLogic").GetComponent<HandsInventory>();
        inv.ToggleControls(true);

        mode = GameObject.Find("GameLogic").GetComponent<CameraMode>();
        mode.animating = true;

        frame = 0f;
        prevFrame = 0f;

        if (combineFrame == 0)
        {
            inv.ExecuteDelayedCombination();
            inv.CreateAnimationObject("InsulinPenWrapper", !hand);
        }
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (animator.speed != 0)
        {
            prevFrame = frame;
            frame += Time.deltaTime;

            if (PlayerAnimationManager.CompareFrames(frame, prevFrame, combineFrame))
            {
                inv.ExecuteDelayedCombination();
                inv.CreateAnimationObject("InsulinPenWrapper", !hand);
            }

            if (PlayerAnimationManager.CompareFrames(frame, prevFrame, wrapperDestroyingFrame))
            {
                inv.DeleteAnimationObject();
            }
          
            inv.ToggleControls(true);
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        inv.ToggleControls(false);
        mode.animating = false;
    }   
}
