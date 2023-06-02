using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit ;


public class GrabHandPose : MonoBehaviour
{
    public HandPoseControl righHandPose;
    private Vector3 startingHandPosition;
    private Vector3 finalHandPosition;
    private Quaternion startingHandRotation;
    private Quaternion finalHandRotation;

    private Quaternion[] startingFingerRotation;
    private Quaternion[] finalFingerRotation;


    private void Start()
    {
        XRGrabInteractable grabInteractable = GetComponent<XRGrabInteractable>();
        grabInteractable.selectEntered.AddListener(SetupPose);
        grabInteractable.selectExited.AddListener(UnSetPose);
        righHandPose.gameObject.SetActive(false);
    }

    public void SetupPose(BaseInteractionEventArgs arg)
    {
        Debug.Log(arg.interactorObject.transform.gameObject.name);
        if (arg.interactorObject is XRDirectInteractor)
        {

            HandPoseControl handData = arg.interactorObject.transform.GetComponentInChildren<HandPoseControl>();
            handData.animator.enabled = false;
            SetHandDataValues(handData, righHandPose);
            SetHandData(handData, finalHandPosition, finalHandRotation, finalFingerRotation);

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

        startingFingerRotation = new Quaternion[h1.fingerBones.Length];
        finalFingerRotation = new Quaternion[h1.fingerBones.Length];

        for (int i = 0; i < h1.fingerBones.Length; i++)
        {
            startingFingerRotation[i] = h1.fingerBones[i].localRotation;
            finalFingerRotation[i] = h2.fingerBones[i].localRotation;
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



    public void UnSetPose(BaseInteractionEventArgs arg)
    {
        if (arg.interactorObject is XRDirectInteractor)
        {
            HandPoseControl handData = arg.interactorObject.transform.GetComponentInChildren<HandPoseControl>();
            handData.animator.enabled = true;
            SetHandData(handData, startingHandPosition, startingHandRotation, startingFingerRotation);

        }
    }
}
