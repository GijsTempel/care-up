using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestoreCameraOrientation : StateMachineBehaviour
{
    public int actionFrame = 0;
    public bool atTheEnd = true
        ;
    protected float frame = 0f;
    protected float prevFrame = 0f;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       
        if (actionFrame == 0 && !atTheEnd)
        {
            Camera.main.transform.localRotation = PlayerAnimationManager.GetSavedCameraOrientation();
        }
    }


    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (animator.speed != 0)
        {
            if (PlayerAnimationManager.CompareFrames(frame, prevFrame, actionFrame) && !atTheEnd)
            {
                Camera.main.transform.localRotation = PlayerAnimationManager.GetSavedCameraOrientation();
            }
            prevFrame = frame;
            frame += Time.deltaTime;
        }
    }


    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (atTheEnd)
            Camera.main.transform.localRotation = PlayerAnimationManager.GetSavedCameraOrientation();
        frame = 0;
        prevFrame = 0;

    }
}
