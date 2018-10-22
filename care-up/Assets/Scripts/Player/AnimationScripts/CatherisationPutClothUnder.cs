using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatherisationPutClothUnder : CinematicAnimation
{
    public int changeClothModelFrame;
    public int dropClothFrame;
    public Vector3 clothPosition;

    private HandsInventory inv;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);

        inv = GameObject.FindObjectOfType<HandsInventory>();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateUpdate(animator, stateInfo, layerIndex);

        if (PlayerAnimationManager.CompareFrames(frame, prevFrame, changeClothModelFrame))
        {
            if (inv.LeftHandObject != null)
            {
                inv.RemoveHandObject(true);
            }

            if (inv.RightHandObject != null)
            {
                inv.RemoveHandObject(false);
            }

            inv.CreateAnimationObject("cloth_02_inHands", false);
        }

        if (PlayerAnimationManager.CompareFrames(frame, prevFrame, dropClothFrame))
        {
            inv.DeleteAnimationObject();

            if (GameObject.Find("cloth_02"))
            {
                if (GameObject.Find("cloth_02").GetComponent<MeshRenderer>() != null)
                {
                    GameObject.Find("cloth_02").GetComponent<MeshRenderer>().enabled = true;
                }
            }

            // add new ghost positions for GauzeTrayFull
            HandsInventory.ItemPosition posNormal = new HandsInventory.ItemPosition();
            posNormal.objectName = "GauzeTrayWet";
            posNormal.position = new Vector3(9.24f, 1.096f, 12.1548f);
            posNormal.rotation = new Vector3(-90f, 0f, 0f);
            inv.customGhostPositions.Add(posNormal);

            HandsInventory.ItemPosition posNew = new HandsInventory.ItemPosition();
            posNew.objectName = "GauzeTrayWet";
            posNew.position = new Vector3(10.051f, 0.7351f, 9.2659f);
            posNew.rotation = new Vector3(-90f, 0f, 0f);
            inv.customGhostPositions.Add(posNew);
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
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
