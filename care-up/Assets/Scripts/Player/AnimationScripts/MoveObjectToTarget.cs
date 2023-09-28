using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveObjectToTarget : StateMachineBehaviour
{
    public string targetName;
    public string objectName;
    public bool copyRot = true;
    public int actionFrame = -1;
    protected float frame;
    protected float prevFrame;
    public bool isGhostObject = false;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        frame = 0f;
        prevFrame = 0f;
    }

    void MoveObject()
    {
        GameObject targetObject = GameObject.Find(targetName);
        GameObject _object = GameObject.Find(objectName);
        if (isGhostObject)
        {
            foreach(PickableObject o in GameObject.FindObjectsOfType<PickableObject>())
            {
                if (o.name == objectName && o.mainObject != null)
                {
                    _object = o.gameObject;
                    break;
                }
            }
        }

        if (targetObject != null && _object != null)
        {
            _object.transform.position = targetObject.transform.position;
            if (copyRot)
                _object.transform.rotation = targetObject.transform.rotation;
            if (isGhostObject)
            {
                _object.GetComponent<PickableObject>().mainObject.SavePosition(targetObject.transform.position,
                    targetObject.transform.rotation, true);

            }
        }
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (animator.speed != 0)
        {
            prevFrame = frame;
            frame = stateInfo.normalizedTime * stateInfo.length;

            if (PlayerAnimationManager.CompareFrames(frame, prevFrame, actionFrame))
            {
                MoveObject();
            }
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (actionFrame < 0)
        {
            MoveObject();
        }
    }
}
