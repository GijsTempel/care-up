using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecombineAndDropToTrash : AnimationCombine
{
    public bool dropHand;
    public int dropFrame;
	public string trashName = "TrashBucket";

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        inv = GameObject.Find("GameLogic").GetComponent<HandsInventory>();
        mode = GameObject.Find("GameLogic").GetComponent<CameraMode>();
		if (GameObject.Find(trashName) == null)
		{
			trashName = "PlasticTrashbucket";
		}

        inv.ToggleControls(true);
		//if (inv.
        frame = 0f;
        prevFrame = 0f;

		GameObject objToThrow;
		if (dropHand)
		{
			objToThrow = inv.rightHandObject.gameObject;
		}
		else
		{
			objToThrow = inv.leftHandObject.gameObject;
		}
		if (objToThrow.GetComponent<ExtraObjectOptions>() != null)
		{
			if (objToThrow.GetComponent<ExtraObjectOptions>().TrashBin != "")
				trashName = objToThrow.GetComponent<ExtraObjectOptions>().TrashBin;
		}

        if (combineFrame == 0)
        {
            inv.ExecuteDelayedCombination();
        }

        mode.dontMoveCamera = true;
		mode.SetCinematicMode(GameObject.Find(trashName).transform);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (PlayerAnimationManager.CompareFrames(frame, prevFrame, dropFrame))
        {
            //inv.FreezeObject(dropHand);
			inv.RemoveHandObject(dropHand);
            
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
