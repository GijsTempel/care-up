using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandPoseControl : MonoBehaviour
{
    public enum HandPoseMode { Default, TransitIn, TransitOut, CopyAnimIn, CopyAnimOut}

    private HandPoseMode handPoseMode = HandPoseMode.Default;
    public bool copyAnimation = false;
    public AnimHandsTransform animHandsTransform;
    private float poseTransitionDuration = 0.2f;
    private HandPoseData handPose;
    private Vector3 startingHandPosition;
    private Vector3 finalHandPosition;
    private Quaternion startingHandRotation;
    private Quaternion finalHandRotation;
    private Quaternion[] startingFingerRotations;
    private Quaternion[] finalFingerRotations;
    private float handDataRoutineTime = float.PositiveInfinity;
    private void Start()
    {
        handPose = GetComponent<HandPoseData>();
    }

    void Update()
    {
        if (handPoseMode != HandPoseMode.Default)
        {
            SetHandDataRoutine();
        }
        if (copyAnimation && animHandsTransform != null)
        {
            GameObject animHandRootBone = animHandsTransform.rightHandRootBone;
            Transform[] targetFingers = animHandsTransform.rightFingerBones;
            if (handPose.handType == HandPoseData.HandModelType.Left)
            {
                animHandRootBone = animHandsTransform.leftHandRootBone;
                targetFingers = animHandsTransform.leftFingerBones;
            }
            if (animHandRootBone != null)
            {
                handPose.rootBone.position = animHandRootBone.transform.position;
                handPose.rootBone.rotation = animHandRootBone.transform.rotation;
                for (int i = 0; i < handPose.fingerBones.Length; i++)
                {
                    handPose.fingerBones[i].localRotation = targetFingers[i].localRotation;
                }
            }
        }
    }

    public void SetupPose(HandPoseData newHandPoseData, float newPoseTransitionDuration = 0.2f)
    {
        poseTransitionDuration = newPoseTransitionDuration;
        handPose.animator.enabled = false;
        SetHandDataValues(newHandPoseData);
        handDataRoutineTime = 0f;
        handPoseMode = HandPoseMode.TransitIn;
    }

    public void UnSetPose()
    {
        handPose.animator.enabled = true;
        handDataRoutineTime = 0f;
        handPoseMode = HandPoseMode.TransitOut;
    }

    public void SetHandDataValues(HandPoseData h2)
    {
        startingHandPosition = handPose.root.localPosition;
        finalHandPosition = h2.root.localPosition;
        Vector3 parentOffset = handPose.GetParentOffset();
        finalHandPosition.x = (finalHandPosition.x * h2.root.transform.parent.localScale.x) - parentOffset.x;
        finalHandPosition.y = (finalHandPosition.y * h2.root.transform.parent.localScale.y) - parentOffset.y;
        finalHandPosition.z = (finalHandPosition.z * h2.root.transform.parent.localScale.z) - parentOffset.z;

        startingHandRotation = handPose.root.localRotation;
        finalHandRotation = h2.root.localRotation;

        startingFingerRotations = new Quaternion[handPose.fingerBones.Length];
        finalFingerRotations = new Quaternion[handPose.fingerBones.Length];

        for (int i = 0; i < handPose.fingerBones.Length; i++)
        {
            startingFingerRotations[i] = handPose.fingerBones[i].localRotation;
            finalFingerRotations[i] = h2.fingerBones[i].localRotation;
        }
    }

    private void SetHandDataRoutine()
    {
        Debug.Log("@" + name + "_ttime:" + (handDataRoutineTime / poseTransitionDuration).ToString());

        Vector3 p = Vector3.Lerp(startingHandPosition, finalHandPosition, handDataRoutineTime / poseTransitionDuration);
        Quaternion r = Quaternion.Lerp(startingHandRotation, finalHandRotation, handDataRoutineTime / poseTransitionDuration);
        if (handPoseMode == HandPoseMode.TransitOut)
        {
            p = Vector3.Lerp(finalHandPosition, startingHandPosition, handDataRoutineTime / poseTransitionDuration);
            r = Quaternion.Lerp(finalHandRotation, startingHandRotation, handDataRoutineTime / poseTransitionDuration);
        }

        handPose.root.localPosition = p;
        handPose.root.localRotation = r;
        for (int i = 0; i < finalFingerRotations.Length; i++)
        {
            if (handPoseMode == HandPoseMode.TransitIn)
                handPose.fingerBones[i].localRotation = Quaternion.Lerp(startingFingerRotations[i], finalFingerRotations[i], handDataRoutineTime / poseTransitionDuration);
            else
                handPose.fingerBones[i].localRotation = Quaternion.Lerp(finalFingerRotations[i], startingFingerRotations[i],  handDataRoutineTime / poseTransitionDuration);
        }
        handDataRoutineTime += Time.deltaTime;
        if (handDataRoutineTime > poseTransitionDuration)
            handPoseMode = HandPoseMode.Default;
}
}
