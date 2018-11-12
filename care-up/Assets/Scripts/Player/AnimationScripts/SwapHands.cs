using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwapHands : StateMachineBehaviour {

    private float frame;
    private float prevFrame;
    public int actionFrame = 0;

    HandsInventory handsInventory;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        frame = 0f;
        prevFrame = 0f;
        handsInventory = GameObject.Find("GameLogic").GetComponent<HandsInventory>();

        if (actionFrame == 0)
        {
            handsInventory.SwapHands();
        }
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (animator.speed != 0)
        {
            if (PlayerAnimationManager.CompareFrames(frame, prevFrame, actionFrame))
            {
                handsInventory.SwapHands();
            }

            prevFrame = frame;
            frame += Time.deltaTime;
        }
    }
}
