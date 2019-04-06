using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CareUp.Actions;
using UnityEngine.UI;


public class ActionStepButton : MonoBehaviour {
    Action action;
    ActionsPanel.Mode lastMode;
    public Text main_text;
    ActionsPanel actionsPanel;
	void Start () {
        actionsPanel = GameObject.FindObjectOfType<ActionsPanel>();

    }

    public void setAction(Action a)
    {
        action = a;
        if (main_text != null)
            main_text.text = a.shortDescr;
    }

    public void updateLook(int currentIndex)
    {
        if (action == null)
            return;
        if (lastMode != actionsPanel.mode)
        {
            lastMode = actionsPanel.mode;
            if (lastMode == ActionsPanel.Mode.ShortDescr)
            {
                main_text.text = action.shortDescr;
            }
            else if (lastMode == ActionsPanel.Mode.Type)
            {
                string ss = action.Type.ToString();
                ss += "\n";
                string[] ObjectNames = new string[0];
                action.ObjectNames(out ObjectNames);
                foreach (string s in ObjectNames)
                {
                    ss += s + "  ";
                }
                main_text.text = ss;
            }
            else if (lastMode == ActionsPanel.Mode.Comment)
            {
                main_text.text = action.comment;
            }
        }
        int index = action.SubIndex;
        if (currentIndex == index)
        {
            GetComponent<Image>().color = new Color(0f, 1f, 0f, 0.4f);
        }
        else if (index < currentIndex)
        {
            GetComponent<Image>().color = new Color(.5f, .5f, .5f, 0.4f);
        }
        else
        {
            GetComponent<Image>().color = new Color(1f, .6f, 0f, 0.4f);
        }
    }

}
