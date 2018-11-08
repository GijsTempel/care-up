using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddNewGhostPositionsOnAnimation : StateMachineBehaviour
{
    public List<HandsInventory.GhostPosition> list = new List<HandsInventory.GhostPosition>();

	// OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        HandsInventory inv = GameObject.FindObjectOfType<HandsInventory>();
        
        foreach (HandsInventory.GhostPosition i in list)
        {
            inv.customGhostPositions.Add(i);
        }
    }
}
