using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropFromHandAndReturn : StateMachineBehaviour
{
    //Temporary from object, and pick up at the end of animation
    public string ReplaceThisWith = "DropAndReturn";
    PickableObject obj = null;
    public PlayerAnimationManager.Hand hand;

    protected HandsInventory inv;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        inv = GameObject.FindObjectOfType<HandsInventory>();
        if (hand == PlayerAnimationManager.Hand.Left)
        {
            obj = inv.leftHandObject;
            inv.DropLeftObject();
        }
        else
        {
            obj = inv.rightHandObject;
            inv.DropRightObject();
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (obj != null)
        {
            if (inv.PickItem(obj, hand ))
                obj.CreateGhostObject();
        }
    }
}
