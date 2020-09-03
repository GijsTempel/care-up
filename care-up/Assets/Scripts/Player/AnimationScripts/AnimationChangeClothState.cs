using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationChangeClothState : StateMachineBehaviour
{
    public PlayerAnimationManager.Hand clothHand;

    public List<int> crumbled = new List<int>();
    public List<int> folded = new List<int>();

    private HandsInventory inv;
    private ClothObject cloth;

    private float frame;
    private float prevFrame;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // get Cloth for later use
        GameObject gameLogic = GameObject.Find("GameLogic");
        if (gameLogic != null)
        {
            inv = gameLogic.GetComponent<HandsInventory>();

            if (inv != null)
            {
                GameObject gameObject = (clothHand == PlayerAnimationManager.Hand.Left)
                    ? inv.LeftHandObject : inv.RightHandObject;

                if (gameObject != null)
                {
                    cloth = gameObject.GetComponent<ClothObject>();
                    if (cloth == null)
                    {
                        Debug.LogWarning(clothHand + " hand does not have ClothObject.");
                    }
                }
                else
                {
                    Debug.LogWarning(clothHand + " hand is empty.");
                }
            }
            else
            {
                Debug.LogWarning("No HandsInventory found.");
            }
        }
        else
        {
            Debug.LogWarning("No GameLogic found.");
        }
        
        // set up variable for comparison
        frame = 0f;
        prevFrame = 0f;

        // check if any frames are set to 0, do immediately
        //
        // check crumbled frames and make cloth crumbled
        foreach (int i in crumbled)
        {
            if (i == 0)
            {
                cloth.ChangeState(ClothObject.ClothHoldState.Crumpled, true);
            }
        }

        // check folded frames and make cloth folded
        foreach (int i in folded)
        {
            if (i == 0)
            {
                cloth.ChangeState(ClothObject.ClothHoldState.Folded, true);
            }
        }
    }

	// OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
	override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (animator.speed != 0)
        {
            // check crumbled frames and make cloth crumbled
            foreach (int i in crumbled)
            {
                if (PlayerAnimationManager.CompareFrames(frame, prevFrame, i))
                {
                    cloth.ChangeState(ClothObject.ClothHoldState.Crumpled, true);
                }
            }

            // check folded frames and make cloth folded
            foreach (int i in folded)
            {
                if (PlayerAnimationManager.CompareFrames(frame, prevFrame, i))
                {
                    cloth.ChangeState(ClothObject.ClothHoldState.Folded, true);
                }
            }

            // update variables
            prevFrame = frame;
            frame = stateInfo.normalizedTime * stateInfo.length;
        }
    }

	// OnStateExit is called when a transition ends and the state machine finishes evaluating this state
	//override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}

	// OnStateMove is called right after Animator.OnAnimatorMove(). Code that processes and affects root motion should be implemented here
	//override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}

	// OnStateIK is called right after Animator.OnAnimatorIK(). Code that sets up animation IK (inverse kinematics) should be implemented here.
	//override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
	//
	//}
}
