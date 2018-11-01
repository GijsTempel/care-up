using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowOnFrame : StateMachineBehaviour
{

    public int hideFrame;
    private GameObject Obj = null;
    public List<string> ObjNames;
    public bool toShow = true;

    protected float frame;
    protected float prevFrame;


    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (PlayerAnimationManager.CompareFrames(frame, prevFrame, hideFrame))
        {
            if (GameObject.FindObjectOfType<ObjectsIDController>() != null && ObjNames.Count != 0)
            {
                Debug.Log("ObjNames.Count ");
                ObjectsIDController idCont = GameObject.FindObjectOfType<ObjectsIDController>();
                foreach (string __name in ObjNames)
                {
                    Debug.Log(__name);
                    Obj = idCont.getFromHidden(__name);
                    if (Obj != null)
                    {
                        Obj.SetActive(true);
                    }
                }
            }
        }

        prevFrame = frame;
        frame += Time.deltaTime;

    }


}
