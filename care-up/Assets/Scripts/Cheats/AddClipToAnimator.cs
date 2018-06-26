using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;


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
		if (leftClip) { 
			Motion LeftMotion = (Motion)leftClip as Motion;
   //         UnityEditor.Animations.AnimatorState lm;
   //         lm = animationController.layers[0].stateMachine.stateMachines[3].stateMachine.stateMachines[0].stateMachine.AddState(LeftMotion.name);
   //         lm.motion = LeftMotion;
			//animationController.layers[0].stateMachine.AddAnyStateTransition(lm);
   //         int tLenght = animationController.layers[0].stateMachine.anyStateTransitions.Length;
   //         animationController.layers[0].stateMachine.anyStateTransitions[tLenght - 1].AddCondition(AnimatorConditionMode.Equals, 1, "Combine");
			//if (LeftObjectsID > -1)
			//{
			//	animationController.layers[0].stateMachine.anyStateTransitions[tLenght - 1].AddCondition(AnimatorConditionMode.Equals, LeftObjectsID, "leftID");
			//}
			//if (RightbjectsID > -1)
    //        {
				//animationController.layers[0].stateMachine.anyStateTransitions[tLenght - 1].AddCondition(AnimatorConditionMode.Equals, RightbjectsID, "rightID");
            //}
            
            
            
            UnityEditor.Animations.AnimatorState rm;
            //rm = animationController.layers[1].stateMachine.AddState(LeftMotion.name);
            //print(animationController.layers[1].stateMachine.stateMachines[0].stateMachine.name);

            //animationController.layers[1].stateMachine.AddAnyStateTransition(rm);
            //int ttLenght = animationController.layers[1].stateMachine.anyStateTransitions.Length;
            //animationController.layers[1].stateMachine.anyStateTransitions[ttLenght - 1].AddCondition(AnimatorConditionMode.Equals, 1, "S Combine");
            //if (LeftObjectsID > -1)
            //{
            //    animationController.layers[1].stateMachine.anyStateTransitions[ttLenght - 1].AddCondition(AnimatorConditionMode.Equals, LeftObjectsID, "rightID");
            //}
            //if (RightbjectsID > -1)
            //{
            //    animationController.layers[1].stateMachine.anyStateTransitions[ttLenght - 1].AddCondition(AnimatorConditionMode.Equals, RightbjectsID, "leftID");
            //}
            AnimatorStateMachine am = FindMachine(animationController.layers[1].stateMachine, "Combine Animations/Injection Scene/New StateMachine");
            if (am != null)
                print(am.name);
				
		}

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
            print(nextAddr);
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


 