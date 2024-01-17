using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cinematic : StateMachineBehaviour
{
    public string target;
    public string resetToTarget = "";
    
    public bool resetCamera = false;

    public bool forceCloseEyes = false;
    // default value was always 0.5f, setting to 0 will have the same effect
    // this is the time taken by the transition from starting point to ending point
    // affects both position in rotation equally
    public float customTransitionTime = 0.5f; 

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
            mode.SetCinematicMode(GameObject.Find(target).transform, forceCloseEyes, customTransitionTime);
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
