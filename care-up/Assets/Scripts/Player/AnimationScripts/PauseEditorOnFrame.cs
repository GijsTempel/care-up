using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PauseEditorOnFrame : StateMachineBehaviour
{
    public int actionFrame = 0;
    protected float frame;
    protected float prevFrame;


    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

        if (actionFrame == 0)
        {
            
                #if (UNITY_EDITOR)
                    EditorApplication.isPaused = true;
                #endif 

        }

    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (animator.speed != 0)
        {
            if (PlayerAnimationManager.CompareFrames(frame, prevFrame, actionFrame))
            {
                if (GameObject.FindObjectOfType<PlayerAnimationManager>() != null)
                {
                    #if (UNITY_EDITOR)
                     EditorApplication.isPaused = true;
                    #endif
                }

            }
            prevFrame = frame;
            frame = stateInfo.normalizedTime * stateInfo.length;
        }




            /*frame = animator.GetCurrentAnimatorClipInfo(0)[0].clip.length * (animator.GetCurrentAnimatorStateInfo(0).normalizedTime % 1) * animator.GetCurrentAnimatorClipInfo(0)[0].clip.frameRate;
            gameUI.debugSS = frame.ToString(); */

        
    }
}
