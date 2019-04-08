using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cinematic : StateMachineBehaviour
{
    public string target;
    public bool resetCamera = false;
    protected CameraMode mode;
    public bool ExitCinematicAtEnd = false;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        mode = GameObject.Find("GameLogic").GetComponent<CameraMode>();
        mode.SetCinematicMode(GameObject.Find(target).transform);
        if (resetCamera)
        {
            Camera.main.transform.localRotation = Quaternion.Euler(Vector3.zero);
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        mode.animationEnded = true;
        if (ExitCinematicAtEnd)
        {
            mode.ToggleCameraMode(CameraMode.Mode.Free);
        }
    }

   
}
