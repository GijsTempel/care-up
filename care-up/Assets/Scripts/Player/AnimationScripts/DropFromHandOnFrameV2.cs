using UnityEngine;

public class DropFromHandOnFrameV2 : StateMachineBehaviour
{
    public enum Hand
    {
        Left, 
        Right,
    };

    public Hand hand;
    public int dropFrame;

    public string dropToObject;
    public bool dropObjectAsChild;

    protected HandsInventory inv;

    protected float frame = 0;
    protected float prevFrame = 0;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        inv = GameObject.Find("GameLogic").GetComponent<HandsInventory>();


        if (frame == 0)
        {
            if (hand == Hand.Left)
            {
                inv.DropLeftObject();
            }
            else
            {
                inv.DropRightObject();
            }
        }
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (inv == null)
        {
            Debug.LogError("No inventory found, aborting function");
            return;
        }

        if (animator.speed != 0)
        {
            prevFrame = frame;
            frame = stateInfo.normalizedTime * stateInfo.length;

            if (PlayerAnimationManager.CompareFrames(frame, prevFrame, dropFrame))
            {
                if (hand == Hand.Left)
                {
                    inv.DropLeftObject();
                }
                else
                {
                    inv.DropRightObject();
                }
            }  
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (frame / 60f > frame)
        {
            if (hand == Hand.Left)
            {
                inv.DropLeftObject();
            }
            else
            {
                inv.DropRightObject();
            }
        }
    }
}
