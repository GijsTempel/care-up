using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetTriggerIntValue : StateMachineBehaviour
{
    public string TriggerName;
    public int value;
    public int actionFrame = 0;
    protected float frame;
    protected float prevFrame;
    public string ObjectName = "";

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (actionFrame == 0)
        {
            animator.SetInteger(TriggerName, value);
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
                if (ObjectName == "")
                {
                    animator.SetInteger(TriggerName, value);
                }
                else
                {
                    if (GameObject.Find(ObjectName) != null)
                    {
                        if (GameObject.Find(ObjectName).GetComponent<Animator>() != null)
                            GameObject.Find(ObjectName).GetComponent<Animator>().SetInteger(TriggerName, value);
                    }
                }
            }
        }
    }


    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (actionFrame / 60f > frame)
        {
            if (ObjectName == "")
            {
                animator.SetInteger(TriggerName, value);
            }
            else
            {
                if (GameObject.Find(ObjectName) != null)
                {
                    if (GameObject.Find(ObjectName).GetComponent<Animator>() != null)
                        GameObject.Find(ObjectName).GetComponent<Animator>().SetInteger(TriggerName, value);
                }
            }
        }
    }


}
