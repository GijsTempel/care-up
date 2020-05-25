using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateAtFrame : StateMachineBehaviour
{
    private float currentFrame;
    private float prevFrame;

    public int actionFrame;
    public string actionObjectName;

    void Activate()
    {
        GameObject ActionObject = GameObject.Find(actionObjectName);
        if (ActionObject.GetComponent<WTGNeighborsUpdater>() != null)
        {
            ActionObject.GetComponent<WTGNeighborsUpdater>().Activate();
        }
    }

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        currentFrame = 0f;
        prevFrame = 0f;
        if (actionFrame == 0)
        {
            Activate();
        }
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (animator.speed != 0)
        {
            if (PlayerAnimationManager.CompareFrames(currentFrame, prevFrame, actionFrame))
            {
                Activate();
            }

            prevFrame = currentFrame;
            currentFrame += Time.deltaTime;
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        currentFrame = 0;
        prevFrame = 0;
        if (actionFrame < 0)
        {
            Activate();
        }
    }
}
