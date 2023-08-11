using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RenameObjectAtFrame : StateMachineBehaviour
{
    public int renameFrame;
    public string ObjName;
    public string ObjNewName;

    protected float frame;
    protected float prevFrame;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        frame = 0f;
        prevFrame = 0f;

        if (renameFrame == 0)
        {
            FindToRenameObject();
        }
    }

    void RenameObject(GameObject obj)
    {
        obj.name = ObjNewName;
    }
   
    void FindToRenameObject()
    {
        PlayerScript player = GameObject.FindObjectOfType<PlayerScript>();
        if (player != null)
        {
            GameObject leftObj = player.GetObjectInHand(true);
            GameObject rightObj = player.GetObjectInHand(false);

            if (leftObj != null && leftObj.name == ObjName)
                RenameObject(leftObj);
            else if (rightObj != null && rightObj.name == ObjName)
                RenameObject(rightObj);
            
        }
        else
        {
            GameObject obj = GameObject.Find(ObjName);
            if (obj != null)
                RenameObject(obj);
        }
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (animator.speed != 0)
        {
            prevFrame = frame;
            frame = stateInfo.normalizedTime * stateInfo.length;

            if (PlayerAnimationManager.CompareFrames(frame, prevFrame, renameFrame))
            {
                FindToRenameObject();
            }
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (renameFrame < 0 || (renameFrame / 60f > frame))
        {
            FindToRenameObject();
        }
    }
}
