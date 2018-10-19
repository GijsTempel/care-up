using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReplacePrefabInHand : StateMachineBehaviour {
    public int swapFrame;
    public string swapObjName;
    public bool leftHand = true;

    protected float frame;
    protected float prevFrame;
    HandsInventory handsInventory;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        handsInventory = GameObject.FindObjectOfType<HandsInventory>();

    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

        if (PlayerAnimationManager.CompareFrames(frame, prevFrame, swapFrame) && swapObjName != "")
        {
            handsInventory.ReplaceHandObject(leftHand, swapObjName);
        }

        if (animator.speed != 0)
        {
            prevFrame = frame;
            frame += Time.deltaTime;
        }
    }

    }
