using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseEyes : StateMachineBehaviour {
    public bool toClose = true;
    public int actionFrame = 0;
    protected float frame;
    protected float prevFrame;


    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (actionFrame == 0)
        {
            Animator anim = GameObject.FindObjectOfType<PlayerAnimationManager>().GetComponent<Animator>();

            if (toClose)
            {
                anim.SetTrigger("close_eyes");
            }
            else
            {
                anim.SetTrigger("open_eyes");
            }
        }
    }


    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
    

        if (animator.speed != 0)
        {
            if (PlayerAnimationManager.CompareFrames(frame, prevFrame, actionFrame))
            {
                if (GameObject.FindObjectOfType<PlayerAnimationManager>() != null)
                {
                    Animator anim = GameObject.FindObjectOfType<PlayerAnimationManager>().GetComponent<Animator>();
                    if (toClose)
                    {
                        anim.SetTrigger("close_eyes");
                    }
                    else
                    {
                        anim.SetTrigger("open_eyes");
                    }
                }

            }
            prevFrame = frame;
            frame += Time.deltaTime;
        }

    }




}
