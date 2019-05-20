using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateWalkToGroups : StateMachineBehaviour {

    public int actionFrame = 0;
    protected float frame;
    protected float prevFrame;
    GameUI gameUI;


    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        gameUI = GameObject.FindObjectOfType<GameUI>();
        if (actionFrame == 0)
        {
            gameUI.UpdateWalkToGroupButtons();
        }
    }


    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (animator.speed != 0)
        {
            if (PlayerAnimationManager.CompareFrames(frame, prevFrame, actionFrame))
            {
                gameUI.UpdateWalkToGroupButtons();
            }
            prevFrame = frame;
            frame += Time.deltaTime;
        }

    }

}
