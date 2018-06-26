using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Animations;


public class AddClipToAnimator : MonoBehaviour {


#if UNITY_EDITOR
    public AnimatorController animationController;
    public AnimationClip animationClip;
	public int LeftObjectsID = -1;
	public int RightbjectsID = -1;

    [ContextMenu("Run Test")]
    public void Run()
    {
        Motion motion = (Motion)animationClip as Motion;
		//UnityEditor.Animations.AnimatorState lm = animationController.AddMotion(motion, 1);
		UnityEditor.Animations.AnimatorState rm = animationController.AddMotion(motion);
		UnityEditor.Animations.AnimatorState ta;
		ta = GetAnimationClip("Armature|x0040_ipad_closeUp_L_Lib");
		animationController.layers[0].stateMachine.AddAnyStateTransition(rm);
		int tLenght = animationController.layers[0].stateMachine.anyStateTransitions.Length;
		animationController.layers[0].stateMachine.anyStateTransitions[tLenght - 1].AddCondition(AnimatorConditionMode.Equals, 1, "Combine");
                                       
		//if (RightbjectsID > -1)
		//{
		//	ta.transitions[1].AddCondition(AnimatorConditionMode.Equals, LeftObjectsID, "rightID");
		//}
		//ta.transitions[1].AddCondition(AnimatorConditionMode.Equals, 1, "Combine");
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


 