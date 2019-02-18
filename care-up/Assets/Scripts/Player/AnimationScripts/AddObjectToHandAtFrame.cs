using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddObjectToHandAtFrame : StateMachineBehaviour
{
    public int addFrame;
    public string objectName;
    public PlayerAnimationManager.Hand hand;

    protected float frame;
    protected float prevFrame;

    HandsInventory inventory;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);

        inventory = GameObject.FindObjectOfType<HandsInventory>();
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (PlayerAnimationManager.CompareFrames(frame, prevFrame, addFrame))
        {
            GameObject obj = inventory.CreateObjectByName(objectName, Vector3.zero);
            inventory.ForcePickItem(objectName, hand);
        
            PlayerAnimationManager.SetHandItem(hand == PlayerAnimationManager.Hand.Left, obj);
        }

        if (animator.speed != 0)
        {
            prevFrame = frame;
            frame += Time.deltaTime;
        }
    }
}
