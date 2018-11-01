using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteFromHand : StateMachineBehaviour
{
    public bool leftHand;
    public int dropFrame;
    protected HandsInventory inv;

    protected float frame;
    protected float prevFrame;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        inv = GameObject.Find("GameLogic").GetComponent<HandsInventory>();
	
        if (dropFrame == 0)
        {
            inv.RemoveHandObject(leftHand);
        }

    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

        if (animator.speed != 0)
        {
            if (PlayerAnimationManager.CompareFrames(frame, prevFrame, dropFrame))
            {
                inv.RemoveHandObject(leftHand);
            }

            prevFrame = frame;
            frame += Time.deltaTime;
        }
    }

}
