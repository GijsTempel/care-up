using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubcatenousSequence : AnimationSequenceState
{
    public int takeSyringeFrame;
    public int takeClothFrame;

    private HandsInventory inv;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);

        inv = GameObject.Find("GameLogic").GetComponent<HandsInventory>();
        inv.PutAllOnTable();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (frame == takeClothFrame)
        {
            inv.CreateAnimationObject("Cloth", true);
        }
        else if (frame == takeSyringeFrame)
        {
            inv.ForcePickItem("SyringeWithInjectionNeedle", false);
            PlayerAnimationManager.SetHandItem(false, GameObject.Find("SyringeWithInjectionNeedle"));
            inv.RightHandObject.GetComponent<Syringe>().updatePlunger = true;
        }

        base.OnStateUpdate(animator, stateInfo, layerIndex);
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        inv.DeleteAnimationObject();

        base.OnStateExit(animator, stateInfo, layerIndex);

        if (inv.LeftHandObject && inv.LeftHandObject.GetComponent<Syringe>())
            inv.LeftHandObject.GetComponent<Syringe>().updatePlunger = false;
    }

    // OnStateMove is called right after Animator.OnAnimatorMove(). Code that processes and affects root motion should be implemented here
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK(). Code that sets up animation IK (inverse kinematics) should be implemented here.
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}
}
