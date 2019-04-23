using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropAndReturn : StateMachineBehaviour {

    //Temporary from object, and pick up at the end of animation
    PickableObject leftObj = null;
    PickableObject rightObj = null;
    public bool leftHand = false;
    public bool rightHand = false;

    protected HandsInventory inv;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        inv = GameObject.FindObjectOfType<HandsInventory>();
        if (leftHand)
        {
            leftObj = inv.leftHandObject;
            inv.DropLeftObject();
        }
        if (rightHand)
        {
            rightObj = inv.rightHandObject;
            inv.DropRightObject();
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (rightObj != null)
        {
            inv.ForcePickItem(rightObj.gameObject, false, true);
            PlayerAnimationManager.SetHandItem(false, rightObj.gameObject);
        }
        if (leftObj != null)
        {
            inv.ForcePickItem(leftObj.gameObject, true, true);
            PlayerAnimationManager.SetHandItem(true, leftObj.gameObject);
        }
    }
}
