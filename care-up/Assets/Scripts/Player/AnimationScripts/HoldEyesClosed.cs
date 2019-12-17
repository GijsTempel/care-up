using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoldEyesClosed : StateMachineBehaviour
{
    CameraMode cameraMode;
    public bool toHoldClose = true;
    public int actionFrame = 0;
    protected float frame;
    protected float prevFrame;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        cameraMode = GameObject.FindObjectOfType<CameraMode>();
        if (actionFrame <= 0)
        {
            cameraMode.AllowOpenEyes = !toHoldClose;
        }
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (animator.speed != 0)
        {
            if (PlayerAnimationManager.CompareFrames(frame, prevFrame, actionFrame))
            {
                cameraMode.AllowOpenEyes = !toHoldClose;
            }
            prevFrame = frame;
            frame += Time.deltaTime;
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
