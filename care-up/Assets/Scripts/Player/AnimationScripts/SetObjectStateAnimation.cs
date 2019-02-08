using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetObjectStateAnimation : StateMachineBehaviour
{

    public enum Hand
    {
        No,
        Left,
        Right
    };

    public enum AnimationType
    {
        None,
        Lie,
        Hold
    };

    public enum FollowStates
    {
        None,
        Fallow,
        NotFallow
    };



    public string ObjectName = "";
    public Hand HandToSwitch;
    public AnimationType AnimationToChange;
    public FollowStates ToFollowHoldingHand;
    public string NewAnimationName = "";

    protected float frame = 0f;
    protected float prevFrame = 0f;
    public int ActionFrame;


    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (ActionFrame == 0)
        {
            set_set();
        }
        frame = 0f;
        prevFrame = 0f;
    }



    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (animator.speed != 0)
        {
            if (PlayerAnimationManager.CompareFrames(frame, prevFrame, ActionFrame))
            {
                set_set();
               
            }

            prevFrame = frame;
            frame += Time.deltaTime;
        }
    }

    void set_set()
    {
        if (GameObject.Find(ObjectName) != null)
        {
           
            if (GameObject.Find(ObjectName).GetComponent<ObjectStateManager>() != null)
            {
                ObjectStateManager obj = GameObject.Find(ObjectName).GetComponent<ObjectStateManager>();


                if (ToFollowHoldingHand != SetObjectStateAnimation.FollowStates.None)
                {
                    obj.followHoldingHand = ToFollowHoldingHand == SetObjectStateAnimation.FollowStates.Fallow;
                }

                if (HandToSwitch == SetObjectStateAnimation.Hand.Left)
                {
                    obj.follow_left = true;
                }
                else if (HandToSwitch == SetObjectStateAnimation.Hand.Right)
                {
                    obj.follow_left = false;
                }

                if (NewAnimationName != "" && AnimationToChange != SetObjectStateAnimation.AnimationType.None)
                {

                    if (AnimationToChange == SetObjectStateAnimation.AnimationType.Lie)
                    {
                        obj.SetAnimation(true, NewAnimationName);
                    }
                    else
                    {
                        obj.SetAnimation(false, NewAnimationName);
                    }

                }
            }
        }
    }


    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }

}
