using UnityEngine;

public class UpdateWalkToGroups : StateMachineBehaviour
{
    public int actionFrame = 0;
    protected float frame;
    protected float prevFrame;
    GameUI gameUI;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        gameUI = GameObject.FindObjectOfType<GameUI>();

        if (actionFrame == 0)
        {
            _Action();
        }
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (animator.speed != 0)
        {
            prevFrame = frame;
            frame = stateInfo.normalizedTime * stateInfo.length;

            if (PlayerAnimationManager.CompareFrames(frame, prevFrame, actionFrame))
            {
                _Action();
            }
        }
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (actionFrame / 60f > frame)
        {
            _Action();
        }
    }

    void _Action()
    {
        gameUI.UpdateWalkToGroupButtons();
        ActionManager.UpdateRequirements();
        if (GameObject.FindObjectOfType<AnimatedFingerHint>() != null)
        {
            GameObject.FindObjectOfType<AnimatedFingerHint>().DelayedAction(1f);
        }
    }
}
