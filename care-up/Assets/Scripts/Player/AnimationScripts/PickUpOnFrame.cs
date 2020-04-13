using UnityEngine;

public class PickUpOnFrame : StateMachineBehaviour
{

    public int actionFrame;
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

        if (actionFrame == 0)
        {
            AddObject();
        }
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (animator.speed != 0)
        {
            prevFrame = frame;
            frame += Time.deltaTime;
        }

        if (PlayerAnimationManager.CompareFrames(frame, prevFrame, actionFrame))
        {
            AddObject();
        }
    }

    public void AddObject()
    {
        if (GameObject.Find(objectName) != null)
        {
            obj = GameObject.Find(objectName);
            inventory.ForcePickItem(obj, hand);
            PlayerAnimationManager.SetHandItem(hand == PlayerAnimationManager.Hand.Left, obj);
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (actionFrame / 60f > frame)
        {
            AddObject();
        }

        frame = 0f;
        obj = null;
    }
}
