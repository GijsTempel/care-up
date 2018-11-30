using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowOnFrame : StateMachineBehaviour
{

    public string ControlObjectName = "";
    public int showFrame;
    private GameObject Obj = null;
    public List<string> ObjNames;
    public bool toShow = true;

    protected float frame;
    protected float prevFrame;


    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (PlayerAnimationManager.CompareFrames(frame, prevFrame, showFrame))
        {
            if (ControlObjectName == "")
            {
                if (GameObject.FindObjectOfType<ObjectsIDController>() != null && ObjNames.Count != 0)
                {
                    ObjectsIDController idCont = GameObject.FindObjectOfType<ObjectsIDController>();
                    foreach (string __name in ObjNames)
                    {
                        Obj = idCont.getFromHidden(__name);
                        if (Obj != null)
                        {
                            Obj.SetActive(toShow);
                        }
                    }
                }
            }
            else
            {

                if (GameObject.Find(ControlObjectName) != null && ObjNames.Count != 0)
                {
                    if (GameObject.Find(ControlObjectName).GetComponent<ExtraObjectOptions>() != null)
                    {
                        ExtraObjectOptions ControlObject = GameObject.Find(ControlObjectName).GetComponent<ExtraObjectOptions>();


                        foreach (string __name in ObjNames)
                        {
                            ControlObject._show(__name, toShow);

                        }
                    }
                }

            }
        }

        if (animator.speed != 0)
        {
            prevFrame = frame;
            frame += Time.deltaTime;
        }

    }


}
