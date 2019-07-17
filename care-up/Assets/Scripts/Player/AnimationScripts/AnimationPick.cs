using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationPick : StateMachineBehaviour {

    public bool hand;

    private float frame;
    private float prevFrame;
    private HandsInventory inv;
    bool isHolding = false;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {

        inv = GameObject.Find("GameLogic").GetComponent<HandsInventory>();

        frame = 0f;
        prevFrame = 0f;
        
        RobotManager.SetUITriggerActive(false);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (animator.speed != 0)
        {
            if (PlayerAnimationManager.CompareFrames(frame, prevFrame, 50))
            {
                isHolding = true;
                inv.SetHold(hand);
            }

            inv.ToggleControls(true);
            PlayerScript.actionsLocked = true;
            
            prevFrame = frame;
            frame += Time.deltaTime;
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!isHolding)
            inv.SetHold(hand);

        inv.ToggleControls(false);
        PlayerScript.actionsLocked = false;

        if (GameObject.FindObjectOfType<TutorialManager>() == null ||
            GameObject.FindObjectOfType<Tutorial_UI>() != null ||
            GameObject.FindObjectOfType<Tutorial_Theory>() != null)
        {
            RobotManager.SetUITriggerActive(true);
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
