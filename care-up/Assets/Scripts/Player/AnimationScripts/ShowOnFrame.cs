﻿using System.Collections.Generic;
using UnityEngine;

public class ShowOnFrame : StateMachineBehaviour
{
    public string ControlObjectName = "";
    public int showFrame;
    private GameObject Obj = null;
    public List<string> ObjNames;
    public bool toShow = true;
    public bool meshRenderer = false;

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

    void ShowHideObj(GameObject _obj)
    {
        if (meshRenderer && (_obj.GetComponent<MeshRenderer>() != null || _obj.GetComponent<SkinnedMeshRenderer>() != null))
        {
            if (!toShow)
            {
                if (_obj.GetComponent<MeshRenderer>() != null)
                    _obj.GetComponent<MeshRenderer>().enabled = false;
                else
                    _obj.GetComponent<SkinnedMeshRenderer>().enabled = false;

            }
            else
            {
                foreach(MeshRenderer m in _obj.GetComponents<MeshRenderer>())
                {
                    m.enabled = true;
                }
                foreach (SkinnedMeshRenderer m in _obj.GetComponents<SkinnedMeshRenderer>())
                {
                    m.enabled = true;
                }
            }
        }
        else
        {
            _obj.SetActive(toShow);
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
                    if (GameObject.Find(name) != null)
                    {
                        ShowHideObj(GameObject.Find(name));
                    }
                    else
                    {
                        Obj = idCont.GetFromHidden(name);
                        if (Obj != null)
                        {
                            ShowHideObj(Obj);
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
                    ShowHideObj(GameObject.Find(name));
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
                        ControlObject._show(name, toShow, meshRenderer);
                    }
                }
            }
        }
        ActionManager.BuildRequirements();
        ActionManager.UpdateRequirements();
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (animator.speed != 0)
        {
            prevFrame = frame;
            frame = stateInfo.normalizedTime * stateInfo.length;

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
