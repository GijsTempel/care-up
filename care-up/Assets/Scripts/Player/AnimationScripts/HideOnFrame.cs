using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideOnFrame : StateMachineBehaviour
{

    public int hideFrame;
	public string hideObjName;

    protected float frame;
    protected float prevFrame;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

        
    }

	bool CompareFrames(float currentFrame, float previousFrame, int compareFrame)
    {
        float targetFrame = compareFrame / 60f; // 60fps
        return (currentFrame >= targetFrame && previousFrame < targetFrame);
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

		if (CompareFrames(frame, prevFrame, hideFrame) && hideObjName != "")
        {
			Debug.Log("CompareFrames(frame, prevFrame, hideFrame)");
			if (GameObject.Find(hideObjName))
			{
				Debug.Log(GameObject.Find(hideObjName).gameObject.name);
				GameObject.Find(hideObjName).gameObject.SetActive(false);
    			}
        }
   
        prevFrame = frame;
        frame += Time.deltaTime;
   
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
  
    }

}
