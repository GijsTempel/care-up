using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CinematicAnimation : StateMachineBehaviour {
    
    public bool audio = false;
    public string audioFileName;
    public int audioFrame;
    public string positionObjectName;
    public float volume = 1f;

    protected float frame = 0f;
    protected float prevFrame = 0f;

    static CameraMode cameraMode;
    
	 // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	
        if (cameraMode == null)
        {
            cameraMode = GameObject.Find("GameLogic").GetComponent<CameraMode>();
            if (cameraMode == null) Debug.LogError("No camera mode");
        }

        frame = 0f;
        prevFrame = 0f;
	}

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (audio)
        {
            if (PlayerAnimationManager.CompareFrames(frame, prevFrame, audioFrame))
            {
                AudioClip clip = Resources.Load<AudioClip>("Audio/" + audioFileName);
                if (positionObjectName != "")
                {
                    AudioSource.PlayClipAtPoint(clip, GameObject.Find(positionObjectName).transform.position, volume);
                }
                else
                {
                    AudioSource.PlayClipAtPoint(clip, Camera.main.transform.position, volume);
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
	override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        cameraMode.animationEnded = true;
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
