﻿using UnityEngine;
using System.Collections;

/// <summary>
/// Handle animation changes of the player
/// </summary>
public class PlayerAnimationManager : MonoBehaviour
{
    [System.Serializable]
    public enum Hand
    {
        Right,
        Left
    };
    public int leftObjID = 0;
    public int rightObjID = 0;
    public int leftModifier01 = 0;
    public int rightModifier01 = 0;
    public float leftModifier02 = 0f;
    public float rightModifier02 = 0f;
    private Transform propL;
    private Transform propR;
    float syncSpeed = 0.01f;

    public float ikWeight = 1.0f;

    public static bool ikActive = false;

    private static Transform leftInteractObject;
    private static Transform rightInteractObject;

    private static Animator animationController;
    private static CameraMode cameraMode;
    private static HandsInventory handsInventory;
    private static PlayerScript playerScript;

    private static AnimationSequence animationSequence;

    void Start()
    {
        propL = GameObject.Find("prop.L").transform;
        propR = GameObject.Find("prop.R").transform;

        animationController = GetComponent<Animator>();
        if (animationController == null) Debug.LogError("Animator not found");

        cameraMode = GameObject.Find("GameLogic").GetComponent<CameraMode>();
        if (cameraMode == null) Debug.LogError("No camera mode");

        handsInventory = GameObject.Find("GameLogic").GetComponent<HandsInventory>();
        if (handsInventory == null) Debug.LogError("No handsInventory");

        playerScript = GameObject.FindObjectOfType<PlayerScript>();
        if (playerScript == null) Debug.LogError("No player");
    }
    
	public static void PlayCombineAnimation(int leftID, int rightID, Transform target = null)
    {
		animationController.SetInteger("leftID", leftID);
		animationController.SetInteger("rightID", rightID);
		animationController.SetTrigger("Combine");
		animationController.SetTrigger("S Combine");

        //if (name != "LeftPick" && name != "RightPick" &&
        //    name != "closeup_left" && name != "closeup_right" &&
        //    name != "faraway_left" && name != "faraway_right")
        //{
        //    animationController.SetTrigger("S " + name);
        //}
        playerScript.ResetFreeLook();
        if (target)
        {
            cameraMode.SetCinematicMode(target);
        }

        InteractableObject.ResetDescription();
    }

    void Update()
    {
        leftModifier02 = propL.localPosition.y;
        rightModifier02 = propR.localPosition.y;

        //Right and Left hands animation synchronization
        int RightAnimHash = animationController.GetCurrentAnimatorStateInfo(0).nameHash;
        int LeftAnimHash = animationController.GetCurrentAnimatorStateInfo(1).nameHash;

        float normalizedTime_R = animationController.GetCurrentAnimatorStateInfo(0).normalizedTime;
        float normalizedTime_L = animationController.GetCurrentAnimatorStateInfo(1).normalizedTime;
        float clipLength_R = animationController.GetCurrentAnimatorStateInfo(0).length;
        float clipLength_L = animationController.GetCurrentAnimatorStateInfo(1).length;
        float clipTime_R = clipLength_R * normalizedTime_R;
        float clipTime_L = clipLength_L * normalizedTime_L;

        if (RightAnimHash == LeftAnimHash
            && (clipTime_R > 0.35f && clipTime_R < (clipLength_R - 0.55f)
            && (clipTime_L > 0.35f && clipTime_L < (clipLength_L - 0.55f))))
        {

            //print(((normalizedTime_L - normalizedTime_R) * clipLength_R).ToString() + "  ||  " + normalizedTime_L.ToString() + "  " + normalizedTime_R.ToString());

            if (Mathf.Abs(clipTime_L - clipTime_R) < syncSpeed)
            {
                animationController.Play(LeftAnimHash, 1, normalizedTime_R + Time.deltaTime / clipLength_L);
            }
            else
            {
                float syncStep = syncSpeed;
                if ((normalizedTime_L - normalizedTime_R) > 0f)
                {
                    syncStep = -syncSpeed;
                }
                animationController.Play(LeftAnimHash, 1, (clipTime_L + (syncStep * 2f) + Time.deltaTime) / clipLength_L);
            }

        }


    }

    public static void PlayUseAnimation(int UseObjID, int UseOnID, Transform target = null)
    {
		animationController.SetInteger("leftID", UseObjID);
		animationController.SetInteger("rightID", UseOnID);
        animationController.SetTrigger("Use");
        animationController.SetTrigger("S Use");

        if (target)
        {
            cameraMode.SetCinematicMode(target);
        }

        InteractableObject.ResetDescription();
    }




    public static void PlayAnimation(string name, Transform target = null)
    {
        animationController.SetTrigger(name);

        if (name != "LeftPick" && name != "RightPick" &&
            name != "closeup_left" && name != "closeup_right" &&
            name != "faraway_left" && name != "faraway_right" &&
            name != "no")
        {
            animationController.SetTrigger("S " + name);
            playerScript.ResetFreeLook();
        }
        
        if (target)
        {
            cameraMode.SetCinematicMode(target);
        }

        InteractableObject.ResetDescription();
    }
    


    /// <summary>
    /// Sets the idle state of the hand, depending on object held.
    /// </summary>
    /// <param name="hand">True = left, False = right</param>
    /// <param name="item">Name of the item</param>
    public static void SetHandItem(bool hand, GameObject item)
    {
        string handName = hand ? "LeftHandState" : "RightHandState";
        int itemID = 0;

        if (item != null)
        {
            itemID = item.GetComponent<PickableObject>().holdAnimationID;
        }
        else itemID = 0;

        animationController.SetInteger(handName, itemID);
    }

    public static void PlayAnimationSequence(string name, Transform target)
    {
        cameraMode.dontMoveCamera = true;
        animationSequence = new AnimationSequence(name);
        cameraMode.cinematicToggle = true; //before play animation
        PlayAnimation(name + "Sequence", target);
    }

    public static void PlayAnimationSequence(string name)
    {
        animationSequence = new AnimationSequence(name);
    }

    public static void NextSequenceStep(bool flag)
    {
        if (flag)
        {
            if (animationSequence != null)
            {
                animationSequence.NextStep();
            }
        }
        else
        { 
            animationController.speed = 1f;
        }
    }

    public static void ToggleAnimationSpeed()
    {
        animationController.speed = (animationController.speed == 0) ? 1f : 0f;
    }

    public static void AbortSequence()
    {
        animationController.SetTrigger("AbortSequence");
        animationController.speed = 1f;
        handsInventory.DeleteAnimationObject();
        handsInventory.sequenceAborted = true;
    }

    public static void SequenceTutorialLock(bool value)
    {
        if (animationSequence != null)
        {
            animationSequence.TutorialLock(value);
        }
    }

    public static bool SequenceCompleted
    {
        get { return animationSequence != null ? animationSequence.Completed : true; }
    }

    public static bool CompareFrames(float currentFrame, float previousFrame, int compareFrame)
    {
        float targetFrame = compareFrame / 60f; // 60fps
        return (currentFrame >= targetFrame && previousFrame < targetFrame);

    }
}
