using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacingNeedle : AnimationCombine {
    
    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        base.OnStateEnter(animator, stateInfo, layerIndex);

        HandsInventory inv = GameObject.Find("GameLogic").GetComponent<HandsInventory>();
        if (inv.LeftHandObject.name == "AbsorptionNeedle" ||
             inv.RightHandObject.name == "AbsorptionNeedle")
        {
            InjectionPatient patient = GameObject.FindObjectOfType<InjectionPatient>();
            if (patient != null)
            {
                patient.PutAbsorptionNeedleDialogue();
            }
        }
    }
}
