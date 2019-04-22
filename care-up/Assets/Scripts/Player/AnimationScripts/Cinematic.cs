using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cinematic : StateMachineBehaviour
{
    public string target;
    public bool resetCamera = false;
    protected CameraMode mode;

    protected float prevFrame;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        mode = GameObject.FindObjectOfType<CameraMode>();
        if (GameObject.Find(target) != null)
        {
            if (GameObject.Find(target).transform.Find("CinematicTarget") != null)
            {
                float dist = Vector3.Distance(GameObject.Find(target).transform.Find("CinematicTarget").transform.position,
                    animator.transform.position);
            }
        }
       
        if (resetCamera)
        {
            Camera.main.transform.localRotation = Quaternion.Euler(Vector3.zero);
        }
    }

   
}
