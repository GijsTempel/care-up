using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionExpectant : MonoBehaviour
{
    private ActionHandler actionHandler;
    public bool isCurrentAction = false;
    public ActionManager.ActionType actionType = ActionManager.ActionType.None;
    public string LeftActionManagerObject = "";
    public string RightActionManagerObject = "";

    void Start()
    {
        UpdateAction();
    }

    public void UpdateAction()
    {
        if (actionHandler == null)
            actionHandler = GameObject.FindObjectOfType<ActionHandler>();
        
        isCurrentAction = actionHandler.CheckAction(actionType, LeftActionManagerObject, RightActionManagerObject);
        Debug.Log(isCurrentAction);
    }
    
}
