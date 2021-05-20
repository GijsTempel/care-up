using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cinematic : StateMachineBehaviour
{
    public string target;
    public string resetToTarget = "";

    public bool resetCamera = false;
    protected CameraMode mode;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        mode = GameObject.Find("GameLogic").GetComponent<CameraMode>();
        if (GameObject.Find(target) == null)
        {
            Debug.Log("Cinematic target can't be found -- " + target);
        }
        else
        {
            mode.SetCinematicMode(GameObject.Find(target).transform);
        }
        if (resetCamera)
        {
            PlayerAnimationManager.SetSavedCameraOrientation(Camera.main.transform.localRotation);
            Camera.main.transform.localRotation = Quaternion.Euler(Vector3.zero);
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (resetCamera)
            Camera.main.transform.localRotation = PlayerAnimationManager.GetSavedCameraOrientation();

        mode.animationEnded = true;
        if (resetToTarget != "")
        {
            if (GameObject.Find(resetToTarget) != null)
            {
                mode.ResetPlayerToTarget(GameObject.Find(resetToTarget).transform);
            }
        }

    }

   
}
