using UnityEngine;

public class SwitchCameras : StateMachineBehaviour {

    public string cameraName;
    public int switchFrame;
    public int backFrame;

    protected float frame;
    protected float prevFrame;
    private Camera playerCamera = null;
    private Camera switchCamera = null;


    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        foreach (Camera c in GameObject.FindObjectsOfType<Camera>())
        {
            if (c.transform.parent != null)
            {
                if (c.transform.parent.name == "Head")
                    playerCamera = c;
            }
            if (c.name == cameraName)
                switchCamera = c;
        }
        if (backFrame > switchFrame && switchFrame == 0)
        {
            switchCam(false);
        }
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
    
        if (animator.speed != 0)
        {
            if (switchCamera != null && playerCamera != null && backFrame > switchFrame)
            {
                
                if (PlayerAnimationManager.CompareFrames(frame, prevFrame, switchFrame))
                    switchCam(false);

                if (PlayerAnimationManager.CompareFrames(frame, prevFrame, backFrame) && playerCamera != null)
                    switchCam(true);

            }
            prevFrame = frame;
            frame = stateInfo.normalizedTime * stateInfo.length;
        }
    }

    void switchCam(bool toMain)
    {
        if (switchCamera != null && playerCamera != null)
        {
            switchCamera.enabled = !toMain;
            playerCamera.enabled = toMain;
        }
    }

    //OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    //override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //}  
}
