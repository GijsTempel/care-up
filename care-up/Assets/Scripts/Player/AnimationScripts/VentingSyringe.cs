using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VentingSyringe : AnimationUseOn
{
    public bool hand;

    public int plungerStart;
    public int plungerEnd;
    private int currentFrame = 0;

    private Syringe syringe;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);

        syringe = hand ? inv.LeftHandObject.GetComponent<Syringe>() : inv.RightHandObject.GetComponent<Syringe>();

        currentFrame = 0;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (currentFrame == plungerStart)
        {
            syringe.updatePlunger = true;
        }

        if (currentFrame == plungerEnd)
        {
            syringe.updatePlunger = false;
        }

        if (animator.speed != 0)
        {
            ++currentFrame;
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateExit(animator, stateInfo, layerIndex);

        syringe.updatePlunger = false;
    }
}