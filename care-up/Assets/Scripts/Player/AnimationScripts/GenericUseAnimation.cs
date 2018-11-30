using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericUseAnimation : StateMachineBehaviour {

    protected HandsInventory inv;
    protected CameraMode mode;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {

        inv = GameObject.Find("GameLogic").GetComponent<HandsInventory>();
        inv.ToggleControls(true);

        mode = GameObject.Find("GameLogic").GetComponent<CameraMode>();
        mode.animating = true;

        RobotManager.SetUITriggerActive(false);
    }

	override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        if (animator.speed != 0)
        {

            inv.ToggleControls(true);
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

        inv.ToggleControls(false);
        mode.animating = false;
        mode.animationEnded = true;

        if (GameObject.FindObjectOfType<TutorialManager>() == null ||
            GameObject.FindObjectOfType<Tutorial_UI>() != null ||
            GameObject.FindObjectOfType<Tutorial_Theory>() != null)
        {
            RobotManager.SetUITriggerActive(true);
        }
    }

}
