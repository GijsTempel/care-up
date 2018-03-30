using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UseOnAndDelete : AnimationUseOn
{
    public int deleteFrame;
    public bool hand;

    protected float frame = 0f;
    protected float prevFrame = 0f;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);

        frame = 0f;
        prevFrame = 0f;
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateUpdate(animator, stateInfo, layerIndex);

        if (PlayerAnimationManager.CompareFrames(frame, prevFrame, deleteFrame))
        {
            inv.RemoveHandObject(hand);
        }

        if (animator.speed != 0)
        {
            prevFrame = frame;
            frame += Time.deltaTime;
        }
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateExit(animator, stateInfo, layerIndex);

        mode.animationEnded = true;
    }
}
