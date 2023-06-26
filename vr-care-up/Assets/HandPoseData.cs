using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandPoseData : MonoBehaviour
{
    public enum HandModelType {  Left, Right}
    public HandModelType handType;
    public Transform root;
    public Animator animator;
    public Transform[] fingerBones; 
    public Transform rootBone;
    private Vector3 baseRootBonePosition;
    private Quaternion baseRootBoneRotation;

    private void Start()
    {
        baseRootBonePosition = rootBone.localPosition;
        baseRootBoneRotation = rootBone.localRotation;
    }

    public Vector3 GetBaseRootBonePosition()
    {
        return baseRootBonePosition;
    }

    public Quaternion GetBaseRootBoneRotation()
    {
        return baseRootBoneRotation;

    }
    

    public Vector3 GetParentOffset()
    {
        return transform.parent.parent.localPosition;
    }
     
}
