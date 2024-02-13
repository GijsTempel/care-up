using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CareUp.Actions;
using TMPro;

public class ActionManagerStepTest : MonoBehaviour
{
    public ActionManager.ActionType actionType;
    public TextMeshProUGUI descrText;
    public TextMeshProUGUI typeText;
    public GameObject greenHighlite;
    public string leftObj;
    public string rightObj;
    public GameObject checkMarkImage;
    Action action;
    float compliteTime = -1f;


    private static ActionHandler actionHandle = null;

    public void Start()
    {
        if (actionHandle == null)
        {
            actionHandle = GameObject.FindObjectOfType<ActionHandler>();
        }
    }

    public void ActionButtonClicked()
    {
        if (actionHandle != null)
        {
            actionHandle.TryExecuteAction(actionType, leftObj, rightObj);
        }
    }
    public Action GetAction()
    {
        return action;
    }

    public float GetComplitTime()
    {
        return compliteTime;
    }

    public void SetCompliteTime(float _time)
    {
        compliteTime = _time;
    }
    public void SetCheckmark(bool toSet = true)
    {
        checkMarkImage.SetActive(toSet);
    }

    public void UpdateLook(int currentIndex)
    {
        greenHighlite.SetActive(currentIndex == action.SubIndex);
    }

    public void SetAction(Action a)
    {
        action = a;
        action.ObjectNames(out string[] objectNames);
        descrText.text = "[" + action.sequentialNumber.ToString() + "] " + 
            action.SubIndex.ToString() + " " +
            objectNames[0] + " " + "\n" + action.shortDescr;
        typeText.text = action.Type.ToString();
        actionType = action.Type;

        if (actionType == ActionManager.ActionType.PersonTalk)
        {
            leftObj = action._topic;
        }
        else
        {

            leftObj = objectNames[0];
            if (objectNames.Length > 1)
                rightObj = objectNames[1];  
        }
    }
}
