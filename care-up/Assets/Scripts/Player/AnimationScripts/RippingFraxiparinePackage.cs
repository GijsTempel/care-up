using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RippingFraxiparinePackage : AnimationCombine
{
    public bool hand;

    public int openTopFrame;
    public int dropTopFrame;
    public int swapHandsFrame;
    public int removeBottomFrame;
    public int dropBottomFrame;
    public string GhostObjectTarget;
    GameObject obj;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        inv = GameObject.Find("GameLogic").GetComponent<HandsInventory>();
        mode = GameObject.Find("GameLogic").GetComponent<CameraMode>();

        inv.ToggleControls(true);

        frame = 0f;
        prevFrame = 0f;

        if (combineFrame == 0)
        {
            inv.ExecuteDelayedCombination();
        }

        mode.dontMoveCamera = true;
        mode.SetCinematicMode(GameObject.Find("TrashBucket").transform);
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (PlayerAnimationManager.CompareFrames(frame, prevFrame, openTopFrame))
        {
            AddFrexiPackageBottom();
        }
        else if (PlayerAnimationManager.CompareFrames(frame, prevFrame, dropTopFrame))
        {
            //inv.FreezeObject(!hand);
            inv.RemoveHandObject(!hand);
        }
        else if (PlayerAnimationManager.CompareFrames(frame, prevFrame, swapHandsFrame))
        {
            inv.SwapHands();
        }
        else if (PlayerAnimationManager.CompareFrames(frame, prevFrame, removeBottomFrame))
        {
            AddFrexiWithNeedleCap();

        }
        else if (PlayerAnimationManager.CompareFrames(frame, prevFrame, dropBottomFrame))
        {
            //inv.FreezeObject(!hand);
            inv.RemoveHandObject(!hand);

        }
        else if (PlayerAnimationManager.CompareFrames(frame, prevFrame, combineFrame))
        {
            inv.DeleteAnimationObject();
        }

        base.OnStateUpdate(animator, stateInfo, layerIndex);
    }

    async void AddFrexiPackageBottom()
    {
        await inv.CreateObjectByName("fraxiPackageBottom", Vector3.zero);
        inv.ForcePickItem("fraxiPackageBottom", !hand);

        inv.ReplaceHandObject(hand, "fraxiPackageSyringeBottom");
    }

    async void AddFrexiWithNeedleCap()
    {
        inv.ReplaceHandObject(!hand, "fraxiPackageTop");

        await inv.CreateObjectByName("Frexi_with_needle_cap", Vector3.zero, callback => obj = callback);
        inv.ForcePickItem("Frexi_with_needle_cap", hand);
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        inv.ToggleControls(false);
        mode.animating = false;
        mode.animationEnded = true;

        if (GameObject.FindObjectOfType<TutorialManager>() == null ||
            GameObject.FindObjectOfType<Tutorial_UI>() != null ||
            GameObject.FindObjectOfType<Tutorial_Theory>() != null)
        {
            RobotManager.SetUITriggerActive(true);
        }


        if (GameObject.Find(GhostObjectTarget) != null)
        {
            Transform targetObj = GameObject.Find(GhostObjectTarget).transform;
            obj.GetComponent<PickableObject>().InstantiateGhostObject(targetObj.position, targetObj.rotation, 0);
            bool isInList = false;
            HandsInventory.GhostPosition CGP = new HandsInventory.GhostPosition();
            CGP.position = targetObj.position;
            CGP.rotation = targetObj.rotation.eulerAngles;
            CGP.objectName = obj.name;
            if (inv.customGhostPositions.Count > 0)
            {
                for (int i = 0; i < inv.customGhostPositions.Count; i++)
                {
                    if (inv.customGhostPositions[i].objectName == obj.name)
                    {
                        isInList = true;
                        inv.customGhostPositions[i] = CGP;
                    }
                }
            }
            if (!isInList)
            {
                inv.customGhostPositions.Add(CGP);
            }
        }
    }


    //OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{

    //    if (GameObject.Find(GhostObjectTarget) != null)
    //    {
    //        Transform targetObj = GameObject.Find(GhostObjectTarget).transform;
    //        obj.GetComponent<PickableObject>().InstantiateGhostObject(targetObj.position, targetObj.rotation, 0);
    //        bool isInList = false;
    //        HandsInventory.GhostPosition CGP = new HandsInventory.GhostPosition();
    //        CGP.position = targetObj.position;
    //        CGP.rotation = targetObj.rotation.eulerAngles;
    //        CGP.objectName = obj.name;
    //        if (inv.customGhostPositions.Count > 0)
    //        {
    //            for (int i = 0; i < inv.customGhostPositions.Count; i++)
    //            {
    //                if (inv.customGhostPositions[i].objectName == obj.name)
    //                {
    //                    isInList = true;
    //                    inv.customGhostPositions[i] = CGP;
    //                }
    //            }
    //        }
    //        if (!isInList)
    //        {
    //            inv.customGhostPositions.Add(CGP);
    //        }

    //    }
    //    base.OnStateUpdate(animator, stateInfo, layerIndex);
    //}

    // OnStateMove is called right after Animator.OnAnimatorMove(). Code that processes and affects root motion should be implemented here
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK(). Code that sets up animation IK (inverse kinematics) should be implemented here.
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}
}
