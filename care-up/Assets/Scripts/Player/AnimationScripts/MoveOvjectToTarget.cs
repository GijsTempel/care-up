using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveOvjectToTarget : StateMachineBehaviour
{
    public string targetName;
    public string objectName;
    public bool copyRot = true;
    public int actionFrame = -1;
    protected float frame;
    protected float prevFrame;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        frame = 0f;
        prevFrame = 0f;
    }

    void MoveObject()
    {
        GameObject targetObject = GameObject.Find(targetName);
        GameObject _object = GameObject.Find(objectName);

        if (targetObject != null && _object != null)
        {
            _object.transform.position = targetObject.transform.position;
            if (copyRot)
                _object.transform.rotation = targetObject.transform.rotation;

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
                MoveObject();
            }
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (actionFrame < 0)
        {
            MoveObject();
        }
    }
}
