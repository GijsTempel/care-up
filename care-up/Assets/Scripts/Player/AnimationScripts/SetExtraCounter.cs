﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetExtraCounter : StateMachineBehaviour {

    public string valueName = "";
    string vname = "extra";
    public int value = 0;
    public int actionFrame = 0;
    protected float frame;
    protected float prevFrame;
    public int increase = 0;


    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        vname = "extra";
        if (valueName != "")
            vname = valueName;
        if (actionFrame == 0)
        {
            animator.SetInteger(vname, value);
        }
    }


    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (animator.speed != 0)
        {
            if (PlayerAnimationManager.CompareFrames(frame, prevFrame, actionFrame))
            {
                if (increase != 0)
                {
                    int new_value = animator.GetInteger(vname) + increase;
                    animator.SetInteger(vname, new_value);
                }
                else
                {
                    animator.SetInteger(vname, value);
                }
            }
            prevFrame = frame;
            frame = stateInfo.normalizedTime * stateInfo.length;
        }
    }
}
