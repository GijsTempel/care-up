using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeWalkToGroupTarget : StateMachineBehaviour
{

    public string WalkToGroupName = "";
    public string TargetName = "";
    public int actionFrame;
    protected float frame;
    protected float prevFrame;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        frame = 0f;
        prevFrame = 0f;
        if (actionFrame == 0)
        {
            set_set();
        }
    }


    void set_set()
    {
        if (GameObject.Find(WalkToGroupName) != null && GameObject.Find(TargetName) != null)
        {
            if (GameObject.Find(WalkToGroupName).GetComponent<WalkToGroup>() != null)
            {
                GameObject.Find(WalkToGroupName).GetComponent<WalkToGroup>().SetTarget(GameObject.Find(TargetName).transform);
            }
        }


    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
    
        if (animator.speed != 0)
        {
            if (PlayerAnimationManager.CompareFrames(frame, prevFrame, actionFrame))
            {
                set_set();
            }

            prevFrame = frame;
            frame = stateInfo.normalizedTime * stateInfo.length;
        }

    }


}
