using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteObjectFromHandOnFrame : StateMachineBehaviour
{
    public int deleteFrame;
    public bool isLeftHand;

    protected float frame;
    protected float prevFrame;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        frame = 0f;
        prevFrame = 0f;

        if (deleteFrame == 0)
        {
            DeleteObject();

        }
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (animator.speed != 0)
        {
            prevFrame = frame;
            frame = stateInfo.normalizedTime * stateInfo.length;

            if (PlayerAnimationManager.CompareFrames(frame, prevFrame, deleteFrame))
            {
                DeleteObject();
            }
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (deleteFrame < 0 || (deleteFrame / 60f > frame))
        {
            DeleteObject();
        }
    }
    private void DeleteObject()
    {
        PlayerScript player = GameObject.FindObjectOfType<PlayerScript>();
        if (player != null)
        {
            player.ForceDeleteObjectFromHand(isLeftHand);
        }

    }
}
