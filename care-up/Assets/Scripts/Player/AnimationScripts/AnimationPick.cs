using UnityEngine;

public class AnimationPick : StateMachineBehaviour
{
    public bool hand;
    private float frame;
    private float prevFrame;
    private HandsInventory inv;
    private int pickFrame = 50;

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
            prevFrame = frame;
            frame += Time.deltaTime;

            if (PlayerAnimationManager.CompareFrames(frame, prevFrame, pickFrame))
            {
                inv.SetHold(hand);
            }

            inv.ToggleControls(true);
            PlayerScript.actionsLocked = true;
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (pickFrame / 60f > frame)
        {
            inv.SetHold(hand);
            Debug.LogWarning("OnStateExit action. Low frame rate.");
        }

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
