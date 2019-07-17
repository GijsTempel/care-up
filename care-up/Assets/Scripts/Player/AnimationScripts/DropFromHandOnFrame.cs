using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropFromHandOnFrame : StateMachineBehaviour
{
    /// <summary>
    /// If drop is not desired, put negative number as the value.
    /// </summary>
    public int dropLeftFrame;
    public int dropRightFrame;

    protected HandsInventory inv;

    protected float frame = 0;
    protected float prevFrame = 0;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        inv = GameObject.Find("GameLogic").GetComponent<HandsInventory>();
	
        if (dropLeftFrame == 0)
        {
            inv.DropLeftObject();
        }

        if (dropRightFrame == 0)
        {
            inv.DropRightObject();
        }

    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (animator.speed != 0)
        {
            if (PlayerAnimationManager.CompareFrames(frame, prevFrame, dropLeftFrame))
            {
                inv.DropLeftObject();
            }

            if (PlayerAnimationManager.CompareFrames(frame, prevFrame, dropRightFrame))
            {
                inv.DropRightObject();
            }

            prevFrame = frame;
            frame += Time.deltaTime;
        }
    }

}
