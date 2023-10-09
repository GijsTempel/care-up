using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAudioOnFrame : StateMachineBehaviour
{
    public string controlObjectName = "";
    public string audioObjectName;
    public int audioFrame;
    protected float frame = 0f;
    protected float prevFrame = 0f;


    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {     
        if (PlayerAnimationManager.CompareFrames(frame, prevFrame, audioFrame))
        {
            if (controlObjectName == "")
            {
                if (GameObject.Find(audioObjectName) != null)
                {
                    GameObject.Find(audioObjectName).GetComponent<AudioSource>().Play();
                }
            }
            else
            {
                GameObject controlObject = GameObject.Find(controlObjectName);
                if (controlObject != null)
                {
                    if (controlObject.GetComponent<ShowHideObjects>() != null)
                    {
                        foreach(GameObject g in controlObject.GetComponent<ShowHideObjects>().hidenObjects)
                        {
                            if (g.name == audioObjectName && g.GetComponent<AudioSource>() != null)
                            {
                                g.GetComponent<AudioSource>().Play();
                                break;
                            }
                        }
                    }
                }
            }
        }

        if (animator.speed != 0)
        {
            prevFrame = frame;
            frame = stateInfo.normalizedTime * stateInfo.length;
        }
    }
}
