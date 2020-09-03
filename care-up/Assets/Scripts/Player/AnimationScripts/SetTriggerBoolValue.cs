using UnityEngine;

public class SetTriggerBoolValue : StateMachineBehaviour
{
    public string TriggerName;
    public bool value = false;
    public int actionFrame = 0;
    protected float frame;
    protected float prevFrame;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (actionFrame == 0)
        {
            animator.SetBool(TriggerName, value);
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
                animator.SetBool(TriggerName, value);
            }
        }
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (actionFrame / 60f > frame)
        {
            animator.SetBool(TriggerName, value);
        }        
    }
}
