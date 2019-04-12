using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cinematic : StateMachineBehaviour
{
    public string target;
    public bool resetCamera = false;
    public float maxDist = 1.8f;
    protected CameraMode mode;
    bool teleport = false;
    int teleportStartFrame;
    protected float frame;

    protected float prevFrame;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        mode = GameObject.FindObjectOfType<CameraMode>();
        if (GameObject.Find(target) != null)
        {
            if (GameObject.Find(target).transform.Find("CinematicTarget") != null)
            {
                float dist = Vector3.Distance(GameObject.Find(target).transform.Find("CinematicTarget").transform.position,
                    animator.transform.position);
                if (dist > maxDist)
                {
                    //teleport = true;
                    //animator.SetTrigger("close_eyes");
                    //teleportStartFrame = (int)(frame * 60) + 10;
                }
            }
        }
        if (!teleport)
        {
            mode.SetCinematicMode(GameObject.Find(target).transform);
        }
        if (resetCamera)
        {
            Camera.main.transform.localRotation = Quaternion.Euler(Vector3.zero);
        }
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (animator.speed != 0)
        {
            if (teleport)
            {
                if (PlayerAnimationManager.CompareFrames(frame, prevFrame, teleportStartFrame))
                {
                    mode.Teleport(GameObject.Find(target).transform);
                }

                if (PlayerAnimationManager.CompareFrames(frame, prevFrame, teleportStartFrame + 10))
                { 
                    animator.SetTrigger("open_eyes");
                }
            }
            prevFrame = frame;
            frame += Time.deltaTime;
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!teleport)
            mode.animationEnded = true;
        teleport = false;
        frame = 0;
        prevFrame = 0;
    }

   
}
