using UnityEngine;

public class ReplacePrefabInHand : StateMachineBehaviour
{
    public int swapFrame;
    public string swapObjName;
    public bool leftHand = true;
    public string GhostObjectTarget = "";

    protected float frame;
    protected float prevFrame;
    HandsInventory handsInventory;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        handsInventory = GameObject.FindObjectOfType<HandsInventory>();
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (animator.speed != 0)
        {
            if (PlayerAnimationManager.CompareFrames(frame, prevFrame, swapFrame) && swapObjName != "")
            {
                handsInventory.ReplaceHandObject(leftHand, swapObjName, GhostObjectTarget);
            }

            prevFrame = frame;
            frame = stateInfo.normalizedTime * stateInfo.length;
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        frame = 0f;
    }
}
