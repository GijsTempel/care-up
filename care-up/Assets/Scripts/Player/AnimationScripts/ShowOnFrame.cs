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
            SetObject();
        }
    }

    void SetObject()
    {
        if (ControlObjectName == "")
        {
            if (GameObject.FindObjectOfType<ObjectsIDsController>() != null && ObjNames.Count != 0)
            {
                ObjectsIDsController idCont = GameObject.FindObjectOfType<ObjectsIDsController>();
                foreach (string name in ObjNames)
                {
                    if (GameObject.Find(name) != null && !toShow)
                    {
                        GameObject.Find(name).SetActive(false);
                    }
                    else
                    {
                        Obj = idCont.GetFromHidden(name);
                        if (Obj != null)
                        {
                            Obj.SetActive(toShow);
                        }
                    }
                }
            }
        }
        else if (ControlObjectName == "-")
        {
            foreach (string name in ObjNames)
            {
                if (GameObject.Find(name) != null)
                {
                    GameObject.Find(name).SetActive(toShow);
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

                    foreach (string name in ObjNames)
                    {
                        ControlObject._show(name, toShow);
                    }
                }
            }
        }
        ActionManager.BuildRequirements();
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (animator.speed != 0)
        {
            prevFrame = frame;
            frame += Time.deltaTime;

            if (PlayerAnimationManager.CompareFrames(frame, prevFrame, showFrame))
            {
                SetObject();
            }
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (showFrame / 60f > frame)
        {
            SetObject();
        }
    }
}
