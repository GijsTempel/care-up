using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAudioOnFrame : StateMachineBehaviour {

    public string audioFileName;
    public int audioFrame;
    public float volume = 1f;

    protected float frame = 0f;
    protected float prevFrame = 0f;
    public bool moveWithCamera = false;

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {     
        if (PlayerAnimationManager.CompareFrames(frame, prevFrame, audioFrame))
        {
            AudioClip clip = Resources.Load<AudioClip>("Audio/" + audioFileName);
            if (moveWithCamera)
            {
                GameObject Narrator = GameObject.FindObjectOfType<Narrator>().gameObject;
                AudioSource audioSource = Narrator.AddComponent<AudioSource>();
                audioSource.clip = clip;
                audioSource.Play();
            }
            else
            {
                AudioSource.PlayClipAtPoint(clip, Camera.main.transform.position, volume);
            }
        }

        if (animator.speed != 0)
        {
            prevFrame = frame;
            frame = stateInfo.normalizedTime * stateInfo.length;
        }
    }

}
