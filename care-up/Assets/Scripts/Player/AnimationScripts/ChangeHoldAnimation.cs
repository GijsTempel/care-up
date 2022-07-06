using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeHoldAnimation : StateMachineBehaviour
{

    private float frame;
    private float prevFrame;
    public int actionFrame = 0;
    public string ObjectName = "";
    public int HoldAnimationID = -1;


    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        frame = 0f;
        prevFrame = 0f;

    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (animator.speed != 0)
        {
            if (HoldAnimationID != -1 && ObjectName != "" && PlayerAnimationManager.CompareFrames(frame, prevFrame, actionFrame))
            {
                if (GameObject.Find(ObjectName) != null)
                {
                    if (GameObject.Find(ObjectName).GetComponent<PickableObject>() != null)
                    {
                        GameObject obj = GameObject.Find(ObjectName).gameObject;
                        obj.GetComponent<PickableObject>().holdAnimationID = HoldAnimationID;
                        if (obj.transform.parent != null)
                        {
                            if (obj.transform.parent.name == "toolHolder.L")
                            {
                                animator.SetInteger("LeftHandState", HoldAnimationID);
                            }
                            else if (obj.transform.parent.name == "toolHolder.R")
                            {
                                animator.SetInteger("RightHandState", HoldAnimationID);
                            }
                        }
                    }
                }
            }

            prevFrame = frame;
            frame = stateInfo.normalizedTime * stateInfo.length;
        }
    }

}
