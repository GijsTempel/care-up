using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class GrabHandPose : MonoBehaviour
{
    public float poseTransitionDuration = 0.2f;
    public HandPoseData righHandPose;
    public HandPoseData leftHandPose;

    private void Start()
    {
        XRGrabInteractable grabInteractable = GetComponent<XRGrabInteractable>();
        grabInteractable.selectEntered.AddListener(SetupPose);
        grabInteractable.selectExited.AddListener(UnSetPose);
        if (righHandPose != null)
            righHandPose.gameObject.SetActive(false);
        if (leftHandPose != null)
            leftHandPose.gameObject.SetActive(false);
    }

    public void SetupPose(BaseInteractionEventArgs arg)
    {
        if (arg.interactorObject is XRDirectInteractor)
        {
            HandPoseData handData = arg.interactorObject.transform.GetComponentInChildren<HandPoseData>();
            if (handData.handType == HandPoseData.HandModelType.Right)
                handData.GetComponent<HandPoseControl>().SetupPose(righHandPose, poseTransitionDuration);
            else
                handData.GetComponent<HandPoseControl>().SetupPose(leftHandPose, poseTransitionDuration);
        }
    }

    public void UnSetPose(BaseInteractionEventArgs arg)
    {
        if (arg.interactorObject is XRDirectInteractor)
        {
            HandPoseControl handPoseControl = arg.interactorObject.transform.GetComponentInChildren<HandPoseControl>();
            if (handPoseControl != null)
                handPoseControl.UnSetPose();

        }
    }
   
#if UNITY_EDITOR

    [MenuItem("Tool/R Mirror Selected Right Grab Pose")]
    public static void MirrorRightPose()
    {
        GrabHandPose handPose = Selection.activeGameObject.GetComponent<GrabHandPose>();
        if (handPose != null)
            handPose.MirrorPose(handPose.leftHandPose, handPose.righHandPose);
    }
    [MenuItem("Tool/L Mirror Selected Left Grab Pose")]
    public static void MirrorLefttPose()
    {
        GrabHandPose handPose = Selection.activeGameObject.GetComponent<GrabHandPose>();
        if (handPose != null)
            handPose.MirrorPose(handPose.righHandPose, handPose.leftHandPose);
    }
#endif
    public void MirrorPose(HandPoseData poseToMirror, HandPoseData poseUsedToMirror)
    {
        Vector3 mirroredPosition = poseUsedToMirror.root.localPosition;
        mirroredPosition.x *= -1;
        Quaternion mirroredRotation = poseUsedToMirror.root.localRotation;
        mirroredRotation.y *= -1;
        mirroredRotation.z *= -1;

        poseToMirror.root.localPosition = mirroredPosition;
        poseToMirror.root.localRotation = mirroredRotation;
        for (int i = 0; i < poseUsedToMirror.fingerBones.Length; i++)
        {
            poseToMirror.fingerBones[i].localRotation = poseUsedToMirror.fingerBones[i].localRotation;
        }

    }
}
