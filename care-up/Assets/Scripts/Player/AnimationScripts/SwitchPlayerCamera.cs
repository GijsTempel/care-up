using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchPlayerCamera : StateMachineBehaviour {
    public string cameraName;
    public int actionFrame = 0;
    protected float frame;
    protected float prevFrame;
    PlayerScript player;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        player = GameObject.FindObjectOfType<PlayerScript>();

        if (actionFrame == 0)
        {
            player.SwitchCamera(cameraName);
        }
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (animator.speed != 0)
        {
            if (PlayerAnimationManager.CompareFrames(frame, prevFrame, actionFrame))
            {
                player.SwitchCamera(cameraName);
            }
            prevFrame = frame;
            frame += Time.deltaTime;
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (actionFrame < 0)
        {
            player.SwitchCamera(cameraName);
        }
        frame = 0;
        prevFrame = 0;
    }

}
