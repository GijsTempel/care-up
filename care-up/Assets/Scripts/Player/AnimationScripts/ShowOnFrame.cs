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


    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        frame = 0f;
        prevFrame = 0f;
        if (showFrame == 0)
        {
            set_set();
        }
    }


    void set_set()
    {
        if (ControlObjectName == "")
        {
            if (GameObject.FindObjectOfType<ObjectsIDsController>() != null && ObjNames.Count != 0)
            {
                ObjectsIDsController idCont = GameObject.FindObjectOfType<ObjectsIDsController>();
                foreach (string __name in ObjNames)
                {
                    Obj = idCont.GetFromHidden(__name);
                    if (Obj != null)
                    {
                        Obj.SetActive(toShow);
                    }
                }
            }
        }
        else if (ControlObjectName == "-")
        {
            foreach (string __name in ObjNames)
            {
                if (GameObject.Find(__name) != null)
                {
                    GameObject.Find(__name).SetActive(toShow);
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

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
    
        if (animator.speed != 0)
        {
            if (PlayerAnimationManager.CompareFrames(frame, prevFrame, showFrame))
            {
                set_set();
            }

            prevFrame = frame;
            frame += Time.deltaTime;
        }

    }


}
