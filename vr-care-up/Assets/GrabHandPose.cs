using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit ;


public class GrabHandPose : MonoBehaviour
{
    
    public float poseTransitionDuration = 0.2f;
    public HandPoseControl righHandPose;
    public HandPoseControl leftHandPose;
    private Vector3 startingHandPosition;
    private Vector3 finalHandPosition;
    private Quaternion startingHandRotation;
    private Quaternion finalHandRotation;

    private Quaternion[] startingFingerRotations;
    private Quaternion[] finalFingerRotations;


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
        Debug.Log(arg.interactorObject.transform.gameObject.name);
        if (arg.interactorObject is XRDirectInteractor)
        {

            HandPoseControl handData = arg.interactorObject.transform.GetComponentInChildren<HandPoseControl>();
            handData.animator.enabled = false;
            if (handData.handType == HandPoseControl.HandModelType.Right)
            {
                SetHandDataValues(handData, righHandPose);
            }
            else
            {
                SetHandDataValues(handData, leftHandPose);
            }
            StartCoroutine(SetHandDataRoutine(handData, finalHandPosition, finalHandRotation, finalFingerRotations, startingHandPosition, startingHandRotation, startingFingerRotations));

        }

    }

    public void SetHandDataValues(HandPoseControl h1, HandPoseControl h2)
    {
        //Debug.Log(h1.transform.parent.name);
        //Debug.Log(h2.transform.parent.name);

        startingHandPosition = h1.root.localPosition;
        finalHandPosition = h2.root.localPosition;
        finalHandPosition.x *= h2.root.transform.parent.localScale.x;
        finalHandPosition.y *= h2.root.transform.parent.localScale.y;
        finalHandPosition.z *= h2.root.transform.parent.localScale.z;


        startingHandRotation = h1.root.localRotation;
        finalHandRotation = h2.root.localRotation;

        startingFingerRotations = new Quaternion[h1.fingerBones.Length];
        finalFingerRotations = new Quaternion[h1.fingerBones.Length];

        for (int i = 0; i < h1.fingerBones.Length; i++)
        {
            startingFingerRotations[i] = h1.fingerBones[i].localRotation;
            finalFingerRotations[i] = h2.fingerBones[i].localRotation;
        }
    }

    public void SetHandData(HandPoseControl h, Vector3 newPosition, Quaternion newRotation, Quaternion[] newBonesRotation)
    {
        Debug.Log(h.transform.parent.name);

        h.root.localPosition = newPosition;
        h.root.localRotation = newRotation;
        for (int i = 0; i < newBonesRotation.Length; i++)
        {
            h.fingerBones[i].localRotation = newBonesRotation[i];
        }
    }

    public IEnumerator SetHandDataRoutine(HandPoseControl h, Vector3 newPosition, Quaternion newRotation, Quaternion[] newBonesRotation, 
        Vector3 startingPosition, Quaternion startingRotation, Quaternion[] startingBonesRotation)
    {
        float timer = 0;
        while(timer < poseTransitionDuration)
        {
            Vector3 p = Vector3.Lerp(startingPosition, newPosition, timer / poseTransitionDuration);
            Quaternion r = Quaternion.Lerp(startingRotation, newRotation, timer / poseTransitionDuration);

            h.root.localPosition = p;
            h.root.localRotation = r;
            for (int i = 0; i < newBonesRotation.Length; i++)
            {
                h.fingerBones[i].localRotation = Quaternion.Lerp(startingBonesRotation[i], newBonesRotation[i], timer / poseTransitionDuration);
            }

            timer += Time.deltaTime;
            yield return null;
        }
    }

    public void UnSetPose(BaseInteractionEventArgs arg)
    {
        if (arg.interactorObject is XRDirectInteractor)
        {
            HandPoseControl handData = arg.interactorObject.transform.GetComponentInChildren<HandPoseControl>();
            handData.animator.enabled = true;
            StartCoroutine(SetHandDataRoutine(handData, startingHandPosition, startingHandRotation, startingFingerRotations, finalHandPosition, finalHandRotation, finalFingerRotations));

        }
    }
}
