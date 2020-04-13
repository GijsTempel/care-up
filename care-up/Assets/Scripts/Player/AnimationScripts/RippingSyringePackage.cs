using UnityEngine;

public class RippingSyringePackage : AnimationCombine
{
    public bool hand;

    public int removeBottomFrame;
    public int dropBottomFrame;

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
        
        if (PlayerAnimationManager.CompareFrames(frame, prevFrame, removeBottomFrame))
        {
            inv.ReplaceHandObject(!hand, "fraxiPackageTop");

            inv.CreateAnimationObject("Frexi_with_needle_cap", hand);
        }
        else if (PlayerAnimationManager.CompareFrames(frame, prevFrame, dropBottomFrame))
        {
            inv.FreezeObject(!hand);
        }
        else if (PlayerAnimationManager.CompareFrames(frame, prevFrame, combineFrame))
        {
            inv.DeleteAnimationObject();
        }

        base.OnStateUpdate(animator, stateInfo, layerIndex);
    } 
}
