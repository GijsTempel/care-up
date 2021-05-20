using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RenameObjectOnFrame : StateMachineBehaviour {


    private float frame;
    private float prevFrame;
    public int actionFrame = 0;
    public string objectName = "";
    public string newName = "";



    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        frame = 0f;
        prevFrame = 0f;

        if (actionFrame == 0 && objectName != "")
        {
            if (GameObject.Find(objectName) != null)
            {
                GameObject.Find(objectName).name = newName;
            }
        }
    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (animator.speed != 0)
        {
            if (PlayerAnimationManager.CompareFrames(frame, prevFrame, actionFrame) && objectName != "")
            {
                if (GameObject.Find(objectName) != null)
                {
                    GameObject.Find(objectName).name = newName;
                }
            }

            prevFrame = frame;
            frame = stateInfo.normalizedTime * stateInfo.length;
        }
    }
}
