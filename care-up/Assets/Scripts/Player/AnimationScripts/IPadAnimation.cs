using UnityEngine;

public class IPadAnimation : StateMachineBehaviour
{
    public int takeFrame;
    public int dropFrame;

    public bool openingAnimation = false;

    protected float frame;
    protected float prevFrame;

    protected HandsInventory inv;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        inv = GameObject.Find("GameLogic").GetComponent<HandsInventory>();

        frame = 0f;
        prevFrame = 0f;

        PlayerScript.actionsLocked = true;
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (animator.speed != 0)
        {
            if (openingAnimation)
            {
                if (PlayerAnimationManager.CompareFrames(frame, prevFrame, takeFrame))
                {
                    // pick iPad in left hand
                    inv.ForcePickItem("ipad", false);
                }
            }
            else
            {
                if (PlayerAnimationManager.CompareFrames(frame, prevFrame, dropFrame))
                {
                    inv.DropRightObject();
                }

                PlayerScript.actionsLocked = true;
            }

            prevFrame = frame;
            frame = stateInfo.normalizedTime * stateInfo.length;
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!openingAnimation)
        {
            GameObject.FindObjectOfType<PlayerScript>().PickItemsBackAfterRobotUI();

            RobotManager.SetUITriggerActive(true);
        }

        PlayerScript.actionsLocked = false;
    }
}
