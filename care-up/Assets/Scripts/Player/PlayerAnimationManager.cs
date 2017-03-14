﻿using UnityEngine;
using System.Collections;

/// <summary>
/// Handle animation changes of the player
/// </summary>
public class PlayerAnimationManager : MonoBehaviour {

    public float ikWeight = 1.0f;

    public static bool ikActive = false;

    private static Transform leftInteractObject;
    private static Transform rightInteractObject;

    private static Animator animationController;
    private static CameraMode cameraMode;

    void Start()
    {
        animationController = GetComponent<Animator>();
        if (animationController == null) Debug.LogError("Animator not found");

        cameraMode = GameObject.Find("GameLogic").GetComponent<CameraMode>();
        if (cameraMode == null) Debug.LogError("No camera mode");
    }

    private void Update()
    {
        if (animationController.GetNextAnimatorStateInfo(0).fullPathHash
            == Animator.StringToHash("Base Layer.Armature|IdleN"))
            cameraMode.animationEnded = true;
    }

    public static void PlayAnimation(string name, Transform target = null)
    {
        animationController.SetTrigger(name);

        if (target)
        {
            cameraMode.SetCinematicMode(target);
        }
        
        /*if (leftInteract)
        {
            leftInteractObject = leftInteract;
            if (rightInteract)
            {
                rightInteractObject = rightInteract;
            }
            else
            {
                rightInteractObject = leftInteract;
            }
        }*/
    }

    //a callback for calculating IK
    private void OnAnimatorIK(int layerIndex)
    {
        //if the IK is active, set the position and rotation directly to the goal. 
        if (ikActive)
        {
            // Set the right hand target position and rotation, if one has been assigned
            if (rightInteractObject != null)
            {
                animationController.SetIKPositionWeight(AvatarIKGoal.RightHand, ikWeight);
                animationController.SetIKPosition(AvatarIKGoal.RightHand, rightInteractObject.position);
            }

            if (leftInteractObject != null)
            {
                animationController.SetIKPositionWeight(AvatarIKGoal.LeftHand, ikWeight);
                animationController.SetIKPosition(AvatarIKGoal.LeftHand, leftInteractObject.position);
            }
        }
        //if the IK is not active, set the position and rotation of the hand and head back to the original position
        else
        {
            animationController.SetIKPositionWeight(AvatarIKGoal.RightHand, 0);
            animationController.SetIKPositionWeight(AvatarIKGoal.LeftHand, 0);
        }
    }
}
