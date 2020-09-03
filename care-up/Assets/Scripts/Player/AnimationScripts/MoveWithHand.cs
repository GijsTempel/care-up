using System.Collections.Generic;
using UnityEngine;

public class MoveWithHand : StateMachineBehaviour
{
    public string objectName = "";
    public string controlObjectName = "";
    public string finishAtTarget = "";
    public int startFrame;
    public int stopFrame;
    private GameObject obj = null;
    private GameObject controlObj = null;

    private GameObject originalParent = null;

    protected float frame;
    protected float prevFrame;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        frame = 0f;
        prevFrame = 0f;
        InitMove();

        if (startFrame == 0)
        {
            StartMove();
        }
    }

    void InitMove()
    {
        obj = GameObject.Find(objectName);
        controlObj = GameObject.Find(controlObjectName);
        if (obj != null && controlObj != null)
        {
            if (obj.transform.parent != null)
                originalParent = obj.transform.parent.gameObject;
        }
    }

    void StartMove()
    {
        var objPos = obj.transform.position;
        var objRot = obj.transform.rotation;
        obj.transform.parent = controlObj.transform;
        obj.transform.position = objPos;
        obj.transform.rotation = objRot;
    }

    void StopMove()
    {
        var objPos = obj.transform.position;
        var objRot = obj.transform.rotation;
        if (finishAtTarget != "")
        {
            if (GameObject.Find(finishAtTarget))
            {
                Transform targetTrans = GameObject.Find(finishAtTarget).transform;
                objPos = targetTrans.position;
                objRot = targetTrans.rotation;
            }
        }
        if (originalParent != null)
            obj.transform.parent = originalParent.transform;
        else
            obj.transform.parent = null;
        obj.transform.position = objPos;
        obj.transform.rotation = objRot;
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (animator.speed > 0)
        {
            prevFrame = frame;
            frame = stateInfo.normalizedTime * stateInfo.length;
            if (startFrame > 0)
            {
                if (PlayerAnimationManager.CompareFrames(frame, prevFrame, startFrame))
                    StartMove();
            }
            if (stopFrame > 0)
            { 
                if (PlayerAnimationManager.CompareFrames(frame, prevFrame, stopFrame))
                {
                    StopMove();
                }
            }
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
       if (stopFrame <= 0)
        {
            StopMove();
        }
    }
}
