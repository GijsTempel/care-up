using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetCameraAngleOnFrame : StateMachineBehaviour
{
    public int angleFrame;
    public Vector3 angle;

    protected float frame = 0f;
    protected float prevFrame = 0f;
    
    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (PlayerAnimationManager.CompareFrames(frame, prevFrame, angleFrame))
        {
            Camera.main.transform.localRotation = Quaternion.Euler(angle);
        }

        if (animator.speed != 0)
        {
            prevFrame = frame;
            frame += Time.deltaTime;
        }
    }
}
