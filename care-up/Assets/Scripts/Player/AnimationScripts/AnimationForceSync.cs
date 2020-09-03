using System.Collections.Generic;
using UnityEngine;

public class AnimationForceSync : StateMachineBehaviour
{
    public List<int> syncFrames = new List<int>();

    protected float frame;
    protected float prevFrame;

    public void SyncAnimations(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        int targetLayer = (layerIndex == 0) ? 1 : 0;
        animator.Play(0, targetLayer, stateInfo.normalizedTime);
    }

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        frame = 0f;
        prevFrame = 0f;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (animator.speed != 0)
        {
            foreach (int syncFrame in syncFrames)
            {
                if (PlayerAnimationManager.CompareFrames(frame, prevFrame, syncFrame))
                {
                    SyncAnimations(animator, stateInfo, layerIndex);
                }
            }

            prevFrame = frame;
            frame = stateInfo.normalizedTime * stateInfo.length;
        }
    }
}
