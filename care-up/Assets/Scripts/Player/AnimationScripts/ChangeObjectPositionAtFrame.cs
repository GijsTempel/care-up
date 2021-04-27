using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ChangeObjectPositionAtFrame : StateMachineBehaviour
{
  

    public int ActionFrame;
    public string ObjName;
    public string TargetPosition;

    protected float frame;
    protected float prevFrame;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {


    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {


        if (animator.speed != 0)
        {
            prevFrame = frame;
            frame = stateInfo.normalizedTime * stateInfo.length;

            if (PlayerAnimationManager.CompareFrames(frame, prevFrame, ActionFrame))
            {

                if (GameObject.Find(ObjName))
                {
                    GameObject.Find(ObjName).gameObject.transform.position = GameObject.Find(TargetPosition).gameObject.transform.position;
                    GameObject.Find(ObjName).gameObject.transform.rotation = GameObject.Find(TargetPosition).gameObject.transform.rotation;
                }
            }
        }

        prevFrame = frame;
        frame = stateInfo.normalizedTime * stateInfo.length;

    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }
}
