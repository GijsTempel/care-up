using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropObjectOnFrame : StateMachineBehaviour
{
    public int dropFrame;
    public bool isLeftHand;
    public string targetPositionObjectName;

    protected float frame;
    protected float prevFrame;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        frame = 0f;
        prevFrame = 0f;

        if (dropFrame == 0)
        {
            DropObject();

        }
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (animator.speed != 0)
        {
            prevFrame = frame;
            frame = stateInfo.normalizedTime * stateInfo.length;

            if (PlayerAnimationManager.CompareFrames(frame, prevFrame, dropFrame))
            {
                DropObject();
            }
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (dropFrame < 0 || (dropFrame / 60f > frame))
        {
            DropObject();
        }
    }
    private void DropObject()
    {
        PlayerScript player = GameObject.FindObjectOfType<PlayerScript>();
        if (player != null)
        {
            GameObject obj = player.GetObjectInHand(isLeftHand);
            if (obj != null)
            {
                player.ForceDropFromHand(isLeftHand, true);
                GameObject posObj = GameObject.Find(targetPositionObjectName);
                if (posObj != null)
                {
                    obj.transform.position = posObj.transform.position;
                    obj.transform.rotation = posObj.transform.rotation;
                }
            }
        }
    }
}
