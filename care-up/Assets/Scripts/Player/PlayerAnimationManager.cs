using UnityEngine;
using System.Collections;

/// <summary>
/// Handle animation changes of the player
/// </summary>
public class PlayerAnimationManager : MonoBehaviour {

    public float ikWeight = 1.0f;

    public static bool ikActive = false;

    private static Transform masterIK_hand;

    private static Transform leftInteractObject;
    private static Transform rightInteractObject;
    private static Transform leftHand;
    private static Transform rightHand;

    private static Animator animationController;

    void Start()
    {
        animationController = GetComponent<Animator>();
        if (animationController == null) Debug.LogError("Animator not found");

        masterIK_hand = GameObject.Find("masterIK_hand").transform;
        if (masterIK_hand == null) Debug.LogError("No master IK bone");

        leftHand = masterIK_hand.FindChild("IK_hand.L").transform;
        rightHand = masterIK_hand.FindChild("IK_hand.R").transform;
    }

    public static void PlayAnimation(string name, Transform leftInteract = null, Transform rightInteract = null)
    {
        animationController.SetTrigger(name);

        if (leftInteract)
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
        }
    }

    private void LateUpdate()
    {
        if (ikActive && leftInteractObject)
        {
            Debug.Log("up");
            masterIK_hand.position = leftInteractObject.position;
            masterIK_hand.rotation = leftInteractObject.rotation;
        }
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
                animationController.SetIKRotationWeight(AvatarIKGoal.RightHand, ikWeight);
                animationController.SetIKPosition(AvatarIKGoal.RightHand, rightHand.position);
                animationController.SetIKRotation(AvatarIKGoal.RightHand, rightHand.rotation);
                Debug.Log(masterIK_hand.FindChild("IK_hand.R").transform.position);
            }

            if (leftInteractObject != null)
            {
                animationController.SetIKPositionWeight(AvatarIKGoal.LeftHand, ikWeight);
                animationController.SetIKRotationWeight(AvatarIKGoal.LeftHand, ikWeight);
                animationController.SetIKPosition(AvatarIKGoal.LeftHand, leftHand.position);
                animationController.SetIKRotation(AvatarIKGoal.LeftHand, leftHand.rotation);
            }
        }
        //if the IK is not active, set the position and rotation of the hand and head back to the original position
        else
        {
            animationController.SetIKPositionWeight(AvatarIKGoal.RightHand, 0);
            animationController.SetIKRotationWeight(AvatarIKGoal.RightHand, 0);
            animationController.SetIKPositionWeight(AvatarIKGoal.LeftHand, 0);
            animationController.SetIKRotationWeight(AvatarIKGoal.LeftHand, 0);
            animationController.SetLookAtWeight(0);
        }
    }
}
