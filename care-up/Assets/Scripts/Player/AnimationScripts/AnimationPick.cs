using UnityEngine;

public class AnimationPick : StateMachineBehaviour
{
    public bool hand;
    private float frame;
    private float prevFrame;
    private HandsInventory inv;
    bool isHolding = false;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        inv = GameObject.Find("GameLogic").GetComponent<HandsInventory>();

        frame = 0f;
        prevFrame = 0f;

        RobotManager.SetUITriggerActive(false);
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (animator.speed != 0)
        {
            if (PlayerAnimationManager.CompareFrames(frame, prevFrame, 50))
            {
                //Debug.Log("targetFrame: " + 50 / 60f + ". currentFrame : " + frame + ". previousFrame" + prevFrame);
                isHolding = true;
                inv.SetHold(hand);
            }

            inv.ToggleControls(true);
            PlayerScript.actionsLocked = true;

            prevFrame = frame;
            frame += Time.deltaTime;
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!isHolding)
            inv.SetHold(hand);

        inv.ToggleControls(false);
        PlayerScript.actionsLocked = false;

        if (GameObject.FindObjectOfType<TutorialManager>() == null ||
            GameObject.FindObjectOfType<Tutorial_UI>() != null ||
            GameObject.FindObjectOfType<Tutorial_Theory>() != null)
        {
            RobotManager.SetUITriggerActive(true);
        }
    }
}
