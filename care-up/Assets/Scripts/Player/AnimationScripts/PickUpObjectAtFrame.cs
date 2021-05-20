using UnityEngine;

public class PickUpObjectAtFrame : StateMachineBehaviour
{
    public int pickFrame;
    public string objectName;
    public PlayerAnimationManager.Hand hand;

    protected float frame = 0f;
    protected float prevFrame;

    HandsInventory inventory;
    GameObject obj = null;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);

        inventory = GameObject.FindObjectOfType<HandsInventory>();
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (animator.speed != 0)
        {
            prevFrame = frame;
            frame = stateInfo.normalizedTime * stateInfo.length;

            if (PlayerAnimationManager.CompareFrames(frame, prevFrame, pickFrame))
            {
                obj = GameObject.Find(objectName);
                if (obj != null)
                {
                    inventory.ForcePickItem(obj, hand, true);
                    PlayerAnimationManager.SetHandItem(hand == PlayerAnimationManager.Hand.Left, obj);
                }
            }
        }
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (pickFrame / 60f > frame)
        {
            obj = GameObject.Find(objectName);
            if (obj != null)
            {
                inventory.ForcePickItem(obj, hand, true);
                PlayerAnimationManager.SetHandItem(hand == PlayerAnimationManager.Hand.Left, obj);
            }
        }
    }
}
