using UnityEngine;

public class DecombineSwitchAndDropToTrash : DecombineAndDropToTrash
{
    public int SwitchFrame;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);       
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (PlayerAnimationManager.CompareFrames(frame, prevFrame, SwitchFrame))
        {
            inv.SwapHands();
        }
        base.OnStateUpdate(animator, stateInfo, layerIndex);
    }
}
