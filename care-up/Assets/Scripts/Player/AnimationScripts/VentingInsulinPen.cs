using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VentingInsulinPen : AnimationUseOn
{
    public bool hand;

    public int buttonStart;
    public int buttonEnd;

    public int button2Start;
    public int button2End;

    private float frame = 0f;
    private float prevFrame = 0f;

    private InsulinPen pen;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);
        pen = hand ? inv.LeftHandObject.GetComponent<InsulinPen>() : inv.RightHandObject.GetComponent<InsulinPen>();

        frame = 0f;
        prevFrame = 0f;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (PlayerAnimationManager.CompareFrames(frame, prevFrame, buttonStart))
        {
            pen.animateButton = true;
        }

        if (PlayerAnimationManager.CompareFrames(frame, prevFrame, buttonEnd))
        {
            pen.animateButton = false;
        }

        if (PlayerAnimationManager.CompareFrames(frame, prevFrame, button2Start))
        {
            pen.animateButton = true;
        }

        if (PlayerAnimationManager.CompareFrames(frame, prevFrame, button2End))
        {
            pen.animateButton = false;
        }

        if (animator.speed != 0)
        {
            prevFrame = frame;
            frame = stateInfo.normalizedTime * stateInfo.length;
        }

        base.OnStateUpdate(animator, stateInfo, layerIndex);
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateExit(animator, stateInfo, layerIndex);

        pen.animateButton = false;
    }
}