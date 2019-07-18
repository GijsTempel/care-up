using System.Collections;
using System.Collections.Generic;
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
        //Debug.Log(frame);
        if (animator.speed != 0)
        {
            if (PlayerAnimationManager.CompareFrames(frame, prevFrame, pickFrame))
            {
                obj = GameObject.Find(objectName);
                if (obj != null)
                {
                    inventory.ForcePickItem(obj, hand, true);
                    PlayerAnimationManager.SetHandItem(hand == PlayerAnimationManager.Hand.Left, obj);
                }
            }

  
            prevFrame = frame;
            frame += Time.deltaTime;
        }
    }
}
