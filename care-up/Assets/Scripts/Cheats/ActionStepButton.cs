using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CareUp.Actions;
using UnityEngine.UI;


public class ActionStepButton : MonoBehaviour {
    Action action;
	void Start () {
	}

    public void setAction(Action a)
    {
        action = a;

    }

    public void updateLook(int currentIndex)
    {
        if (action == null)
            return;

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
