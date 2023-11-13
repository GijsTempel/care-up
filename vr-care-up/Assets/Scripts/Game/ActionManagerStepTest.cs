using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionManagerStepTest : MonoBehaviour
{
    public ActionManager.ActionType actionType;
    public string leftObj;
    public string rightObj;

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
}
