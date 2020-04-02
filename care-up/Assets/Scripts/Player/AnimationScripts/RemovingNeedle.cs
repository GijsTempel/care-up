using UnityEngine;

public class RemovingNeedle : CinematicAnimation
{
    public int removeNeedleFrame;
    public int decombineFrame;
    public bool leftHand;

    private bool absorption;
    private Syringe syringe;

    HandsInventory inv;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);

        inv = GameObject.Find("GameLogic").GetComponent<HandsInventory>();

        syringe = leftHand ? inv.LeftHandObject.GetComponent<Syringe>() : inv.RightHandObject.GetComponent<Syringe>();
        syringe.updateProtector = true;

        absorption = syringe.name == "SyringeWithAbsorptionNeedle";
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateUpdate(animator, stateInfo, layerIndex);

        if (PlayerAnimationManager.CompareFrames(frame, prevFrame, removeNeedleFrame))
        {
            inv.DeleteAnimationObject();
        }

        if (PlayerAnimationManager.CompareFrames(frame, prevFrame, decombineFrame))
        {
            inv.ReplaceHandObject(leftHand, "Syringe");
            inv.CreateAnimationObject((absorption ? "AbsorptionNeedleNoCap" : "InjectionNeedleNoCap"), !leftHand);
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateExit(animator, stateInfo, layerIndex);

        syringe.updateProtector = false;
    }  
}
