using UnityEngine;

public class PaperAndPenAnimation : CinematicAnimation
{
    public int pickPenFrame;
    public int dropPenFrame;

    HandsInventory inv;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);

        inv = GameObject.Find("GameLogic").GetComponent<HandsInventory>();
        frame = 0;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (PlayerAnimationManager.CompareFrames(frame, prevFrame, pickPenFrame))
        {
            inv.ForcePickItem("Pen", false);
        }
        
        if (PlayerAnimationManager.CompareFrames(frame, prevFrame, dropPenFrame))
        {
            inv.PutAllOnTable();
        }

        base.OnStateUpdate(animator, stateInfo, layerIndex);
    }   
}
