using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IPadAnimation : StateMachineBehaviour
{
    public int takeFrame;
    public int dropFrame;

    public bool openingAnimation = false;

    protected float frame;
    protected float prevFrame;

    protected HandsInventory inv;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        inv = GameObject.Find("GameLogic").GetComponent<HandsInventory>();

        frame = 0f;
        prevFrame = 0f;

        PlayerScript.actionsLocked = true;
    }

	// OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
	override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (animator.speed != 0)
        {
            if (openingAnimation)
            {
                if (PlayerAnimationManager.CompareFrames(frame, prevFrame, takeFrame))
                {
                    // pick iPad in left hand
                    inv.ForcePickItem("ipad", false);
                }
            }
            else
            {
                if (PlayerAnimationManager.CompareFrames(frame, prevFrame, dropFrame))
                {
                    inv.DropRightObject();
                }

                PlayerScript.actionsLocked = true;
            }

            prevFrame = frame;
            frame += Time.deltaTime;
        }
    }

	// OnStateExit is called when a transition ends and the state machine finishes evaluating this state
	override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!openingAnimation)
        {
            GameObject.FindObjectOfType<PlayerScript>().PickItemsBackAfterRobotUI();

            RobotManager.SetUITriggerActive(true);

            GameObject.FindObjectOfType<Cheat_CurrentAction>().PostAnimationFadeIn();
        }

        PlayerScript.actionsLocked = false;
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
