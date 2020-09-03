using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationSequenceState : StateMachineBehaviour {


    public string SequenceName = "";
    public bool TuggleFromCinematic = true;
    public List<int> keyFrames = new List<int>();

    protected int keyFrame = 0;

    protected float frame = 0f;
    protected float prevFrame = 0f;
    

    protected HandsInventory inv;

	 // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)

    {
        keyFrame = 0;
        frame = 0f;
        prevFrame = 0f;

        inv = GameObject.FindObjectOfType<HandsInventory>();
        if (SequenceName != "")
        {
            PlayerAnimationManager.PlayAnimationSequence(SequenceName);
        }
        
        if (GameObject.Find("ItemDescription") != null)
            GameObject.Find("ItemDescription").SetActive(false);
    }

	// OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
	override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        if (keyFrame < keyFrames.Count)
        {
            if (animator.speed != 0)
            {
                prevFrame = frame;
                frame = stateInfo.normalizedTime * stateInfo.length;

                if (PlayerAnimationManager.CompareFrames(frame, prevFrame, keyFrames[keyFrame]))
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
                prevFrame = frame;
                frame = stateInfo.normalizedTime * stateInfo.length;
            }
        }
	}

	// OnStateExit is called when a transition ends and the state machine finishes evaluating this state
	override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {

        if (TuggleFromCinematic)
        {
            CameraMode cameraMode = GameObject.Find("GameLogic").GetComponent<CameraMode>();
            cameraMode.ToggleCameraMode(CameraMode.Mode.Cinematic);
            cameraMode.animationEnded = true;
            cameraMode.cinematicToggle = false;
        }
        // unlock 2nd workfield action
        WorkField wf = GameObject.FindObjectOfType<WorkField>();
        if (wf != null)
        {
            wf.cleaningLocked = false;
        }
    }	
}
