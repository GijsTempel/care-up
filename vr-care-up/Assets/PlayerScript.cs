using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    public AnimHandsTransform animHandsTransform;
    private Animator animHandsAnimator;
    public string currentWTGName = "";
    private HandPoseControl leftHandPoseControl;
    private HandPoseControl rightHandPoseControl;
    private Transform mainCameraTransform;
    public Animation fadeAnimation;

    private HandPresence leftHandPresence;
    private HandPresence rightHandPresence;


    public void DropFromHand(bool leftHand)
    {
        if (leftHand && leftHandPresence != null)
            leftHandPresence.DropObjectFromHand();
        else if (!leftHand && rightHandPresence != null)
            rightHandPresence.DropObjectFromHand();
    }
    public GameObject GetObjectInHand(bool isLeft)
    {
        if (isLeft && leftHandPresence != null)
            return leftHandPresence.GetObjectInHand();
        if (!isLeft && rightHandPresence != null)
            return rightHandPresence.GetObjectInHand();
        return null;
    }

    public void AddHandPresence(bool isLeft, HandPresence hp)
    {
        if (isLeft)
            leftHandPresence = hp;
        else
            rightHandPresence = hp;
    }

    public ActionTrigger.TriggerHandAction GetCurrentHandPose(bool isLeftHand)
    {
        if (isLeftHand && leftHandPresence != null)
            return leftHandPresence.GetCurrentHandPose();
        if (!isLeftHand && rightHandPresence != null)
            return rightHandPresence.GetCurrentHandPose();
        return ActionTrigger.TriggerHandAction.None;
    }
    public bool TriggerAction(string triggerName, GameObject cinematicTarget = null)
    {
        Debug.Log("@$$$" + name + "TriggerAction:" + triggerName + Random.Range(0, 9999).ToString());
        if (leftHandPoseControl == null || rightHandPoseControl == null)
            return false;
        if (IsInCopyAnimationState())
            return false;
        if (leftHandPoseControl.handPoseMode != HandPoseControl.HandPoseMode.Default ||
            rightHandPoseControl.handPoseMode != HandPoseControl.HandPoseMode.Default )
            return false;

        if (cinematicTarget != null)
        {
            // fadeAnimation.Play();
            Vector3 zVec = new Vector3(1f, 0f, 1f);
            animHandsTransform.fallowVRCamera = false;
            animHandsTransform.transform.position = cinematicTarget.transform.position;
            animHandsTransform.transform.rotation = cinematicTarget.transform.rotation;
            Vector3 cinPos = cinematicTarget.transform.position;
            Vector3 camPos = mainCameraTransform.position;
            Vector3 pos = transform.position;
            Vector3 newPos = cinPos - (camPos - pos);
            newPos.y = transform.position.y;
            transform.position = newPos;

            Vector3 cinForward = cinematicTarget.transform.forward;
            Vector3 camForward = Vector3.Scale(mainCameraTransform.forward, zVec).normalized;
            float rotAngle = Vector3.Angle(cinForward, camForward);
            // transform.RotateAround(transform.position, Vector3.up, rotAngle);
        }
        leftHandPoseControl.SetupCopyAnimationData();
        rightHandPoseControl.SetupCopyAnimationData();
        animHandsAnimator = animHandsTransform.animator;
        animHandsAnimator.SetTrigger(triggerName);
        return true;
    }

    public bool IsInCopyAnimationState()
    {
        if (leftHandPoseControl != null)
        {
            if (leftHandPoseControl.handPoseMode == HandPoseControl.HandPoseMode.CopyAnimIn ||
                leftHandPoseControl.handPoseMode == HandPoseControl.HandPoseMode.CopyAnimOut)
                return true;
        }
        if (rightHandPoseControl != null)
        {
            if (rightHandPoseControl.handPoseMode == HandPoseControl.HandPoseMode.CopyAnimIn ||
                rightHandPoseControl.handPoseMode == HandPoseControl.HandPoseMode.CopyAnimOut)
                return true;
        }

        return false;
    }

    public void ExitCopyAnimationState()
    {
    if (leftHandPoseControl == null || rightHandPoseControl == null)
            return;
        leftHandPoseControl.ExitCopyAnimationState();
        rightHandPoseControl.ExitCopyAnimationState();     
        animHandsTransform.fallowVRCamera = true;

    }
    public void AddHandPoseControl(HandPoseControl control, bool isLeftHand)
    {
        if (!isLeftHand)
            rightHandPoseControl = control;
        else
            leftHandPoseControl = control;
    }

    private void Start()
    {
        mainCameraTransform = transform.Find("Camera Offset/Main Camera");
        animHandsAnimator = animHandsTransform.transform.GetComponentInChildren<Animator>();
    }
    
    private void Update()
    {
    }

    public void UpdateWalkToGroup(string WTGName)
    {
        currentWTGName = WTGName;
    }

}
