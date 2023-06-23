using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimHandsTransform : MonoBehaviour
{
    public Transform targetVRObject;
    public Transform targetRightHand;
    public Transform targetLeftHand;
    public GameObject rightHandRootBone;
    public GameObject leftHandRootBone;
    public Transform[] rightFingerBones; 
    public Transform[] leftFingerBones;
    public bool fallowVRCamera = true;

    private Transform toolHoldBoneLeft;
    private Transform toolHoldBoneRight;

    public Animator animator;
    void Start()
    {
        toolHoldBoneRight = rightHandRootBone.transform.Find("toolHolder.R");
        toolHoldBoneLeft = leftHandRootBone.transform.Find("toolHolder.L");
    }

    void Update()
    {
        if (targetVRObject != null)
        {
            if (fallowVRCamera)
            {
                // transform.position = targetVRObject.position;
                Vector3 VRHandsMidPoint = targetRightHand.position - (targetRightHand.position - targetLeftHand.position) * 0.5f;
                Vector3 boneHandsMidPoint = rightHandRootBone.transform.position -
                    (rightHandRootBone.transform.position - leftHandRootBone.transform.position) * 0.5f;
                transform.position = transform.position + (VRHandsMidPoint - boneHandsMidPoint);

                Vector3 newRotation = new Vector3(transform.eulerAngles.x, targetVRObject.eulerAngles.y, transform.eulerAngles.z);
                
                transform.eulerAngles = newRotation;
            }
        }

    }
}
