using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationSequenceState : StateMachineBehaviour {

    public List<int> keyFrames = new List<int>();

    protected int keyFrame = 0;
    protected int frame = 0;

	 // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        keyFrame = 0;
        frame = 0;
	}

	// OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
	override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        if (keyFrame < keyFrames.Count)
        {
            if (animator.speed != 0)
            {
                if (++frame == keyFrames[keyFrame])
                {
                    PlayerAnimationManager.NextSequenceStep(true);
                    animator.speed = 0f;
                    ++keyFrame;
                }
            }
        }
        else
        {
            // let the count go after last keyframe
            if (animator.speed != 0)
            {
                ++frame; 
            }
        }
	}

	// OnStateExit is called when a transition ends and the state machine finishes evaluating this state
	override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        CameraMode cameraMode = GameObject.Find("GameLogic").GetComponent<CameraMode>();
        cameraMode.ToggleCameraMode(CameraMode.Mode.Cinematic);
        cameraMode.animationEnded = true;
        cameraMode.cinematicToggle = false;
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
