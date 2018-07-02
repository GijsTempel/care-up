using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor.Animations;
#endif

public class AddClipToAnimator : MonoBehaviour {


#if UNITY_EDITOR
    public AnimatorController animationController;
	public AnimationClip leftClip;
    public AnimationClip rightClip;
	public int LeftObjectsID = -1;
	public int RightbjectsID = -1;

    [ContextMenu("Run Test")]
    public void Run()
    {
        if (leftClip != null) 
        { 
            Motion LeftMotion = (Motion)leftClip as Motion;
            AddActionToMachine(0, "Combine Animations/Injection Scene", LeftMotion, LeftObjectsID, RightbjectsID, "Combine");
            AddActionToMachine(1, "Combine Animations/Injection Scene", LeftMotion, LeftObjectsID, RightbjectsID, "S Combine");
	    }
        if (rightClip != null)
        {
            Motion RightMotion = (Motion)rightClip as Motion;
            AddActionToMachine(0, "Combine Animations/Injection Scene", RightMotion, RightbjectsID, LeftObjectsID, "Combine");
            AddActionToMachine(1, "Combine Animations/Injection Scene", RightMotion, RightbjectsID, LeftObjectsID, "S Combine");
        }
    }

    void AddActionToMachine(int layer, string machineName, Motion clip, int leftID, int rightID, string trigger)
    {
        AnimatorStateMachine am = FindMachine(animationController.layers[layer].stateMachine, machineName);
        UnityEditor.Animations.AnimatorState lm;
        lm = am.AddState(clip.name);
        lm.motion = clip;
        AnimatorStateMachine bm = animationController.layers[layer].stateMachine;
        bm.AddAnyStateTransition(lm);
        int l = bm.anyStateTransitions.Length;
        bm.anyStateTransitions[l - 1].AddCondition(AnimatorConditionMode.If, 1, trigger);
        if (leftID > -1)
            bm.anyStateTransitions[l - 1].AddCondition(AnimatorConditionMode.Equals, leftID, "leftID");
        if (rightID > -1)
            bm.anyStateTransitions[l - 1].AddCondition(AnimatorConditionMode.Equals, rightID, "rightID");
        lm.AddTransition(animationController.layers[layer].stateMachine);
        lm.transitions[lm.transitions.Length - 1].hasExitTime = true;
    }
    
    AnimatorStateMachine FindMachine(AnimatorStateMachine machine, string addr)
    {
        string n = addr;
        bool lastElement = true;
        string nextAddr = "";
        int addrLen = addr.Split('/').Length;
        if (addrLen > 1)
        {
            n = addr.Split('/')[0];
            lastElement = false;
            for (int i = 1; i < addrLen; i++)
            {
                nextAddr += addr.Split('/')[i];
                if (i < (addrLen - 1))
                    nextAddr += "/";
            }
        }
        foreach (ChildAnimatorStateMachine state in machine.stateMachines)
        {
            if (state.stateMachine.name == n)
            {
                AnimatorStateMachine sm = state.stateMachine;
                if (!lastElement)
                    return (FindMachine(sm, nextAddr));
                return sm;
            }
        }
        return null;
    }
    
    

    public UnityEditor.Animations.AnimatorState GetAnimationClip(string name)
    {
        if (!animationController) return null;
        
		foreach (UnityEditor.Animations.ChildAnimatorState state in animationController.layers[0].stateMachine.states)
        {
			if (state.state.name == name)
            {
				return state.state;
            }
        }
        return null; // no clip by that name
    }

#endif
}


 