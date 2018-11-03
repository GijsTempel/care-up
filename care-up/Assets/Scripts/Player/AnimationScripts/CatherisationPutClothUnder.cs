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
                inv.DropLeftObject();
            }

            if (inv.RightHandObject != null)
            {
                inv.DropRightObject();
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
            HandsInventory.GhostPosition posNormalGauze = new HandsInventory.GhostPosition();
            posNormalGauze.objectName = "GauzeTrayWet";
            posNormalGauze.position = new Vector3(9.24f, 1.096f, 12.1548f);
            posNormalGauze.rotation = new Vector3(-90f, 0f, 0f);
            posNormalGauze.id = 1;
            inv.customGhostPositions.Add(posNormalGauze);

            HandsInventory.GhostPosition posNewGauze = new HandsInventory.GhostPosition();
            posNewGauze.objectName = "GauzeTrayWet";
            posNewGauze.position = new Vector3(10.187f, 0.748f, 9.2253f);
            posNewGauze.rotation = new Vector3(-90f, 0f, 87.576f);
            posNewGauze.id = 2;
            inv.customGhostPositions.Add(posNewGauze);

            HandsInventory.GhostPosition posNormalTrash = new HandsInventory.GhostPosition();
            posNormalTrash.objectName = "PlasticTrashbucket";
            posNormalTrash.position = new Vector3(9.2011f, 1.0979f, 11.265f);
            posNormalTrash.rotation = new Vector3(-90f, 0f, -90f);
            posNormalTrash.id = 1;
            inv.customGhostPositions.Add(posNormalTrash);

            HandsInventory.GhostPosition posNewTrash = new HandsInventory.GhostPosition();
            posNewTrash.objectName = "PlasticTrashbucket";
            posNewTrash.position = new Vector3(9.9253f, 0.742f, 9.1915f);
            posNewTrash.rotation = new Vector3(-90f, 0f, 90f);
            posNewTrash.id = 2;
            inv.customGhostPositions.Add(posNewTrash);
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
