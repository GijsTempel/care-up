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
    private PlayerScript player;
    private Vector3 finalRootBonePosition;
    private Quaternion finalRootBoneRotation;
    private float handDataRoutineTime = float.PositiveInfinity;
    private void Start()
    {
        player = GameObject.FindObjectOfType<PlayerScript>();
        handPose = GetComponent<HandPoseData>();
    }

    void Update()
    {
        // Debug.Log("@" + name + "_timer:" + handDataRoutineTime.ToString());

        // Debug.Log("@" + name + "_handMode:" + handPoseMode.ToString());
        if (handPoseMode != HandPoseMode.Default && handDataRoutineTime < float.PositiveInfinity)
        {
            if (handPoseMode == HandPoseMode.CopyAnimIn)
                UpdateCopyAnimationData();
            SetHandDataRoutine();
        }

        // if (copyAnimation)
        // {
        //     if (handPoseMode != HandPoseMode.CopyAnimIn)
        //     {
        //         SetupCopyAnimationData();
        //     }
        //     else
        //     {
        //         ExitCopyAnimationState();
        //     }
        //         copyAnimation = false;
        // }
    }

    public void ExitCopyAnimationState()
    {
        handPoseMode = HandPoseMode.CopyAnimOut;
        handPose.animator.enabled = true;
        handDataRoutineTime = 0f;
    }

    public void SetupCopyAnimationData()
    {
        handPose.animator.enabled = false;
        handPoseMode = HandPoseMode.CopyAnimIn;
        handDataRoutineTime = 0f;
        poseTransitionDuration = 0.5f;

        startingHandPosition = handPose.root.localPosition;

        startingHandRotation = handPose.root.localRotation;
        finalHandRotation = startingHandRotation;

        startingFingerRotations = new Quaternion[handPose.fingerBones.Length];
        finalFingerRotations = new Quaternion[handPose.fingerBones.Length];

        for (int i = 0; i < handPose.fingerBones.Length; i++)
        {
            startingFingerRotations[i] = handPose.fingerBones[i].localRotation;
        }
    }

    public void UpdateCopyAnimationData()
    {
        if (animHandsTransform == null)
        {
            return;
        }

        GameObject animHandRootBone = animHandsTransform.rightHandRootBone;
        Transform[] targetFingers = animHandsTransform.rightFingerBones;
        if (handPose.handType == HandPoseData.HandModelType.Left)
        {
            animHandRootBone = animHandsTransform.leftHandRootBone;
            targetFingers = animHandsTransform.leftFingerBones;
        }
        if (animHandRootBone != null)
        {

            Debug.Log("@" + name + "_rand:" + Random.RandomRange(0, 9999).ToString());
            finalRootBonePosition = animHandRootBone.transform.position;;
            finalRootBoneRotation = animHandRootBone.transform.rotation;
            finalHandPosition = startingHandPosition;
            startingHandRotation = handPose.root.localRotation;
            finalHandRotation = startingHandRotation;
            for (int i = 0; i < handPose.fingerBones.Length; i++)
            {
                Quaternion rot = targetFingers[i].localRotation;
                if (handPose.handType == HandPoseData.HandModelType.Left)
                {
                    rot.y *= -1;
                    rot.z *= -1;
                }
                finalFingerRotations[i] = rot;
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
        float lerpValue = handDataRoutineTime / poseTransitionDuration;
        
        Vector3 p = Vector3.Lerp(startingHandPosition, finalHandPosition, lerpValue);
        Quaternion r = Quaternion.Lerp(startingHandRotation, finalHandRotation, lerpValue);
        if (handPoseMode == HandPoseMode.TransitOut || handPoseMode == HandPoseMode.CopyAnimOut)
        {
            p = Vector3.Lerp(finalHandPosition, startingHandPosition, lerpValue);
            r = Quaternion.Lerp(finalHandRotation, startingHandRotation, lerpValue);
        }

        handPose.root.localPosition = p;
        handPose.root.localRotation = r;
        for (int i = 0; i < finalFingerRotations.Length; i++)
        {
            if (handPoseMode == HandPoseMode.TransitIn || handPoseMode == HandPoseMode.CopyAnimIn)
                handPose.fingerBones[i].localRotation = Quaternion.Lerp(startingFingerRotations[i], finalFingerRotations[i], lerpValue);
            if (handPoseMode == HandPoseMode.TransitOut || handPoseMode == HandPoseMode.CopyAnimOut)
                handPose.fingerBones[i].localRotation = Quaternion.Lerp(finalFingerRotations[i], startingFingerRotations[i],  lerpValue);
        }
        if (handPoseMode == HandPoseMode.CopyAnimIn)
        {
            handPose.rootBone.position = Vector3.Lerp(handPose.rootBone.position, finalRootBonePosition, lerpValue);
            handPose.rootBone.rotation = Quaternion.Lerp(handPose.rootBone.rotation, finalRootBoneRotation, lerpValue);
        }
        if (handPoseMode == HandPoseMode.CopyAnimOut)
        {
            handPose.rootBone.localPosition = Vector3.Lerp(handPose.rootBone.localPosition, handPose.GetBaseRootBonePosition(), lerpValue);
            handPose.rootBone.localRotation = Quaternion.Lerp(handPose.rootBone.localRotation, handPose.GetBaseRootBoneRotation(), lerpValue);
        }


        handDataRoutineTime += Time.deltaTime;
        if (handDataRoutineTime > poseTransitionDuration && 
            (handPoseMode == HandPoseMode.TransitIn || handPoseMode == HandPoseMode.TransitOut || handPoseMode == HandPoseMode.CopyAnimOut))
            {
                handPoseMode = HandPoseMode.Default;
            }
    }
}
