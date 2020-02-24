using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowFrame : StateMachineBehaviour {

    private float frame;
    private float prevFrame;
    GameUI gameUI;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        gameUI = GameObject.FindObjectOfType<GameUI>();
        frame = 0f;
        prevFrame = 0f;
  
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (animator.speed != 0)
        {
            prevFrame = frame;
            frame += Time.deltaTime;
            gameUI.debugSS = ((int)(frame * 60)).ToString();

            /*frame = animator.GetCurrentAnimatorClipInfo(0)[0].clip.length * (animator.GetCurrentAnimatorStateInfo(0).normalizedTime % 1) * animator.GetCurrentAnimatorClipInfo(0)[0].clip.frameRate;
            gameUI.debugSS = frame.ToString(); */
            
            
            
        }
    }
}
