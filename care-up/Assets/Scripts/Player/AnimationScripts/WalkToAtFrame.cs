using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkToAtFrame : StateMachineBehaviour
{
    public string target;
    public int actionFrame = -1;
    protected float frame;
    protected float prevFrame;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        frame = 0;
        prevFrame = 0;
        if (actionFrame == 0)
        {
            MoveToTarget();
        }
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (animator.speed != 0)
        {
            if (PlayerAnimationManager.CompareFrames(frame, prevFrame, actionFrame))
            {
                MoveToTarget();
            }

            prevFrame = frame;
            frame = stateInfo.normalizedTime * stateInfo.length;
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        frame = 0;
        prevFrame = 0;
        if (actionFrame < 0)
            MoveToTarget();
    }

    private void MoveToTarget()
    {
        GameObject targetObject = GameObject.Find(target);
        if (targetObject != null)
        {
            if (targetObject.GetComponent<WalkToGroup>() != null)
            {
                Debug.Log("___________" + target);
                PlayerScript ps = GameObject.FindObjectOfType<PlayerScript>();
                ps.WalkToGroup_(targetObject.GetComponent<WalkToGroup>());
            }
        }
    }
   
}
