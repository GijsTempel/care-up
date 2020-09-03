using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetCameraAngleOnFrame : StateMachineBehaviour
{
    public int angleFrame = 0;
    public Vector3 angle;

    public bool saveCameraOrientation = true;
    public bool restoreCameraOrientation = true;

    protected float frame = 0f;
    protected float prevFrame = 0f;
    
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (saveCameraOrientation)
            PlayerAnimationManager.SetSavedCameraOrientation(Camera.main.transform.localRotation);
        if (angleFrame == 0)
        {
            Camera.main.transform.localRotation = Quaternion.Euler(angle);
        }
       
    }



    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (animator.speed != 0)
        {
            if (PlayerAnimationManager.CompareFrames(frame, prevFrame, angleFrame))
            {
                Camera.main.transform.localRotation = Quaternion.Euler(angle);
            }
            prevFrame = frame;
            frame = stateInfo.normalizedTime * stateInfo.length;
        }
    }



    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        frame = 0;
        prevFrame = 0;
        if (restoreCameraOrientation)
            Camera.main.transform.localRotation = PlayerAnimationManager.GetSavedCameraOrientation();
    }

}
