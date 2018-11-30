using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpGlovesAnimation : StateMachineBehaviour
{
    private float frame;
    private float prevFrame;
    public int showFrame = 1;
    public bool toPickup = true;

    HandsInventory handsInventory;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        frame = 0f;
        prevFrame = 0f;

        handsInventory = GameObject.Find("GameLogic").GetComponent<HandsInventory>();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (animator.speed != 0)
        {
            if (PlayerAnimationManager.CompareFrames(frame, prevFrame, showFrame))
            {
                handsInventory.GlovesToggle(toPickup);
            }
            
            prevFrame = frame;
            frame += Time.deltaTime;
        }
    }


}
