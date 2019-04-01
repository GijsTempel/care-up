using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetTriggerAtFrame : StateMachineBehaviour
{
    private float currentFrame;
    private float prevFrame;

    public int actionFrame;
    public string trigger;
    public string ObjectName = "";
    public bool toPrintFrames = false;


    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        currentFrame = 0f;
        prevFrame = 0f;
        if (actionFrame == 0)
        {
            set_trigger(animator);
        }
    }

    void set_trigger(Animator animator)
    {
        if (ObjectName == "")
        {
            animator.SetTrigger(trigger);
        }
        else
        {
            if (GameObject.Find(ObjectName) != null)
            {
                if (GameObject.Find(ObjectName).GetComponent<Animator>() != null)                
                    GameObject.Find(ObjectName).GetComponent<Animator>().SetTrigger(trigger);
            }
        }
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (animator.speed != 0)
        {
            if (PlayerAnimationManager.CompareFrames(currentFrame, prevFrame, actionFrame))
            {
                set_trigger(animator);
            }

            prevFrame = currentFrame;
            currentFrame += Time.deltaTime;
        }
    }

}
