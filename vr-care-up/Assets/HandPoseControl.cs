using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandPoseControl : MonoBehaviour
{
    private float poseTransitionDuration = 0.2f;
    private HandPoseData handPose;
    private Vector3 startingHandPosition;
    private Vector3 finalHandPosition;
    private Quaternion startingHandRotation;
    private Quaternion finalHandRotation;

    private Quaternion[] startingFingerRotations;
    private Quaternion[] finalFingerRotations;

    private void Start()
    {
        handPose = GetComponent<HandPoseData>();
    }

    public void SetupPose(HandPoseData newHandPoseData)
    {
        handPose.animator.enabled = false;
        SetHandDataValues(newHandPoseData);
        StartCoroutine(SetHandDataRoutine(finalHandPosition, finalHandRotation, finalFingerRotations, startingHandPosition, startingHandRotation, startingFingerRotations));
    }

    public void UnSetPose()
    {
            handPose.animator.enabled = true;
            StartCoroutine(SetHandDataRoutine(startingHandPosition, startingHandRotation, startingFingerRotations, finalHandPosition, finalHandRotation, finalFingerRotations));
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

    public IEnumerator SetHandDataRoutine(Vector3 newPosition, Quaternion newRotation, Quaternion[] newBonesRotation, 
        Vector3 startingPosition, Quaternion startingRotation, Quaternion[] startingBonesRotation)
    {
        float timer = 0;
        while(timer < poseTransitionDuration)
        {
            Vector3 p = Vector3.Lerp(startingPosition, newPosition, timer / poseTransitionDuration);
            Quaternion r = Quaternion.Lerp(startingRotation, newRotation, timer / poseTransitionDuration);

            handPose.root.localPosition = p;
            handPose.root.localRotation = r;
            for (int i = 0; i < newBonesRotation.Length; i++)
            {
                handPose.fingerBones[i].localRotation = Quaternion.Lerp(startingBonesRotation[i], newBonesRotation[i], timer / poseTransitionDuration);
            }

            timer += Time.deltaTime;
            yield return null;
        }
    }
}
