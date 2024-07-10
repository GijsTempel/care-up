using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class PickupObjectOnFrame : StateMachineBehaviour
{
    public int pickupFrame;
    public bool toLeftHand;
    public string objectName;

    protected float frame;
    protected float prevFrame;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        frame = 0f;
        prevFrame = 0f;

        if (pickupFrame == 0)
        {
            PickupObject();

        }
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (animator.speed != 0)
        {
            prevFrame = frame;
            frame = stateInfo.normalizedTime * stateInfo.length;

            if (PlayerAnimationManager.CompareFrames(frame, prevFrame, pickupFrame))
            {
                PickupObject();
            }
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (pickupFrame < 0 || (pickupFrame / 60f > frame))
        {
            PickupObject();
        }
    }
    private void PickupObject()
    {
        PlayerScript player = GameObject.FindObjectOfType<PlayerScript>();
        if (player != null)
        {
            GameObject objInHand = player.GetObjectInHand(toLeftHand);
            if (objInHand != null)
            {
                Debug.LogError("Can not pickup. There is another object in hand");
                return;
            }
            PickableObject obj = null;
            foreach(PickableObject p in GameObject.FindObjectsOfType<PickableObject>())
            {
                if (p.name == objectName)
                {
                    obj = p;
                    break;
                }
            }
            if (obj == null)
            {
                Debug.LogError("Can not find object to pickup");
                return;
            }
            player.ForcePickUpObject(obj, toLeftHand);
        }
    }
}
