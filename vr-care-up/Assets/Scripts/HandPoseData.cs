using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Hands.Samples.VisualizerSample;

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

    /*[SerializeField]
    GameObject debugJointPrefab;
    GameObject[] debugJoints;*/

    public Transform[] colliderBones;
    HandVisualizer.HandGameObjects m_HandGameObjects;

    private void Start()
    {
        baseRootBonePosition = rootBone.localPosition;
        baseRootBoneRotation = rootBone.localRotation;

        /*debugJoints = new GameObject[colliderBones.Length];
        for (int i = 0; i < debugJoints.Length; i++)
        {
            debugJoints[i] = Instantiate<GameObject>(debugJointPrefab, colliderBones[i]);
            debugJoints[i].transform.localScale = Vector3.one * 0.02f;
        }*/
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
    public void LateUpdate()
    {
        // surely in late update it's initialized properly finally?
        // confirmed
        if (m_HandGameObjects == null)
        {
            m_HandGameObjects = (handType == HandModelType.Left) ?
                GameObject.FindAnyObjectByType<HandVisualizer>().m_LeftHandGameObjects :
                GameObject.FindAnyObjectByType<HandVisualizer>().m_RightHandGameObjects;
        }
    }

    void Update()
    {
        if (m_HandGameObjects != null)
        {
            for (int i = 0; i < colliderBones.Length; i++)
            {
                colliderBones[i].position = m_HandGameObjects.m_DrawJoints[i].transform.position;
                colliderBones[i].rotation = m_HandGameObjects.m_DrawJoints[i].transform.rotation;
            }
        }
    }
}
