using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionExpectant : MonoBehaviour
{
    private ActionHandler actionHandler;
    public bool isCurrentAction = false;
    public ActionManager.ActionType actionType = ActionManager.ActionType.None;
    public string leftActionManagerObject = "";
    public string rightActionManagerObject = "";

    void Start()
    {
        UpdateAction();
    }

    public bool TryExecuteAction()
    {
        if (actionHandler == null)
            actionHandler = GameObject.FindObjectOfType<ActionHandler>();
        return actionHandler.TryExecuteAction(actionType, leftActionManagerObject, rightActionManagerObject);
    }
    
    
    public void UpdateAction()
    {
        if (actionHandler == null)
            actionHandler = GameObject.FindObjectOfType<ActionHandler>();
        
        isCurrentAction = actionHandler.CheckAction(actionType, leftActionManagerObject, rightActionManagerObject);
        Debug.Log(isCurrentAction);
    }
}
