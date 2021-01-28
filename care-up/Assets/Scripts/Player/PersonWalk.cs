using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersonWalk : StateMachineBehaviour {
    public int ActionFrame;
    public string PersonName;
    public string PointsHolderName;
    public string EndTriggerObjName = "";
    public List<string> EndTriggers;
    GameObject person = null;
    GameObject KeyPointsHolder = null;
    public float walkingSpeed = -1;
    protected float frame;
    protected float prevFrame;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        if (GameObject.Find(PersonName) != null)
        {
            if (GameObject.Find(PersonName).GetComponent<MoveToPoint>() != null)
                person = GameObject.Find(PersonName);
        }
        if (GameObject.Find(PointsHolderName) != null)
        {
            if (GameObject.Find(PointsHolderName).GetComponent<ExtraObjectOptions>() != null)
                KeyPointsHolder = GameObject.Find(PointsHolderName);
        }
        if (ActionFrame == 0)
            startWalking();

    }


    void startWalking()
    {
        if (person != null)
        {
            if (KeyPointsHolder != null)
                person.GetComponent<MoveToPoint>().SetKeyPoints(PointsHolderName);
            else
                person.GetComponent<MoveToPoint>().StartWalking();
            if (walkingSpeed > 0f)
            {
                person.GetComponent<MoveToPoint>().speed = walkingSpeed;
            }
            if (EndTriggerObjName != "" && EndTriggers.Count > 0)
            {
                person.GetComponent<MoveToPoint>().SetEndTriggers(EndTriggerObjName, EndTriggers);
            }
        }
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (animator.speed != 0)
        {
            if (PlayerAnimationManager.CompareFrames(frame, prevFrame, ActionFrame))
            {
                startWalking();
            }

            prevFrame = frame;
            frame = stateInfo.normalizedTime * stateInfo.length;
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        frame = 0;
        person = null;
        KeyPointsHolder = null;
    }

    // OnStateMove is called right after Animator.OnAnimatorMove(). Code that processes and affects root motion should be implemented here
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK(). Code that sets up animation IK (inverse kinematics) should be implemented here.
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}
}
