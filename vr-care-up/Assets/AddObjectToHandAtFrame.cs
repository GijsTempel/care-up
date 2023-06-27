using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddObjectToHandAtFrame : StateMachineBehaviour
{
    public int showFrame;
    public string objectName;

    protected float frame;
    protected float prevFrame;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        frame = 0f;
        prevFrame = 0f;

        if (showFrame == 0)
        {
            AddObject();
        }
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (animator.speed != 0)
        {
            prevFrame = frame;
            frame = stateInfo.normalizedTime * stateInfo.length;

            if (PlayerAnimationManager.CompareFrames(frame, prevFrame, showFrame))
            {
                AddObject();
            }
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (showFrame < 0 || (showFrame / 60f > frame))
        {
            AddObject();
        }
    }
    private void AddObject()
    {
        PrefabHolder prefabHolder = GameObject.FindObjectOfType<PrefabHolder>();
        if (prefabHolder != null)
        {
            prefabHolder.SpawnObject(objectName);
        }
        else
        {
            Debug.LogError("!Prefab Holder not found");
        }
    }
}
