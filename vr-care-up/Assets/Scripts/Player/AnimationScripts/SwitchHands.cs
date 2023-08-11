using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchHands : StateMachineBehaviour
{
    public int actionFrame;

    protected float frame;
    protected float prevFrame;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        frame = 0f;
        prevFrame = 0f;

        if (actionFrame == 0)
        {
            SwitchObjectsInHands();

        }
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (animator.speed != 0)
        {
            prevFrame = frame;
            frame = stateInfo.normalizedTime * stateInfo.length;

            if (PlayerAnimationManager.CompareFrames(frame, prevFrame, actionFrame))
            {
                SwitchObjectsInHands();
            }
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (actionFrame < 0 || (actionFrame / 60f > frame))
        {
            SwitchObjectsInHands();
        }
    }
    private void SwitchObjectsInHands()
    {
        PlayerScript player = GameObject.FindObjectOfType<PlayerScript>();
        if (player != null)
        {
            player.SwitchHands();
        }

    }
}
