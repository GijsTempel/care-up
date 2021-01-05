using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetSavedPosition : StateMachineBehaviour
{
    private Transform CinematicPosition;
    private Transform ArmsPosition;
    CameraMode cameraMode;
    public int ResetTransformFrame = 0;
    protected float frame;
    protected float prevFrame;
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

        CinematicPosition = GameObject.Find("CinematicControl").transform;
        ArmsPosition = GameObject.Find("Arms").transform;
        
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (animator.speed != 0)
        {
            if (PlayerAnimationManager.CompareFrames(frame, prevFrame, ResetTransformFrame))
            {
                if (CinematicPosition != null)
                {

                    if (ArmsPosition != null)
                    {
                        CinematicPosition.transform.localPosition = new Vector3(0, 0, 0);
                        ArmsPosition.transform.localRotation = new Quaternion(0, 0, 0, 1);
                    }
                }
            }
            prevFrame = frame;
            frame = stateInfo.normalizedTime * stateInfo.length;
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
  /*  override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
        
    }*/

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
