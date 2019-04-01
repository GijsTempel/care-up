using UnityEngine;

public class SwitchCameras : StateMachineBehaviour {

    public string cameraName;
    public int switchFrame;
    public int backFrame;

    protected float frame;
    protected float prevFrame;
    private Camera playerCamera = null;
    
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (animator.speed != 0)
        {
            if (PlayerAnimationManager.CompareFrames(frame, prevFrame, switchFrame))
            {
                foreach (Camera unit in Camera.allCameras)
                {
                    if (unit.enabled)
                        playerCamera = unit;
                    if (unit.name == cameraName)
                        unit.enabled = true;
                    else
                        unit.enabled = false;

                }
            }

            if (PlayerAnimationManager.CompareFrames(frame, prevFrame, switchFrame) && playerCamera != null)
            {
                foreach (Camera unit in Camera.allCameras)
                {
                    unit.enabled = false;
                   

                }
                playerCamera.enabled = true;
            }
            prevFrame = frame;
            frame += Time.deltaTime;
        }       
    }

    //OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //}  
}
