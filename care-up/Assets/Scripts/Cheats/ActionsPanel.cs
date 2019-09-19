using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CareUp.Actions;

public class ActionsPanel : MonoBehaviour {
    bool slideState = false;
    int lastStepId = -1;

    public enum Mode
    {
        ShortDescr,
        Type,
        Comment,
        CommentUA,
        Icons,
        Requirements
    };
    public void UpdatePanel()
    {
        lastStepId = -1;
    }

    public ActionsPanel.Mode mode;
    ActionManager am;
	// Use this for initialization
	void Start () {
        if (GameObject.FindObjectOfType<ActionManager>() != null)
        {
            am = GameObject.FindObjectOfType<ActionManager>();
        }
        buildActionsList();
    }


    public void SetMode(int _mode) 
    {
        mode = (ActionsPanel.Mode)_mode;
        lastStepId = -1;
        print(mode.ToString());
    }


	// Update is called once per frame
	void Update () {
        if (am != null)
        {
            if (lastStepId != am.CurrentActionIndex)
            {
                Transform content = transform.Find("ActionsList/Viewport/Content").transform;
                foreach (ActionStepButton ab in content.GetComponentsInChildren<ActionStepButton>())
                {
                    ab.updateLook(am.CurrentActionIndex);
                    //print(am.CurrentActionIndex);
                    lastStepId = am.CurrentActionIndex;
                }
            }
        }
    }

    public void toggleSlide()
    {
        slide(!slideState);
    }

    public void buildActionsList()
    {
        Transform content = transform.Find("ActionsList/Viewport/Content").transform;
        if (am != null)
        {
            foreach (Action a in am.actionList)
            {
                GameObject ActionStep = GameObject.Instantiate(Resources.Load<GameObject>("NecessaryPrefabs/UI/ActionStepButton"), content);
                ActionStep.GetComponent<ActionStepButton>().setAction(a);
              
            }
        }
    }

    public void slide(bool value)
    {
        if (slideState != value)
        {
            slideState = value;
            if (value)
                GetComponent<Animator>().SetTrigger("in");
            else
                GetComponent<Animator>().SetTrigger("out");

        }
    }
}
