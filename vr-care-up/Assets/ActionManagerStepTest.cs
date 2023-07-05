using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionManagerStepTest : MonoBehaviour
{
    public ActionManager.ActionType actionType;
    public string leftObj;
    public string rightObj;


    public void ActionButtonClicked()
    {
        ActionHandler actionHandle = GameObject.FindObjectOfType<ActionHandler>();
        if (actionHandle != null)
        {
            actionHandle.TryExecuteAction(actionType, leftObj, rightObj);
        }

    }
}
