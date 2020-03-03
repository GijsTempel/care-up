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

    public string objectName = "";
    public Hand handToSwitch;
    public AnimationType animationToChange;
    public FollowStates toFollowHoldingHand;
    public string newAnimationName = "";
    public int actionFrame;

    protected float frame = 0f;
    protected float prevFrame = 0f;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (actionFrame == 0)
        {
            SetStateAnimation();
        }
        frame = 0f;
        prevFrame = 0f;
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (animator.speed != 0)
        {
            if (PlayerAnimationManager.CompareFrames(frame, prevFrame, actionFrame))
            {
                SetStateAnimation();
            }

            prevFrame = frame;
            frame += Time.deltaTime;
        }
    }

    void SetStateAnimation()
    {
        if (GameObject.Find(objectName) != null)
        {
            if (GameObject.Find(objectName).GetComponent<ObjectStateManager>() != null)
            {
                ObjectStateManager obj = GameObject.Find(objectName).GetComponent<ObjectStateManager>();

                if (toFollowHoldingHand != FollowStates.None)
                {
                    obj.followHoldingHand = toFollowHoldingHand == FollowStates.Fallow;
                }

                if (handToSwitch == Hand.Left)
                {
                    obj.followLeft = true;
                }
                else if (handToSwitch == Hand.Right)
                {
                    obj.followLeft = false;
                }

                if (newAnimationName != "" && animationToChange != AnimationType.None)
                {
                    if (animationToChange == AnimationType.Lie)
                    {
                        obj.SetAnimation(true, newAnimationName);
                    }
                    else
                    {
                        obj.SetAnimation(false, newAnimationName);
                    }
                }
            }
        }
    }
}
