using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VentingSyringe : AnimationUseOn
{
    public bool hand;

    private Syringe syringe;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);

        syringe = hand ? inv.LeftHandObject.GetComponent<Syringe>() : inv.RightHandObject.GetComponent<Syringe>();

        syringe.updatePlunger = true;
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateExit(animator, stateInfo, layerIndex);

        syringe.updatePlunger = false;
    }
}
