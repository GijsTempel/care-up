using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class HandPoseControl : MonoBehaviour
{
    public enum HandPoseMode { Default, TransitIn, TransitOut, CopyAnimIn, CopyAnimOut, DynamicIn, DynamicOut}
    public Transform objectHolder;
    public HandPoseMode handPoseMode = HandPoseMode.Default;
    public bool copyAnimation = false;
    public AnimHandsTransform animHandsTransform;
    private float poseTransitionDuration = 0.2f;
    private HandPoseData handPose;
    private Vector3 finalHandPosition;
    private Quaternion finalHandRotation;
    private Quaternion[] startingFingerRotations;
    private Quaternion[] finalFingerRotations;
    private PlayerScript player;
    private Vector3 finalRootBonePosition;
    private Quaternion finalRootBoneRotation;
    private bool mirroredAnimation = false;
    private HandPoseData savedH2;
    private GameUIVR gameUIVR;
    public GameObject handWMesh;
    public GameObject handHoloMesh;


    float dynamicTransitionDistance = 0.09f;
    public float dynamicTransitionFactor = 0f;


    private float handDataRoutineTime = float.PositiveInfinity;
    private void Start()
    {
        gameUIVR = GameObject.FindObjectOfType<GameUIVR>();
        player = GameObject.FindObjectOfType<PlayerScript>();
        handPose = GetComponent<HandPoseData>();
    }

    void FindDynamicPoseToAct()
    {
        foreach(GrabHandPose g in GameObject.FindObjectsOfType<GrabHandPose>())
        {
            HandPoseData h = g.righHandPose;
            if (handPose.handType == HandPoseData.HandModelType.Left)
            {
                h = g.leftHandPose;
            }
            if (h != null)
            {
                float dist = Vector3.Distance(transform.position, h.transform.position);
                if (dist < dynamicTransitionDistance)
                {
                    StartDynamicTransition(h);
                }
                Debug.Log(dist);

            }
        }
    }

    void StartDynamicTransition(HandPoseData newHandPoseData)
    {
        SetupPose(newHandPoseData, 1f, HandPoseMode.DynamicIn);

    }

    void EndDynamicTransition()
    {

    }

    void Update()
    {
        //if (handPoseMode == HandPoseMode.Default)
        //    FindDynamicPoseToAct();
        

        if (copyAnimation)
        {
            copyAnimation = false;
            SetupCopyAnimationData(true);
        }
        if ((handPoseMode != HandPoseMode.Default && handDataRoutineTime < float.PositiveInfinity) || 
            handPoseMode == HandPoseMode.DynamicIn)
        {
            if (handPoseMode == HandPoseMode.CopyAnimIn)
                UpdateCopyAnimationData();
            SetHandDataRoutine();
        }
    }

    public void ExitCopyAnimationState(HandPoseData h2 = null, float newPoseTransitionDuration = 0.2f)
    {
        handPoseMode = HandPoseMode.CopyAnimOut;
        if (handPose.animator != null)
            handPose.animator.enabled = true;
        handDataRoutineTime = 0f;
        if (h2 != null)
            SetupPose(h2, newPoseTransitionDuration);
        // handHoloMesh.SetActive(false);
        // handWMesh.SetActive(true);

    }

    public void SetupCopyAnimationData(bool mirrored = false)
    {
        if (handPose.animator != null)
            handPose.animator.enabled = false;
        handPoseMode = HandPoseMode.CopyAnimIn;
        handDataRoutineTime = 0f;
        poseTransitionDuration = 0.5f;
        startingFingerRotations = new Quaternion[handPose.fingerBones.Length];
        finalFingerRotations = new Quaternion[handPose.fingerBones.Length];
        mirroredAnimation = mirrored;
        for (int i = 0; i < handPose.fingerBones.Length; i++)
        {
            startingFingerRotations[i] = handPose.fingerBones[i].localRotation;
        }
        // handHoloMesh.SetActive(true);
        // handWMesh.SetActive(false);
    }

    public void UpdateCopyAnimationData()
    {
        if (animHandsTransform == null)
            return;

        GameObject animHandRootBone = animHandsTransform.rightHandRootBone;
        Transform[] targetFingers = animHandsTransform.rightFingerBones;
        if ((handPose.handType == HandPoseData.HandModelType.Left && !mirroredAnimation) ||
            (handPose.handType == HandPoseData.HandModelType.Right && mirroredAnimation))
        {
            animHandRootBone = animHandsTransform.leftHandRootBone;
            targetFingers = animHandsTransform.leftFingerBones;
        }
        if (animHandRootBone != null)
        {
            finalRootBonePosition = animHandRootBone.transform.position;
            finalRootBoneRotation = animHandRootBone.transform.rotation;
            if (mirroredAnimation)
            {
                finalRootBonePosition = Vector3.Scale(Vector3.Reflect(
                    finalRootBonePosition - animHandsTransform.transform.position, animHandsTransform.transform.forward),
                    new Vector3(-1f, 1f, -1f)) + animHandsTransform.transform.position;
                //finalRootBonePosition.x *= -1;


                finalRootBoneRotation = Quaternion.LookRotation(Vector3.Reflect(finalRootBoneRotation * Vector3.forward, animHandsTransform.transform.right),
                    Vector3.Reflect(finalRootBoneRotation * Vector3.up, animHandsTransform.transform.right));

            }
            for (int i = 0; i < handPose.fingerBones.Length; i++)
            {
                Quaternion rot = targetFingers[i].localRotation;
                if (handPose.handType == HandPoseData.HandModelType.Left && !mirroredAnimation ||
                    handPose.handType == HandPoseData.HandModelType.Right && mirroredAnimation)
                {
                    rot.y *= -1;
                    rot.z *= -1;
                }
                finalFingerRotations[i] = rot;
            }
        }
    }
    private Quaternion ReflectRotation(Quaternion source, Vector3 normal)
    {
        return Quaternion.LookRotation(Vector3.Reflect(source * Vector3.forward, normal), Vector3.Reflect(source * Vector3.up, normal));
    }

    public void SetupPose(HandPoseData newHandPoseData, float newPoseTransitionDuration = 0.2f, 
        HandPoseMode nextHPMode = HandPoseMode.TransitIn)
    {
        savedH2 = newHandPoseData;
        poseTransitionDuration = newPoseTransitionDuration;
        if (handPose.animator != null)
            handPose.animator.enabled = false;
        handDataRoutineTime = 0f;
        
        handPoseMode = nextHPMode;
        if (nextHPMode == HandPoseMode.DynamicIn)
            SetHandDataValues(savedH2);
    }

    public void UnSetPose()
    {
        if (handPose.animator != null)
            handPose.animator.enabled = true;
        handDataRoutineTime = 0f;
        savedH2 = null;
        handPoseMode = HandPoseMode.TransitOut;

    }

    public void SetHandDataValues(HandPoseData h2, bool toUpdateStartPose = true)
    {
        finalHandRotation = Quaternion.Inverse(h2.root.localRotation);
        finalHandPosition = (finalHandRotation * -h2.root.localPosition) / h2.root.localScale.x;

        if (toUpdateStartPose)
            startingFingerRotations = new Quaternion[handPose.fingerBones.Length];
        finalFingerRotations = new Quaternion[handPose.fingerBones.Length];

        for (int i = 0; i < handPose.fingerBones.Length; i++)
        {
            if (toUpdateStartPose)
                startingFingerRotations[i] = handPose.fingerBones[i].localRotation;
            finalFingerRotations[i] = h2.fingerBones[i].localRotation;
        }
    }

    private void SetHandDataRoutine()
    {
        float lerpValue = handDataRoutineTime / poseTransitionDuration;
        bool toUpdateStartPose = true;
        if (handPoseMode == HandPoseMode.DynamicIn)
        {
            lerpValue = dynamicTransitionFactor;
            toUpdateStartPose = false;
        }
        if (savedH2 != null)
        {
            SetHandDataValues(savedH2, toUpdateStartPose);
        }
        
        objectHolder.localPosition = finalHandPosition;
        objectHolder.localRotation = finalHandRotation;
        bool toCopyPoseIn = handPoseMode == HandPoseMode.TransitIn || handPoseMode == HandPoseMode.CopyAnimIn ||
            handPoseMode == HandPoseMode.DynamicIn;
        bool toCopyPoseOut = handPoseMode == HandPoseMode.TransitOut || handPoseMode == HandPoseMode.CopyAnimOut ||
            handPoseMode == HandPoseMode.DynamicOut;
        for (int i = 0; i < finalFingerRotations.Length; i++)
        {
            if (toCopyPoseIn)
                handPose.fingerBones[i].localRotation = Quaternion.Lerp(startingFingerRotations[i], finalFingerRotations[i], lerpValue);
            if (toCopyPoseOut)
                handPose.fingerBones[i].localRotation = Quaternion.Lerp(finalFingerRotations[i], startingFingerRotations[i],  lerpValue);
        }
        if (handPoseMode == HandPoseMode.CopyAnimIn)
        {
            handPose.rootBone.position = Vector3.Lerp(handPose.rootBone.position, finalRootBonePosition, lerpValue);
            handPose.rootBone.rotation = Quaternion.Lerp(handPose.rootBone.rotation, finalRootBoneRotation, lerpValue);
        }
        else
        {
            handPose.rootBone.localPosition = Vector3.Lerp(handPose.rootBone.localPosition, handPose.GetBaseRootBonePosition(), lerpValue);
            handPose.rootBone.localRotation = Quaternion.Lerp(handPose.rootBone.localRotation, handPose.GetBaseRootBoneRotation(), lerpValue);
        }

        handDataRoutineTime += Time.deltaTime;
        if (handDataRoutineTime > poseTransitionDuration &&
            (handPoseMode == HandPoseMode.TransitIn || handPoseMode == HandPoseMode.TransitOut || handPoseMode == HandPoseMode.CopyAnimOut))
        {
            if (gameObject.GetComponent<HandPoseData>().handType == HandPoseData.HandModelType.Left && handPoseMode == HandPoseMode.CopyAnimOut)
                gameUIVR.UpdateHelpWitDelay(1f);

            handPoseMode = HandPoseMode.Default;
            savedH2 = null;
        }
    }
}
