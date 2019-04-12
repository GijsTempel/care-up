using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetExtraCounter : StateMachineBehaviour {

    public int value = 0;
    public int actionFrame = 0;
    protected float frame;
    protected float prevFrame;


    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (actionFrame == 0)
        {
            animator.SetInteger("extra", value);
        }
    }


    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (animator.speed != 0)
        {
            if (PlayerAnimationManager.CompareFrames(frame, prevFrame, actionFrame))
            {
                animator.SetInteger("extra", value);
            }
            prevFrame = frame;
            frame += Time.deltaTime;
        }

    }

}
