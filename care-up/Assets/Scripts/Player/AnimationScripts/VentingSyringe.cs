using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VentingSyringe : AnimationUseOn
{
    public bool hand;

    public int plungerStart;
    public int plungerEnd;

    private float frame = 0f;
    private float prevFrame = 0f;

    private Syringe syringe;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);

        syringe = hand ? inv.LeftHandObject.GetComponent<Syringe>() : inv.RightHandObject.GetComponent<Syringe>();

        frame = 0f;
        prevFrame = 0f;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (animator.speed != 0)
        {
            prevFrame = frame;
            frame = stateInfo.normalizedTime * stateInfo.length;
        }

        if (PlayerAnimationManager.CompareFrames(frame, prevFrame, plungerStart))
        {
            syringe.updatePlunger = true;
        }

        if (PlayerAnimationManager.CompareFrames(frame, prevFrame, plungerEnd))
        {
            syringe.updatePlunger = false;
        }

        base.OnStateUpdate(animator, stateInfo, layerIndex);
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateExit(animator, stateInfo, layerIndex);

        syringe.updatePlunger = false;
    }
}