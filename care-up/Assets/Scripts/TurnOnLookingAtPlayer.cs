using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnOnLookingAtPlayer : StateMachineBehaviour
{
    private float currentFrame;
    private float prevFrame;

    public int actionFrame;
    public string ObjectName = "";
    public bool atTheEnd = false;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        
        currentFrame = 0f;
        prevFrame = 0f;
        if (actionFrame == 0 && !atTheEnd)
        {

            if (ObjectName != null)
            {
              GameObject.Find(ObjectName).GetComponent<PersonObject>().lookAtCamera = true;
              //GameObject.Find(ObjectName).GetComponent<CatherisationPatient>().lookAtCamera = false;
            }
            
        }
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

        if (animator.speed != 0)
        {
            if (!atTheEnd)
            {
                if (PlayerAnimationManager.CompareFrames(currentFrame, prevFrame, actionFrame))
                {
                    if (ObjectName != null)
                    {
                        GameObject.Find(ObjectName).GetComponent<PersonObject>().lookAtCamera = true;
                        //GameObject.Find(ObjectName).GetComponent<CatherisationPatient>().lookAtCamera = false;
                    }
                }

                prevFrame = currentFrame;
                currentFrame = stateInfo.normalizedTime * stateInfo.length;
            }
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
   override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        currentFrame = 0;
        prevFrame = 0;
        if (atTheEnd)
        {
            if (ObjectName != null)
            {
                GameObject.Find(ObjectName).GetComponent<PersonObject>().lookAtCamera = true;
                //GameObject.Find(ObjectName).GetComponent<CatherisationPatient>().lookAtCamera = false;
            }
        }
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}
