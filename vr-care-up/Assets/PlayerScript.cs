using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    public AnimHandsTransform animHandsTransform;
    private Animator animHandsAnimator;
    public string currentWTGName = "";
    private GameObject leftHandObject;
    private GameObject rightHandObject;
    private HandPoseControl leftHandPoseControl;
    private HandPoseControl rightHandPoseControl;

    
    public void TriggerAction(string triggerName)
    {
        if (leftHandPoseControl == null || rightHandPoseControl == null)
            return;
        leftHandPoseControl.SetupCopyAnimationData();
        rightHandPoseControl.SetupCopyAnimationData();
        animHandsAnimator = animHandsTransform.animator;
        animHandsAnimator.SetTrigger(triggerName);

    }
    public void AddHandPoseControl(HandPoseControl control, bool isRightHand)
    {
        if (isRightHand)
            rightHandPoseControl = control;
        else
            leftHandPoseControl = control;
        string sideName = "Left:";
        if (isRightHand)
            sideName = "Right:";
        Debug.Log("@HandModel_" + sideName + control.gameObject.name);
    }

    private void Start()
    {
        animHandsAnimator = animHandsTransform.transform.GetComponentInChildren<Animator>();
    }

    public void SetObjectInHand(GameObject obj, bool isRightHand = true, bool isPickUp = true)
    {
        string objName = "";
        if (obj != null)
            objName = obj.name;

        if (isRightHand)
        {
            if (isPickUp)
            {
                rightHandObject = obj;
            }
            else
            {
                if (rightHandObject == obj)
                {
                    rightHandObject = null;
                    objName = "";
                }
            }
            Debug.Log("@RightHandObj:" + objName);
        }
        else
        {
            if (isPickUp)
            {
                leftHandObject = obj;
            }
            else
            {
                if (leftHandObject == obj)
                {
                    leftHandObject = null;
                    objName = "";
                }
            }

            Debug.Log("@LeftHandObj:" + objName);
        }
    }

    public void UpdateWalkToGroup(string WTGName)
    {
        currentWTGName = WTGName;
    }

}
